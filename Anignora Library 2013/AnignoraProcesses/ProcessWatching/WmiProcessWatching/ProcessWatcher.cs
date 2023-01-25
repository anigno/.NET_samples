namespace AnignoraProcesses.ProcessWatching.WmiProcessWatching
{
    public delegate void OnProcessActionDelegate(string processFullPath,uint processId);

    public class ProcessWatcher
    {

        #region Fields (2) 


        private readonly ProcessCreationWatcher _processCreationWatcher = new ProcessCreationWatcher();
        private readonly ProcessDeletionWatcher _processDeletionWatcher = new ProcessDeletionWatcher();

        #endregion Fields 

        #region Constructors (1) 

        public ProcessWatcher()
        {
            _processCreationWatcher.ProcessCreated += _processCreationWatcher_ProcessCreated;
            _processDeletionWatcher.ProcessDeleted += _processDeletionWatcher_ProcessDeleted;
        }

        #endregion Constructors 

        #region Events (2) 

        public event OnProcessActionDelegate OnProcessCreated;

        public event OnProcessActionDelegate OnProcessDeleted;

        #endregion Events 

        #region Event Handlers (2) 

        void _processCreationWatcher_ProcessCreated(Win32_Process proc)
        {
            if (OnProcessCreated != null) OnProcessCreated(proc.ExecutablePath.ToLower(), proc.ProcessId);
        }

        void _processDeletionWatcher_ProcessDeleted(Win32_Process proc)
        {
            if (OnProcessDeleted != null) OnProcessDeleted(proc.ExecutablePath.ToLower(), proc.ProcessId);
        }

        #endregion Event Handlers 

        #region Public Methods (2) 

        public void Start()
        {
            _processCreationWatcher.Start();
            _processDeletionWatcher.Start();
        }

        public void Stop()
        {
            _processCreationWatcher.Stop();
            _processDeletionWatcher.Stop();
        }

        #endregion Public Methods 

    }
}