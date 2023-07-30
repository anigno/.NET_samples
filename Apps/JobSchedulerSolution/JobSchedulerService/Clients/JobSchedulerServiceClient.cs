#region

using System.ServiceModel;
using System.ServiceModel.Description;
using JobSchedulerService.Service;

#endregion

namespace JobSchedulerService.Clients
{
    public class JobSchedulerServiceClient
    {
        #region Constructors

        public JobSchedulerServiceClient()
        {
            TheCallBack = new JobSchedulerServiceCallbackImplementation();
        }

        #endregion

        #region Public Methods

        public void Init(string p_serviceConfigFile, string p_clientId)
        {
            ServiceEndpoint serviceEndpoint = JobSchedulerClientCommon.GetSeviceEndpointFromConfig<IJobSchedulerService>(p_serviceConfigFile, JobSchedulerClientCommon.END_POINT_CONFIGURATION_NAME);
            TheDuplexChannelFactory = new DuplexChannelFactory<IJobSchedulerService>(TheCallBack, serviceEndpoint);
            TheServiceClient = TheDuplexChannelFactory.CreateChannel();
            TheServiceClient.RegisterClient(p_clientId);
        }

        #endregion

        #region Public Properties

        public IJobSchedulerService TheServiceClient { get; set; }

        public JobSchedulerServiceCallbackImplementation TheCallBack { get; set; }

        public DuplexChannelFactory<IJobSchedulerService> TheDuplexChannelFactory { get; set; }

        #endregion
    }
}