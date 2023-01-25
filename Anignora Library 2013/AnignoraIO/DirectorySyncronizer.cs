using System;
using System.ComponentModel;
using System.IO;

namespace AnignoraIO
{
    /// <summary>
    /// Syncronize directories, blocking
    /// </summary>
    public class DirectorySyncronizer
    {
        #region Public Methods

        public void Sync(string source, string dest, bool keepHistory, string sInclude, string sExclude)
        {
            if (!Directory.Exists(source))
            {
                if (OnError != null) OnError(source, "Directory not found");
                return;
            }
            //if (source.Length < 4 || dest.Length < 4)
            //{
            //    Logger.LogWarning(@"Directory not legal, must be longer then 4 chars (E.G. C:\ is not allowed");
            //    if (OnError != null) OnError("", source + " , " + dest + @" Directory not legal, must be longer then 4 chars (E.G. C:\) is not allowed");
            //    return;
            //}
            sourceDir = source;
            destDir = dest;
            keepDestHistory = keepHistory;
            //*** Sync copy
            FileSystemWalker.OnFileFound += FileSystemWalker_OnFileFoundForSyncCopy;
            FileSystemWalker.OnStart += FileSystemWalker2_OnStart;
            FileSystemWalker.OnDirectoryEnter += FileSystemWalker2_OnDirectoryEnter;
            //Run walker
            FileSystemWalker.WalkSync(sourceDir, sInclude, sExclude);
            FileSystemWalker.OnFileFound -= FileSystemWalker_OnFileFoundForSyncCopy;
            FileSystemWalker.OnStart -= FileSystemWalker2_OnStart;
            //*** Sync seek orphans
            FileSystemWalker.OnDirectoryEnter += FileSystemWalker2_OnDirectoryEnter;
            FileSystemWalker.OnFileFound += FileSystemWalker_OnFileFoundSeekOrphans;
            FileSystemWalker.OnEnd += FileSystemWalker2_OnEnd;
            FileSystemWalker.OnDirectoryExit += FileSystemWalker2_OnDirectoryExitSeek;
            //Run walker
            FileSystemWalker.WalkSync(destDir, sInclude, sExclude);
            FileSystemWalker.OnFileFound -= FileSystemWalker_OnFileFoundSeekOrphans;
            FileSystemWalker.OnEnd -= FileSystemWalker2_OnEnd;
            FileSystemWalker.OnDirectoryExit -= FileSystemWalker2_OnDirectoryExitSeek;
        }

        #endregion

        #region Events

        [Description("Occurs when found entering new directory")]
        public event DirectorySyncronizerDelegate OnDirectoryEnter;

        [Description("Occurs when found empty directory to be deleted")]
        public event DirectorySyncronizerDelegate OnEmptyDirDelete;

        [Description("Occurs when end synchronization")]
        public event DirectorySyncronizerDelegate OnEnded;

        [Description("Occurs on error")]
        public event DirectorySyncronizerErrorDelegate OnError;

        [Description("Occurs when a new file is starting to be coppied")]
        public event DirectorySyncronizerDelegate OnFileCopyStart;

        [Description("Occurs when a file is starting to be coppied to history")]
        public event DirectorySyncronizerDelegate OnFileCopyToHistory;

        [Description("Occurs when a file is going to be delleted")]
        public event DirectorySyncronizerDelegate OnFileDelete;

        [Description("Occurs when a new file is found to be coppied")]
        public event DirectorySyncronizerDelegate OnFileFoundToCopy;

        [Description("Occurs when a file is found to seek if exist in source")]
        public event DirectorySyncronizerDelegate OnFileFoundToSeekOrphan;

        [Description("Occurs when starting")]
        public event DirectorySyncronizerDelegate OnStarted;

        #endregion

        #region Private Methods

        private void DirectoryCreate(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(path, ex.Message);
            }
        }

        private void DirectoryDelete(string path)
        {
            try
            {
                Directory.Delete(path);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(path, ex.Message);
            }
        }

        private string[] DirectoryGetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(path, ex.Message);
                return new string[0];
            }
        }

        private string[] DirectoryGetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(path, ex.Message);
                return new string[0];
            }
        }

        private void FileCopy(string sourceFilePath, string destFilePath)
        {
            try
            {
                if (OnFileCopyStart != null) OnFileCopyStart(sourceFilePath);
                File.Copy(sourceFilePath, destFilePath, true);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(sourceFilePath, ex.Message);
            }
        }

        private void FileDelete(string filePath)
        {
            try
            {
                if (OnFileDelete != null) OnFileDelete(filePath);
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(filePath, ex.Message);
            }
        }

        private DateTime FileGetLastWriteTime(string filePath)
        {
            try
            {
                return File.GetLastWriteTime(filePath);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(filePath, ex.Message);
                return DateTime.MinValue;
            }
        }

        private void FileSystemWalker_OnFileFoundForSyncCopy(string path)
        {
            if (OnFileFoundToCopy != null) OnFileFoundToCopy(path);
            SyncCopy(path);
        }

        private void FileSystemWalker_OnFileFoundSeekOrphans(string path)
        {
            if (OnFileFoundToSeekOrphan != null) OnFileFoundToSeekOrphan(path);
            SyncSeekOrphan(path);
        }

        private void FileSystemWalker2_OnDirectoryEnter(string path)
        {
            if (OnDirectoryEnter != null) OnDirectoryEnter(path);
        }

        private void FileSystemWalker2_OnDirectoryExitSeek(string path)
        {
            if (DirectoryGetFiles(path).Length == 0 && DirectoryGetDirectories(path).Length == 0)
            {
                DirectoryDelete(path);
                if (OnEmptyDirDelete != null) OnEmptyDirDelete(path);
            }
        }

        private void FileSystemWalker2_OnEnd(string path)
        {
            if (OnEnded != null) OnEnded(sourceDir);
        }

        private void FileSystemWalker2_OnStart(string path)
        {
            if (OnStarted != null) OnStarted(sourceDir);
        }

        private string GetHistoryFilePath(string fileRelativePath)
        {
            string ret = null;
            try
            {
                string filePath = destDir + HISTORY_PREFIX + @"\" + DateTime.Now.ToString("[yyyy.MM.dd]") + @"\" + fileRelativePath;
                ret = Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath) + DateTime.Now.ToString(".[hh-mm-ss]") + Path.GetExtension(filePath);
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(fileRelativePath, ex.Message);
            }
            return ret;
        }

        private static string GetRelativePath(string rootPath, string fullPath)
        {
            int a = (rootPath.Length < 4) ? 0 : 1;
            return fullPath.Substring(rootPath.Length + a);
        }

        private void SyncCopy(string sourceFilePath)
        {
            try
            {
                string fileRelativePath = GetRelativePath(sourceDir, sourceFilePath);
                string destFilePath = destDir + @"\" + fileRelativePath;
                string destDirectory = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(destDirectory))
                {
                    DirectoryCreate(destDirectory);
                }
                if (File.Exists(destFilePath))
                {
                    if (FileGetLastWriteTime(destFilePath) < FileGetLastWriteTime(sourceFilePath))
                    {
                        //File exist but older
                        if (keepDestHistory)
                        {
                            string historyPath = GetHistoryFilePath(fileRelativePath);
                            string historyDir = Path.GetDirectoryName(historyPath);
                            if (!Directory.Exists(historyDir))
                            {
                                DirectoryCreate(historyDir);
                            }
                            if (OnFileCopyToHistory != null) OnFileCopyToHistory(destFilePath);
                            FileCopy(destFilePath, historyPath);
                        }
                        FileCopy(sourceFilePath, destFilePath);
                    }
                }
                else
                {
                    //File not exist
                    FileCopy(sourceFilePath, destFilePath);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(sourceFilePath, ex.Message);
            }
        }

        private void SyncSeekOrphan(string destFilePath)
        {
            try
            {
                string fileRelativePath = GetRelativePath(destDir, destFilePath);
                string sourceFilePath = sourceDir + @"\" + fileRelativePath;
                if (!File.Exists(sourceFilePath))
                {
                    if (keepDestHistory)
                    {
                        string historyPath = GetHistoryFilePath(fileRelativePath);
                        string historyDir = Path.GetDirectoryName(historyPath);
                        if (!Directory.Exists(historyDir))
                        {
                            DirectoryCreate(historyDir);
                        }
                        if (OnFileCopyToHistory != null) OnFileCopyToHistory(destFilePath);
                        FileCopy(destFilePath, historyPath);
                    }
                    FileDelete(destFilePath);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(destFilePath, ex.Message);
            }
        }

        #endregion

        #region Fields

        private string destDir;
        private bool keepDestHistory;
        private string sourceDir;

        #endregion

        #region Constants

        public const string HISTORY_PREFIX = "___HISTORY";

        #endregion

        public delegate void DirectorySyncronizerDelegate(string path);

        public delegate void DirectorySyncronizerErrorDelegate(string path, string text);
    }
}