using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.ComponentModel;

namespace AnignoraProcesses.ProcessWatching.PeriodicProcessWatching
{
    public class PeriodicProcessWatcherFast : ProcessWatcherBase
    {

		#region (------  Fields  ------)


        private readonly List<int> _runningPids = new List<int>();

        private readonly Dictionary<int, string> _knownPids = new Dictionary<int, string>();
        private readonly object _syncRoot = new object();
        private readonly Timer _timer = new Timer(500);

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public PeriodicProcessWatcherFast()
        {
            lock (_syncRoot)
            {
                _timer.Elapsed += _timer_Elapsed;
            }
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)


        public override double Interval
        {
            get
            {
                lock (_syncRoot)
                {
                    return _timer.Interval;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _timer.Interval = value;
                    if (_timer.Enabled)
                    {
                        _timer.Stop();
                        _timer.Start();
                    }
                }
            }

        }


		#endregion (------  Properties  ------)

		#region (------  Event Handlers  ------)

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_syncRoot)
            {
                FillRunningPidsList();
                for (int a = 0; a < _runningPids.Count; a++)
                {
                    if (!_knownPids.ContainsKey(_runningPids[a]))
                    {
                        //new pid
                        if (_runningPids[a] > 10) //Check if not system process, pid<10
                        {
                            try
                            {
                                Process p=Process.GetProcessById(_runningPids[a]);
                                if (p.StartInfo.Arguments != "")
                                {
                                }
                                string fileName = p.MainModule.FileName.ToLower();
                                _knownPids.Add(_runningPids[a], fileName);
                                if(OnProcessInstanceCreated!=null)OnProcessInstanceCreated(fileName, (uint)_runningPids[a]);
                            }
                            catch (Win32Exception)
                            {
                                //System process, no main module
                            }
                        }
                    }
                }
                Dictionary<int, string> removeList = new Dictionary<int, string>();
                foreach (KeyValuePair<int, string> pair in _knownPids)
                {
                    if (!_runningPids.Contains(pair.Key))
                    {
                        //pid was deleted
                        removeList.Add(pair.Key,pair.Value);
                    }
                }
                foreach (KeyValuePair<int, string> pair in removeList)
                {
                    _knownPids.Remove(pair.Key);
                    if(OnProcessInstanceDeleted!=null)OnProcessInstanceDeleted(pair.Value, (uint)pair.Key);
                }
            }
        }

		#endregion (------  Event Handlers  ------)

		#region (------  Overridden Methods  ------)

        public override event OnProcessActivityDelegate OnProcessInstanceCreated;
        public override event OnProcessActivityDelegate OnProcessInstanceDeleted;

        public override uint GetProcessInstances(string descriptor)
        {
            uint count = 0;
            foreach (KeyValuePair<int, string> pair in _knownPids)
            {
                if (pair.Value == descriptor) count++;
            }
            return count;
        }

        public override void Start()
        {
            lock (_syncRoot)
            {
                _timer.Start();
            }
        }

        public override void Stop()
        {
            lock (_syncRoot)
            {
                _timer.Stop();
            }
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Private Methods  ------)

        private void FillRunningPidsList()
        {
            lock (_syncRoot)
            {
                _runningPids.Clear();
                foreach (Process p in Process.GetProcesses("."))
                {
                    _runningPids.Add(p.Id);
                }
            }
        }

		#endregion (------  Private Methods  ------)

		#region (------  Public Methods  ------)

        public override void RemoveProcessData(string descriptor)
        {
            List<int> pidsToRemove = new List<int>();
            lock (_syncRoot)
            {
                foreach (KeyValuePair<int, string> pair in _knownPids)
                {
                    if (pair.Value == descriptor)
                    {
                        pidsToRemove.Add(pair.Key);
                    }
                }
                foreach (int pid in pidsToRemove)
                {
                    //Console.WriteLine(pid);
                    _knownPids.Remove(pid);
                }
            }
        }

		#endregion (------  Public Methods  ------)

    }
}