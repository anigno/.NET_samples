#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using JobSchedulerService.DataItems;
using log4net;

#endregion

namespace JobSchedulerService.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class JobSchedulerServiceImplementation : IJobSchedulerService
    {
        #region Constructors

        public JobSchedulerServiceImplementation(string p_serviceConfigFile)
        {
            ServiceConfigFile = p_serviceConfigFile;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (m_tokenSource != null && !m_tokenSource.IsCancellationRequested) m_tokenSource.Cancel(true);
            m_tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(JobExecutionTask, m_tokenSource.Token);
        }

        public void Stop()
        {
            m_tokenSource.Cancel(true);
        }

        public static void Configure(ServiceConfiguration p_config)
        {
            s_log.DebugFormat("{0}", p_config);
            if (!File.Exists(ServiceConfigFile)) throw new FileNotFoundException("Config file not found " + ServiceConfigFile);
            p_config.LoadFromConfiguration(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = ServiceConfigFile }, ConfigurationUserLevel.None));
        }

        public void ClientRequestJob(JobRequest p_jobRequest)
        {
            s_log.DebugFormat("{0}", p_jobRequest);
            Interlocked.Increment(ref m_jobsRequestedCounter);
            m_jobRequestsCollection.Add(p_jobRequest);
            OnNewRequestJob(p_jobRequest);
        }

        public void RegisterClient(string p_clientId)
        {
            s_log.DebugFormat("{0}", p_clientId);
            IJobSchedulerServiceCallback callback = OperationContext.Current.GetCallbackChannel<IJobSchedulerServiceCallback>();
            if (m_clientCallbacks.ContainsKey(p_clientId))
            {
                s_log.WarnFormat("Client: {0} already exist and will be replaced", p_clientId);
                m_clientCallbacks.Remove(p_clientId);
            }
            m_clientCallbacks.Add(p_clientId, callback);
            IEnumerable<JobCompleted> unsentCompletedJobs = m_completedJobsNotSent.Where(p_completed => p_completed.Request.RequestingClientId == p_clientId);
            foreach (JobCompleted jobCompletedIterator in unsentCompletedJobs)
            {
                JobCompleted jobCompleted = jobCompletedIterator;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        s_log.DebugFormat("Resending: {0}", jobCompleted);
                        jobCompleted.IsResend = true;
                        callback.JobRunCompleted(jobCompleted);
                        m_completedJobsNotSent.Remove(jobCompleted);
                    }
                    catch (Exception exception)
                    {
                        s_log.Error("Erro sending unsent JobCompleted", exception);
                    }
                });
            }
        }

        public void RegisterWorker(string p_workerId)
        {
            s_log.DebugFormat("{0}", p_workerId);
            IJobSchedulerServiceCallback callback = OperationContext.Current.GetCallbackChannel<IJobSchedulerServiceCallback>();
            if (m_workerCallbacks.ContainsKey(p_workerId))
            {
                s_log.WarnFormat("Worker: {0} already exist and will be replaced", p_workerId);
                IJobSchedulerServiceCallback value;
                m_workerCallbacks.TryRemove(p_workerId, out value);
            }
            m_workerCallbacks.TryAdd(p_workerId, callback);
            m_freeWorkersCollection.Add(p_workerId);
        }

        public void WorkerCompleted(JobCompleted p_jobCompleted)
        {
            s_log.DebugFormat("{0}", p_jobCompleted);
            m_freeWorkersCollection.Add(p_jobCompleted.WorkerId);
            if (!p_jobCompleted.IsSucceeded)
            {
                Interlocked.Increment(ref m_jobsFailedCounter);
                p_jobCompleted.Request.RetriesLeft--;
                if (p_jobCompleted.Request.RetriesLeft > 0)
                {
                    s_log.DebugFormat("Will retry: {0}", p_jobCompleted.Request);
                    m_jobRequestsCollection.Add(p_jobCompleted.Request);
                }
                else
                {
                    s_log.WarnFormat("Will not retry: {0}", p_jobCompleted.Request);
                }
            }
            else
            {
                Interlocked.Increment(ref m_jobsCompletedCounter);
            }
            SendJobCompletion(p_jobCompleted);
        }

        #endregion

        #region Public Properties

        public int JobsRequestedCounter
        {
            get { return m_jobsRequestedCounter; }
        }

        public int JobsCompletedCounter
        {
            get { return m_jobsCompletedCounter; }
        }

        public int JobsFailedCounter
        {
            get { return m_jobsFailedCounter; }
        }

        public static string ServiceConfigFile { get; private set; }

        #endregion

        #region Events

        public event Action<JobRequest> OnNewRequestJob = delegate { };
        public event Action<JobCompleted> OnJobCompleted = delegate { };

        public event Action<string, Exception> OnError = delegate { };

        #endregion

        #region Private Methods

        private void SendJobCompletion(JobCompleted p_jobCompleted)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    OnJobCompleted(p_jobCompleted);
                    m_clientCallbacks[p_jobCompleted.Request.RequestingClientId].JobRunCompleted(p_jobCompleted);
                }
                catch (Exception exception)
                {
                    s_log.Error("couldn't send JobCompleted: " + p_jobCompleted + " to client", exception);
                    OnError("couldn't send JobCompleted: " + p_jobCompleted + " to client", exception);
                    lock (m_completedJobsNotSent)
                    {
                        s_log.WarnFormat("Adding: {0} to CompletedJobsNotSent list", p_jobCompleted);
                        m_completedJobsNotSent.Add(p_jobCompleted);
                    }
                }
            });
        }

        private void JobExecutionTask()
        {
            try
            {
                foreach (JobRequest jobRequest in m_jobRequestsCollection.GetConsumingEnumerable(m_tokenSource.Token))
                {
                    JobRequest request = jobRequest;
                    s_log.DebugFormat("Waiting to start job: {0}", request);
                    string workerId = m_freeWorkersCollection.Take(m_tokenSource.Token);
                    s_log.DebugFormat("Worker: {0} will take the job: {1}", workerId, request);
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            m_workerCallbacks[workerId].RequestWorkerJobRun(request);
                        }
                        catch (Exception exception)
                        {
                            s_log.Error("couldn't send job to worker: " + workerId, exception);
                            OnError("JobExecutionTask(), couldn't send job to worker: " + workerId, exception);
                        }
                    });
                }
            }
            catch (OperationCanceledException)
            {
                s_log.InfoFormat("Operation cancelled");
            }
        }

        #endregion

        #region Fields

        private CancellationTokenSource m_tokenSource;

        private readonly Dictionary<string, IJobSchedulerServiceCallback> m_clientCallbacks = new Dictionary<string, IJobSchedulerServiceCallback>();

        private int m_jobsRequestedCounter = 0;
        private int m_jobsCompletedCounter = 0;
        private int m_jobsFailedCounter = 0;
        private readonly List<JobCompleted> m_completedJobsNotSent = new List<JobCompleted>();

        //private readonly List<string> m_workingWorkers = new List<string>();

        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly BlockingCollection<JobRequest> m_jobRequestsCollection = new BlockingCollection<JobRequest>();
        private readonly BlockingCollection<string> m_freeWorkersCollection = new BlockingCollection<string>();
        private readonly ConcurrentDictionary<string, IJobSchedulerServiceCallback> m_workerCallbacks = new ConcurrentDictionary<string, IJobSchedulerServiceCallback>();

        #endregion
    }
}