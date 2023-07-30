#region

using System.ServiceModel;
using JobSchedulerService.DataItems;

#endregion

namespace JobSchedulerService.Service
{
    public interface IJobSchedulerServiceCallback
    {
        #region Public Methods

        /// <summary>
        /// Service notification to client of completed job
        /// </summary>
        /// <param name="p_jobCompleted"></param>
        [OperationContract(IsOneWay = true)]
        void JobRunCompleted(JobCompleted p_jobCompleted);

        /// <summary>
        /// Service request worker to execute a job
        /// </summary>
        /// <param name="p_jobRequest"></param>
        [OperationContract(IsOneWay = true)]
        void RequestWorkerJobRun(JobRequest p_jobRequest);

        #endregion
    }
}