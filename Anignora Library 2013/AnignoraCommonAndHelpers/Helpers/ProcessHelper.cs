using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class ProcessHelper
    {
        #region Public Methods

        public static uint GetInstances(string applicationFullPath)
        {
            Process[] processes = Process.GetProcesses();
            uint instancesCount = 0;
            foreach (Process p in processes)
            {
                if (p.Id < 10) continue;
                try
                {
                    if (p.MainModule == null) continue;
                    string path = p.MainModule.FileName;
                    if (path.ToLower() == applicationFullPath.ToLower()) instancesCount++;
                }
                catch (Win32Exception)
                {
                    //System, process
                }
            }
            return instancesCount;
        }

        public static bool IsProcessRunning(string processName)
        {
            bool isMutexCreated;
            new Mutex(false, processName, out isMutexCreated);
            return !isMutexCreated;
        }

        #endregion
    }
}