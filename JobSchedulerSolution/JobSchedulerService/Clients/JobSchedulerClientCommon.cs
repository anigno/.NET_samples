#region

using System.Configuration;
using System.IO;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

#endregion

namespace JobSchedulerService.Clients
{
    public class JobSchedulerClientCommon
    {
        #region Public Methods

        public static ServiceEndpoint GetSeviceEndpointFromConfig<TServiceInterface>(string p_configFilename, string p_endpointConfigurationName)
        {
            //Read configuration
            if (!File.Exists(p_configFilename)) throw new FileNotFoundException("Config file not found " + p_configFilename);
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap {ExeConfigFilename = p_configFilename}, ConfigurationUserLevel.None);
            ConfigurationChannelFactory<TServiceInterface> configurationChannelFactory = new ConfigurationChannelFactory<TServiceInterface>(p_endpointConfigurationName, configuration, null);
            //Generate ServiceEndPoint
            ServiceEndpoint serviceEndpoint = configurationChannelFactory.Endpoint;
            return serviceEndpoint;
        }

        #endregion

        #region Constants

        public const string END_POINT_CONFIGURATION_NAME = "JobSchedulerServiceEndPoint";

        #endregion
    }
}