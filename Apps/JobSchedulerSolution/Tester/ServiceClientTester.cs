#region

using System;
using System.Reflection;
using System.Threading.Tasks;
using JobSchedulerService.Clients;
using JobSchedulerService.DataItems;
using log4net;

#endregion

namespace Tester
{
    public class ServiceClientTester
    {
        #region Public Methods

        public void Init(string p_serviceConfigFile, string p_clientId, bool p_isDisconnectClient = false)
        {
            m_jobSchedulerServiceClient.TheCallBack.OnJobCompleted += OnJobCompletedHandler;
            m_jobSchedulerServiceClient.Init(p_serviceConfigFile, p_clientId);
            Task.Factory.StartNew(async () =>
            {
                for (int a = 0; a < 5; a++)
                {
                    m_jobSchedulerServiceClient.TheServiceClient.ClientRequestJob(new JobRequest(p_clientId, 2,"MyJob"+a));
                    await Task.Delay(100);
                }
                if (p_isDisconnectClient)
                {
                    m_jobSchedulerServiceClient.TheDuplexChannelFactory.Close();
                    await Task.Delay(2000);
                    m_jobSchedulerServiceClient.Init(p_serviceConfigFile, p_clientId);
                }
            });
        }

        #endregion

        #region Private Methods

        private void OnJobCompletedHandler(JobCompleted p_jobCompleted)
        {
            s_log.DebugFormat("{0}", p_jobCompleted);
            Console.WriteLine("Job Completed " + p_jobCompleted);
        }

        #endregion

        #region Fields

        private readonly JobSchedulerServiceClient m_jobSchedulerServiceClient = new JobSchedulerServiceClient();

        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}