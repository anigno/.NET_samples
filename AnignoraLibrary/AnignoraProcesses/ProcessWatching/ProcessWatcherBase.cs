namespace AnignoraProcesses.ProcessWatching
{
    public delegate void OnProcessActivityDelegate(string descriptor, uint pid);

    public abstract class ProcessWatcherBase
    {

		#region (------  Events  ------)

        public abstract event OnProcessActivityDelegate OnProcessInstanceCreated;

        public abstract event OnProcessActivityDelegate OnProcessInstanceDeleted;

		#endregion (------  Events  ------)

		#region (------  Public Methods  ------)

        public abstract uint GetProcessInstances(string descriptor);

        public abstract void RemoveProcessData(string descriptor);

        public abstract void Start();

        public abstract void Stop();

		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)


		#endregion (------  Protected Methods  ------)


        public abstract double Interval { get; set; }


    }
}
