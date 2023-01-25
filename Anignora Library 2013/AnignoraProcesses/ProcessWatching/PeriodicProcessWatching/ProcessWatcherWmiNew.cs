using System;
using System.Management;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AnignoraProcesses.ProcessWatching.PeriodicProcessWatching
{
    public class ProcessWatcherWmiNew:ProcessWatcherBase
    {

		#region (------  Enums  ------)

        public enum ProcessActivityEnum
        {
            Created,
            Deleted
        }

		#endregion (------  Enums  ------)

		#region (------  Fields  ------)


private readonly List<uint> _currentRunningProcessesList = new List<uint>();

        private readonly Dictionary<uint, ProcessData> _removedProcessedDictionary = new Dictionary<uint, ProcessData>();
        private readonly Dictionary<uint, ProcessData> _knownRunningProcessesDictionay = new Dictionary<uint, ProcessData>();
        private readonly ManagementObjectSearcher _managementObjectSearcherProcesses = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");
        private readonly Timer _periodicTimer = new Timer();

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ProcessWatcherWmiNew(int intervalMs)
        {
            _periodicTimer.Interval = intervalMs;
            _periodicTimer.Tick += _periodicTimer_Tick;
        }

		#endregion (------  Constructors  ------)

		#region (------  Overridden Properties  ------)

        public override double Interval
        {
            get { return _periodicTimer.Interval; }
            set { _periodicTimer.Interval=(int) value; }
        }

		#endregion (------  Overridden Properties  ------)


		#region (------  Delegates  ------)

        public delegate void ProcessWatcherWmiNewActivityEventHandler(ProcessData processData, ProcessActivityEnum activity);

		#endregion (------  Delegates  ------)

		#region (------  Event Handlers  ------)

        void _periodicTimer_Tick(object sender, EventArgs e)
        {
            CheckRunningProcessesData();
        }

		#endregion (------  Event Handlers  ------)

		#region (------  Overridden Methods  ------)

        public override event OnProcessActivityDelegate OnProcessInstanceCreated;
        public override event OnProcessActivityDelegate OnProcessInstanceDeleted;

        public override uint GetProcessInstances(string descriptor)
        {
            ManagementObjectSearcher searcher;
            uint cnt=0;
            try
            {
                 searcher= new ManagementObjectSearcher(
                    "root\\CIMV2", "SELECT ProcessId FROM Win32_Process WHERE CommandLine=\"" + descriptor + "\"");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    cnt++;
                }
            }
            catch(Exception ex)
            {
                cnt = 1;
                Console.WriteLine("exception={0} descriptor={1}",ex.Message,descriptor);
            }
            return cnt;
            
        }

        public override void RemoveProcessData(string descriptor)
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            _periodicTimer.Start();
        }

        public override void Stop()
        {
            _periodicTimer.Stop();
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Private Methods  ------)

        private string GetDateFromWmiDateString(string sDate)
        {
            return string.Format(@"{0}/{1}/{2} {3}:{4}:{5}",
                                 sDate.Substring(0,4),
                                 sDate.Substring(4,2),
                                 sDate.Substring(6,2),
                                 sDate.Substring(8,2),
                                 sDate.Substring(10,2),
                                 sDate.Substring(12,2)
                );
        }

		#endregion (------  Private Methods  ------)

		#region (------  Public Methods  ------)

        public void CheckRunningProcessesData()
        {
            lock (_knownRunningProcessesDictionay)
            {
                //Check for new processes
                _currentRunningProcessesList.Clear();
                _removedProcessedDictionary.Clear();
                foreach (ManagementObject queryObj in _managementObjectSearcherProcesses.Get())
                {
                    uint pid = (uint) queryObj["ProcessId"];
                    _currentRunningProcessesList.Add(pid);
                    if (!_knownRunningProcessesDictionay.ContainsKey(pid))
                    {
                        //process created
                        ProcessData pd = new ProcessData();
                        pd.ProcessId = (uint) queryObj["ProcessId"];
                        pd.Caption = (string) queryObj["Caption"];
                        pd.Name = (string) queryObj["Name"];
                        pd.Description = "";
                        pd.ExecutablePath = (string)queryObj["ExecutablePath"];
                        pd.ParentProcessId = (uint)queryObj["ParentProcessId"];
                        //Set commandline, possible null for idle and system processes
                        string commandLine=(string) queryObj["CommandLine"];
                        if(commandLine==null)commandLine=pd.Name;
                        pd.CommandLine = commandLine.ToLower();
                        //Set creation date, possible null for idle and system processes
                        string sDateTime = (string) queryObj["CreationDate"];
                        pd.CreationDateString = sDateTime == null ? "" :GetDateFromWmiDateString( sDateTime.Split('+')[0]);
                        _knownRunningProcessesDictionay.Add(pd.ProcessId, pd);
                        if (OnProcessInstanceCreated != null) OnProcessInstanceCreated(pd.CommandLine, pd.ProcessId);
                        if (OnProcessInstanceCreated != null) OnProcessInstanceCreated(pd.CommandLine,pd.ProcessId);
                    }
                }
                //Check for deleted processes
                foreach (KeyValuePair<uint, ProcessData> processData in _knownRunningProcessesDictionay)
                {
                    if (!_currentRunningProcessesList.Contains(processData.Key))
                    {
                        //process deleted
                        _removedProcessedDictionary.Add(processData.Key, processData.Value);
                    }
                }
                //Actually remove deleted processes
                foreach (KeyValuePair<uint, ProcessData> processData in _removedProcessedDictionary)
                {
                    ProcessData pd = processData.Value; //Nothing holds handled ProcessData after removal
                    _knownRunningProcessesDictionay.Remove(processData.Key);
                    if (OnProcessInstanceDeleted != null) OnProcessInstanceDeleted(pd.CommandLine, pd.ProcessId);
                }
            }
        }

        public void DeleteProcess(uint pid)
        {
            lock (_knownRunningProcessesDictionay)
            {
                _knownRunningProcessesDictionay.Remove(pid);
            }
        }

		#endregion (------  Public Methods  ------)

    }
}