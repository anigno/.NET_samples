using System.Management;

namespace AnignoraProcesses.ProcessWatching.WmiProcessWatching
{
    public class ProcessCreationWatcher : ProcessBaseWatcher
    {
        public event ProcessEventHandler ProcessCreated;

        private const string ANY_PROCESS_CREATION_QUERY_STRING = @"SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

        public ProcessCreationWatcher()
        {
            Query.QueryString = ANY_PROCESS_CREATION_QUERY_STRING;
            EventArrived += ProcessCreationWatcher_EventArrivedCreated;
        }

        void ProcessCreationWatcher_EventArrivedCreated(object sender, EventArrivedEventArgs e)
        {
            Win32_Process proc = new Win32_Process(e.NewEvent["TargetInstance"] as ManagementBaseObject);
            if(ProcessCreated!=null)ProcessCreated(proc);
        }
    }
}