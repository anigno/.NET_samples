#region

using System.ServiceModel;
using JobSchedulerService.DataItems;

#endregion

namespace JobSchedulerService.Service
{
    [ServiceContract(CallbackContract = typeof (IJobSchedulerServiceCallback))]
    public interface IJobSchedulerService
    {
        #region Public Methods

        /// <summary>
        /// Client registration before requesting jobs
        /// </summary>
        /// <param name="p_clientId"></param>
        [OperationContract(IsOneWay = true)]
        void RegisterClient(string p_clientId);

        /// <summary>
        /// Client request job schedule
        /// </summary>
        /// <param name="p_jobRequest"></param>
        [OperationContract(IsOneWay = true)]
        void ClientRequestJob(JobRequest p_jobRequest);

        /// <summary>
        /// Worker registration to become avaliable for running
        /// </summary>
        /// <param name="p_workerId"></param>
        [OperationContract(IsOneWay = true)]
        void RegisterWorker(string p_workerId);

        /// <summary>
        /// Worker notification of completed job, succeeded or failed
        /// </summary>
        /// <param name="p_jobCompleted"></param>
        [OperationContract(IsOneWay = true)]
        void WorkerCompleted(JobCompleted p_jobCompleted);

        #endregion
    }
}