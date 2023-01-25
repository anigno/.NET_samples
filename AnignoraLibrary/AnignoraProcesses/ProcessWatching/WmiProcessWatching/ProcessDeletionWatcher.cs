using System.Management;

namespace AnignoraProcesses.ProcessWatching.WmiProcessWatching
{
    public class ProcessDeletionWatcher :ProcessBaseWatcher
    {
        public event ProcessEventHandler ProcessDeleted;

        private const string ANY_PROCESS_DELETION_QUERY_STRING = @"SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

        public ProcessDeletionWatcher()
        {
            Query.QueryString = ANY_PROCESS_DELETION_QUERY_STRING;
            EventArrived += ProcessDeletionWatcher_EventArrivedDeleted;
        }

        void ProcessDeletionWatcher_EventArrivedDeleted(object sender, EventArrivedEventArgs e)
        {
            Win32_Process proc = new Win32_Process(e.NewEvent["TargetInstance"] as ManagementBaseObject);
            if (ProcessDeleted != null) ProcessDeleted(proc);
        }
    }
}