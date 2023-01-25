using System;

namespace AnignoraIO.AsyncFileAndFolderCopy
{
    public class CopyDirectoryEventArgs : EventArgs
    {
        #region Constructors

        public CopyDirectoryEventArgs(string p_sourcePath, string p_destPath)
        {
            SourcePath = p_sourcePath;
            DestPath = p_destPath;
        }

        #endregion

        #region Public Properties

        public string SourcePath { get; private set; }
        public string DestPath { get; private set; }

        #endregion
    }
}