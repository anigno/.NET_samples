using System;

namespace AnignoraIO.DirectoryWalking
{
    public class DirectoryWalkerActivityEventArgs : EventArgs
    {
        #region Constructors

        public DirectoryWalkerActivityEventArgs(string p_path)
        {
            Path = p_path;
        }

        #endregion

        #region Public Properties

        public string Path { get; set; }

        #endregion
    }
}