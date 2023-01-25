using System;
using System.IO;
using System.Threading;
using AnignoraCommonAndHelpers.Helpers;

namespace AnignoraIO.FileCopyNoneBlocking
{
    /// <summary>
    /// Copy file without blocking caller, else if caller tries to call copy while already copping. block caller
    /// untill end of previous copy process.
    /// </summary>
    public class FileCopyNoneBlockingManager
    {
        #region Constructors

        public FileCopyNoneBlockingManager(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        #endregion

        #region Public Methods

        public void Abort()
        {
            lock (_syncRoot)
            {
                _isAborted = true;
                RaiseEvent(OnFileCopyAborted, _currentSourceFile, _currentDestinationFile, FileCopyOperationResultEnum.Failed, "Aborted");
            }
        }

        public void Copy(string source, string dest)
        {
            AR_Copy.WaitOne();
            AR_Copy.Reset();

            _isAborted = false;
            _copyThread = new Thread(CopyThreadStart);
            _copyThread.Name = "CopyThread " + DateTime.Now.Ticks;
            _currentSourceFile = source;
            _currentDestinationFile = dest;
            _copyThread.Start(new[] {source, dest});
        }

        #endregion

        #region Public Properties

        public long CurrentTotalBytes
        {
            get
            {
                lock (_syncRoot)
                {
                    return _currentTotalBytes;
                }
            }
            private set
            {
                lock (_syncRoot)
                {
                    _currentTotalBytes = value;
                }
            }
        }

        public long CurrentCopiedBytes
        {
            get
            {
                lock (_syncRoot)
                {
                    return _currentCopiedBytes;
                }
            }
            private set
            {
                lock (_syncRoot)
                {
                    _currentCopiedBytes = value;
                }
            }
        }

        #endregion

        #region Events

        public event FileCopyNoneBlockingEventHandler OnFileCopyAborted;

        public event FileCopyNoneBlockingEventHandler OnFileCopyEnded;

        public event FileCopyNoneBlockingEventHandler OnFileCopyError;

        public event FileCopyNoneBlockingEventHandler OnFileCopyInProgress;

        public event FileCopyNoneBlockingEventHandler OnFileCopyStarted;

        #endregion

        #region Private Methods

        private void CopyThreadStart(object o)
        {
            FileInfo fi;
            FileStream fsSource = null;
            FileStream fsDest = null;
            string source = null;
            string dest = null;
            try
            {
                string[] stringArray = (string[]) o;
                source = stringArray[0];
                dest = stringArray[1];

                //Create directory if no exists
                string destDirectory = FilesAndFoldersHelper.GetDirectoryNameEx(dest);
                if (!Directory.Exists(destDirectory))
                {
                    Directory.CreateDirectory(destDirectory);
                }
                fi = new FileInfo(source);
                fsSource = new FileStream(source, FileMode.Open);
                fsDest = new FileStream(dest, FileMode.Create);
                RaiseEvent(OnFileCopyStarted, source, dest, FileCopyOperationResultEnum.Succedded, "Bytes to copy: " + CurrentTotalBytes);
                byte[] buffer = new byte[_bufferSize];
                CurrentCopiedBytes = 0;
                CurrentTotalBytes = fi.Length;
                while (CurrentCopiedBytes < CurrentTotalBytes && _isAborted == false)
                {
                    long readSize = buffer.Length;
                    if (fsSource.Position + readSize > CurrentTotalBytes) readSize = CurrentTotalBytes - fsSource.Position;
                    fsSource.Read(buffer, 0, (int) readSize);
                    fsDest.Write(buffer, 0, (int) readSize);
                    CurrentCopiedBytes += readSize;
                    RaiseEvent(OnFileCopyInProgress, source, dest, FileCopyOperationResultEnum.Succedded, "Bytes copied: " + CurrentCopiedBytes);
                }
                fsSource.Close();
                fsDest.Close();
                if (_isAborted)
                {
                    RaiseEvent(OnFileCopyEnded, source, dest, FileCopyOperationResultEnum.Failed, "Aborted");
                    if (File.Exists(_currentDestinationFile))
                    {
                        File.Delete(_currentDestinationFile);
                    }
                }
                _isAborted = false;
                File.SetLastWriteTime(dest, File.GetLastWriteTime(source));
                File.SetCreationTime(dest, File.GetCreationTime(source));
                RaiseEvent(OnFileCopyEnded, source, dest, FileCopyOperationResultEnum.Succedded, "Bytes copied: " + CurrentCopiedBytes);
            }
            catch (Exception ex)
            {
                if (fsSource != null) fsSource.Close();
                if (fsDest != null) fsDest.Close();
                RaiseEvent(OnFileCopyError, source, dest, FileCopyOperationResultEnum.Failed, ex.Message);
            }
            finally
            {
                AR_Copy.Set();
            }
        }

        private void RaiseEvent(FileCopyNoneBlockingEventHandler fileCopyEventHandler, string source, string destination, FileCopyOperationResultEnum result, string message)
        {
            if (fileCopyEventHandler != null) fileCopyEventHandler(source, destination, result, message);
        }

        #endregion

        #region Fields

        private readonly AutoResetEvent AR_Copy = new AutoResetEvent(true);
        private bool _isAborted;
        private readonly int _bufferSize;
        private long _currentTotalBytes;
        private long _currentCopiedBytes;
        private readonly object _syncRoot = new object();
        private string _currentSourceFile;
        private string _currentDestinationFile;
        private Thread _copyThread;

        #endregion

        public enum FileCopyOperationResultEnum
        {
            Succedded,
            Failed,
        }

        public delegate void FileCopyNoneBlockingEventHandler(string sourceFile, string destinationFile, FileCopyOperationResultEnum result, string message);
    }
}