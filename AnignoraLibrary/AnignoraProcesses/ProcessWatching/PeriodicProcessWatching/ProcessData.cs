using System.ComponentModel;

namespace AnignoraProcesses.ProcessWatching.PeriodicProcessWatching
{
    public class ProcessData
    {

        #region Properties (10) 
        [ReadOnly(true)]
        public bool Allowed { get; set; }

        [ReadOnly(true)]
        public int Instances { get; set; }

        [ReadOnly(true)]
        public string Caption { get; set; }

        [ReadOnly(true)]
        public string CommandLine { get; set; }

        [ReadOnly(true)]
        public string CreationDateString { get; set; }

        [ReadOnly(true)]
        public string Description { get; set; }

        [ReadOnly(true)]
        public string ExecutablePath { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }


        [ReadOnly(true)]
        public uint ParentProcessId { get; set; }

        [ReadOnly(true)]
        public uint ProcessId { get; set; }

        #endregion Properties 

    }
}