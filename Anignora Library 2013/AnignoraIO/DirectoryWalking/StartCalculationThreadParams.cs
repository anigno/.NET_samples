namespace AnignoraIO.DirectoryWalking
{
    public class StartCalculationThreadParams
    {
        #region Constructors

        public StartCalculationThreadParams(string p_rootDirectory, WalkingStyleEnum p_walkingStyle)
        {
            RootDirectory = p_rootDirectory;
            WalkingStyle = p_walkingStyle;
        }

        #endregion

        #region Public Properties

        public string RootDirectory { get; set; }
        public WalkingStyleEnum WalkingStyle { get; set; }

        #endregion
    }
}