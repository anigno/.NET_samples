using System;

namespace AnignoraIO.AsyncFileAndFolderCopy
{
    public class CopyFileEventArgs : EventArgs
    {
        #region Constructors

        public CopyFileEventArgs(string p_sourceFilePath, string p_destFilePath, long p_totalSize, long p_transferedSize)
        {
            SourceFilePath = p_sourceFilePath;
            DestFilePath = p_destFilePath;
            TotalSize = p_totalSize;
            TransferedSize = p_transferedSize;
        }

        #endregion

        #region Public Properties

        public string SourceFilePath { get; private set; }
        public string DestFilePath { get; private set; }
        public long TotalSize { get; private set; }
        public long TransferedSize { get; private set; }

        #endregion
    }
}