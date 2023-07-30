namespace JobSchedulerService.DataItems
{
    public class JobCompleted
    {
        #region Public Methods

        public override string ToString()
        {
            return string.Format("[{0} {1} IsSucceeded: {2} IsResed: {3}]", WorkerId, Request, IsSucceeded, IsResend);
        }

        #endregion

        #region Public Properties

        public JobRequest Request { get; set; }
        public string WorkerId { get; set; }
        public bool IsSucceeded { get; set; }
        public bool IsResend { get; set; }
        public object JobFinishedData { get; set; }

        #endregion
    }
}