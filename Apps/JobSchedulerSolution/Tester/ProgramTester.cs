#region

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using JobSchedulerService.Clients;
using log4net;
using log4net.Config;

#endregion

namespace Tester
{
    internal class ProgramTester
    {
        #region Constructors

        public ProgramTester()
        {
            //Logger init
            FileInfo fileInfo = new FileInfo(@"config.log4net");
            XmlConfigurator.Configure(fileInfo);
            s_log.Debug("Application Started");

            m_jobSchedulerServiceHost.TheService.OnNewRequestJob += p_jobRequest => { Console.WriteLine("{0} / {1} ({2})", m_jobSchedulerServiceHost.TheService.JobsCompletedCounter, m_jobSchedulerServiceHost.TheService.JobsRequestedCounter, m_jobSchedulerServiceHost.TheService.JobsFailedCounter); };
            m_jobSchedulerServiceHost.TheService.OnJobCompleted += p_jobCompleted => { Console.WriteLine("{0} / {1} ({2})", m_jobSchedulerServiceHost.TheService.JobsCompletedCounter, m_jobSchedulerServiceHost.TheService.JobsRequestedCounter, m_jobSchedulerServiceHost.TheService.JobsFailedCounter); };
            m_jobSchedulerServiceHost.Init();
            Console.WriteLine("JobSchedulerService Host opened at: {0}", m_jobSchedulerServiceHost.TheServiceHost.Description.Endpoints[0].Address);
            s_log.InfoFormat("JobSchedulerService Host opened");

            //Task.Factory.StartNew(() => { m_workerClient1.Init(SERVICE_CONFIG_FILE, "Worker01", true); });
            //Task.Factory.StartNew(() => { m_workerClient2.Init(SERVICE_CONFIG_FILE, "Worker02", true); });
            Task.Factory.StartNew(() => { m_workerClient3.Init(SERVICE_CONFIG_FILE, "Worker03"); });
            Task.Factory.StartNew(() => { m_workerClient4.Init(SERVICE_CONFIG_FILE, "Worker04"); });
            Task.Factory.StartNew(() => { m_workerClient5.Init(SERVICE_CONFIG_FILE, "Worker05"); });

            Task.Factory.StartNew(() => { m_serviceClientA.Init(SERVICE_CONFIG_FILE, "Client01"); });
            Task.Factory.StartNew(() => { m_serviceClientB.Init(SERVICE_CONFIG_FILE, "Client02"); });
            //Task.Factory.StartNew(() => { m_serviceClientC.Init(SERVICE_CONFIG_FILE, "Client03", true); });
        }

        #endregion

        #region Private Methods

        private static void Main()
        {
            string s = "1234";
            s = "__" + s.Substring(2);
            new ProgramTester();
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            s_log.InfoFormat("Application exit");
        }

        #endregion

        #region Fields

        private readonly JobSchedulerServiceHost m_jobSchedulerServiceHost = new JobSchedulerServiceHost(SERVICE_CONFIG_FILE);

        private readonly WorkerClientTester m_workerClient1 = new WorkerClientTester();
        private readonly WorkerClientTester m_workerClient2 = new WorkerClientTester();
        private readonly WorkerClientTester m_workerClient3 = new WorkerClientTester();
        private readonly WorkerClientTester m_workerClient4 = new WorkerClientTester();
        private readonly WorkerClientTester m_workerClient5 = new WorkerClientTester();

        private readonly ServiceClientTester m_serviceClientA = new ServiceClientTester();
        private readonly ServiceClientTester m_serviceClientB = new ServiceClientTester();
        private readonly ServiceClientTester m_serviceClientC = new ServiceClientTester();

        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constants

        public const string SERVICE_CONFIG_FILE = "JobSchedulerService.config";

        #endregion
    }
}