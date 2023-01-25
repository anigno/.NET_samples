namespace AnignoraProcesses.BThreadPool
{
    public abstract class ActionltemBase
    {
        protected ActionltemBase(string p_descriptor, int p_retries, int p_timeoutMs)
        {
            Descriptor = p_descriptor;
            Retries = p_retries;
            TimeoutMs = p_timeoutMs;
        }

        public string Descriptor { get; private set; }
        public int Retries { get; set; }
        public int TimeoutMs { get; set; }
        public abstract void Abort();
        public abstract bool Run();
    }
}