#region

using System.ServiceModel;
using JobSchedulerService.Service;

#endregion

namespace JobSchedulerService.Clients
{
    public class JobSchedulerServiceHost
    {
        #region Constructors

        public JobSchedulerServiceHost(string p_serviceConfigFile)
        {
            TheService = new JobSchedulerServiceImplementation(p_serviceConfigFile);
        }

        #endregion

        #region Public Methods

        public void Init()
        {
            TheServiceHost = new ServiceHost(TheService);
            TheServiceHost.Open();
            TheService.Start();
        }

        #endregion

        #region Public Properties

        public JobSchedulerServiceImplementation TheService { get; set; }
        public ServiceHost TheServiceHost { get; set; }

        #endregion
    }
}