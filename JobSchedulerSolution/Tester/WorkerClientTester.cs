#region

using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using JobSchedulerService.Clients;
using JobSchedulerService.DataItems;
using JobSchedulerService.Service;
using log4net;

#endregion

namespace Tester
{
    public class WorkerClientTester
    {
        #region Public Methods

        public void Init(string p_serviceConfigFile, string p_workerId, bool p_isBadWorker = false)
        {
            m_jobSchedulerWorkerClient.Init(p_serviceConfigFile, p_workerId);

            m_jobSchedulerWorkerClient.TheCallBack.OnRequestJobRun += p_jobRequest =>
            {
                //Run job
                Random random = new Random((int) DateTime.Now.Ticks);
                Task.Factory.StartNew(async () =>
                {
                    s_log.DebugFormat("Worker: {0} Starting job: {1}", p_workerId, p_jobRequest);
                    await Task.Delay(random.Next(300, 500));
                    JobCompleted jobCompleted = new JobCompleted() {WorkerId = p_workerId, Request = p_jobRequest, IsSucceeded = true};
                    if (p_isBadWorker) jobCompleted.IsSucceeded = false;
                    s_log.DebugFormat("Job: {0} finished", jobCompleted);
                    m_jobSchedulerWorkerClient.TheServiceWorkerClient.WorkerCompleted(jobCompleted);
                });
            };
        }

        #endregion

        #region Fields

        private readonly JobSchedulerWorkerClient m_jobSchedulerWorkerClient = new JobSchedulerWorkerClient();

        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}