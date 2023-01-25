using System.Collections.Generic;
using System.Management;
using System;

namespace AnignoraProcesses.ProcessWatching.WmiProcessWatching
{
    public class WmiProcessMonitor
    {

		#region Fields (2) 


        private readonly Dictionary<string, List<uint>> _processDataList = new Dictionary<string, List<uint>>();

        private readonly ProcessWatcher _watcher = new ProcessWatcher();

		#endregion Fields 

		#region Constructors (1) 

        public WmiProcessMonitor()
        {
        }

		#endregion Constructors 

		#region Events (2) 

        /// <summary>
        /// Raises on new process instance created, event handler must use BeginInvoke to preventmonitor halting
        /// </summary>
        public event OnProcessActionDelegate OnProcessInstanceCreated;

        /// <summary>
        /// Raises on existing process instance deleted, event handler must use BeginInvoke to preventmonitor halting
        /// </summary>
        public event OnProcessActionDelegate OnProcessInstanceDeleted;

		#endregion Events 

		#region Event Handlers (2) 

        void _watcher_OnProcessCreated(string processFullPath, uint processId)
        {
            ProcessInstanceAdd(processFullPath, processId);
        }

        void _watcher_OnProcessDeleted(string processFullPath, uint processId)
        {
            ProcessInstanceRemove(processFullPath, processId);
        }

		#endregion Event Handlers 

		#region Private Methods (3) 

        private void ProcessInstanceAdd(string processFullPath, uint processId)
        {
            if (!_processDataList.ContainsKey(processFullPath))
            {
                List<uint> pids = new List<uint>();
                _processDataList.Add(processFullPath, pids);
            }
            _processDataList[processFullPath].Add(processId);
            if (OnProcessInstanceCreated != null) OnProcessInstanceCreated(processFullPath, processId);
        }

        private void ProcessInstanceRemove(string processFullPath, uint processId)
        {
            if (!_processDataList.ContainsKey(processFullPath))
            {
                Console.WriteLine(string.Format("Process: {0} pid: {1} not found is processes list", processFullPath, processId));
            }
            _processDataList[processFullPath].Remove(processId);
            if (_processDataList[processFullPath].Count == 0) _processDataList.Remove(processFullPath);
            if (OnProcessInstanceDeleted != null) OnProcessInstanceDeleted(processFullPath, processId);
        }

        private void ReadRunningProcesses()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                object executablePathObject=queryObj["ExecutablePath"];
                if (executablePathObject == null) continue;
                string executablePath=executablePathObject.ToString().ToLower();
                uint pid = (uint)queryObj["ProcessId"];
                ProcessInstanceAdd(executablePath, pid); 
            }
        }

		#endregion Private Methods 

		#region Public Methods (2) 

        public void Start()
        {
            ReadRunningProcesses();
            _watcher.OnProcessCreated += _watcher_OnProcessCreated;
            _watcher.OnProcessDeleted += _watcher_OnProcessDeleted;
            _watcher.Start();
        }

        public void Stop()
        {
            _watcher.Stop();
            _watcher.OnProcessCreated -= _watcher_OnProcessCreated;
            _watcher.OnProcessDeleted -= _watcher_OnProcessDeleted;
        }

		#endregion Public Methods 

    }
}