using System;
using System.IO;

namespace AnignoraIO
{
    public static class FileAndFolderlnfoExtensions
    {
        #region Public Methods

        public static FileInfo[] GetFilesSafe(this DirectoryInfo p_directoryInfo, string p_searchPattern = "*", SearchOption p_searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                FileInfo[] files = p_directoryInfo.GetFiles(p_searchPattern, p_searchOption);
                return files;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return new FileInfo[0];
            }
        }

        public static DirectoryInfo[] GetDirectoriesSafe(this DirectoryInfo p_directoryInfo, string p_searchPattern = "*", SearchOption p_searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo[] directories = p_directoryInfo.GetDirectories(p_searchPattern, p_searchOption);
                return directories;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return new DirectoryInfo[0];
            }
        }

        #endregion

        #region Public Properties

        public static Exception LastError { get; private set; }

        #endregion
    }
}