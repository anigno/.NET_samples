#region

using System;
using System.Reflection;
using System.ServiceModel;
using JobSchedulerService.DataItems;
using log4net;

#endregion

namespace JobSchedulerService.Service
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class JobSchedulerServiceCallbackImplementation : IJobSchedulerServiceCallback
    {
        #region Public Methods

        public void RequestWorkerJobRun(JobRequest p_jobRequest)
        {
            s_log.DebugFormat("JobRequest: {0}", p_jobRequest);
            OnRequestJobRun(p_jobRequest);
        }

        public void JobRunCompleted(JobCompleted p_jobCompleted)
        {
            s_log.DebugFormat("JobCompleted :{0}", p_jobCompleted);
            OnJobCompleted(p_jobCompleted);
        }

        #endregion

        #region Events

        public event Action<JobRequest> OnRequestJobRun = delegate { };
        public event Action<JobCompleted> OnJobCompleted = delegate { };

        #endregion

        #region Fields

        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}