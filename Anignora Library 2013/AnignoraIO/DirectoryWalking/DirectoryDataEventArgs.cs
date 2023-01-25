using System;

namespace AnignoraIO.DirectoryWalking
{
    public class DirectoryDataEventArgs : EventArgs
    {
        #region Constructors

        public DirectoryDataEventArgs(string p_path, long p_size, int p_filesCount)
        {
            Path = p_path;
            Size = p_size;
            FilesCount = p_filesCount;
        }

        #endregion

        #region Public Properties

        public string Path { get; set; }
        public long Size { get; set; }
        public int FilesCount { get; set; }

        #endregion
    }
}