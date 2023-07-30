#region

using System.Threading;

#endregion

namespace JobSchedulerService.DataItems
{
    public class JobRequest
    {
        #region Constructors

        public JobRequest(string p_requestingClientId, int p_retrieses,object p_jobData)
        {
            RequestingClientId = p_requestingClientId;
            RequestNumber = Interlocked.Increment(ref s_uniqueId);
            RetriesLeft = p_retrieses;
            JobData = p_jobData;
        }

        public JobRequest()
        {
            //Service usage ctor
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("[RequestNumber: {0} {1}]", RequestNumber, RequestingClientId);
        }

        #endregion

        #region Public Properties

        public string RequestingClientId { get; set; }

        public int RequestNumber { get; set; }

        public int RetriesLeft { get; set; }

        public object JobData { get; set; }

        #endregion

        #region Fields

        private static int s_uniqueId = 1000;

        #endregion
    }
}