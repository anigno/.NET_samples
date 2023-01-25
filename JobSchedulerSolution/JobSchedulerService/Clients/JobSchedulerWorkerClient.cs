#region

using System.ServiceModel;
using System.ServiceModel.Description;
using JobSchedulerService.Service;

#endregion

namespace JobSchedulerService.Clients
{
    public class JobSchedulerWorkerClient
    {
        #region Constructors

        public JobSchedulerWorkerClient()
        {
            TheCallBack = new JobSchedulerServiceCallbackImplementation();
        }

        #endregion

        #region Public Methods

        public void Init(string p_serviceConfigFile, string p_workerId)
        {
            ServiceEndpoint serviceEndpoint = JobSchedulerClientCommon.GetSeviceEndpointFromConfig<IJobSchedulerService>(p_serviceConfigFile, JobSchedulerClientCommon.END_POINT_CONFIGURATION_NAME);
            TheDuplexChannelFactory = new DuplexChannelFactory<IJobSchedulerService>(TheCallBack, serviceEndpoint);
            TheServiceWorkerClient = TheDuplexChannelFactory.CreateChannel();
            TheServiceWorkerClient.RegisterWorker(p_workerId);
        }

        #endregion

        #region Public Properties

        public JobSchedulerServiceCallbackImplementation TheCallBack { get; set; }

        public IJobSchedulerService TheServiceWorkerClient { get; set; }
        public DuplexChannelFactory<IJobSchedulerService> TheDuplexChannelFactory { get; set; }

        #endregion
    }
}