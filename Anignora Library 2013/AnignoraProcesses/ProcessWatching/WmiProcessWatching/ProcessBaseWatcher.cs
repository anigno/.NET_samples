using System.Management;

namespace AnignoraProcesses.ProcessWatching.WmiProcessWatching
{
    public delegate void ProcessEventHandler(Win32_Process proc);

    public class ProcessBaseWatcher : ManagementEventWatcher
    {
        public ProcessBaseWatcher()
        {
            Query.QueryLanguage = "WQL";
        }
    }
}