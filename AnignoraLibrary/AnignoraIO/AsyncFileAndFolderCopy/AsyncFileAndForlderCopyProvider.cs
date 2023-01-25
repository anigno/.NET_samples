using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace AnignoraIO.AsyncFileAndFolderCopy
{
    public class AsyncFileAndForlderCopyProvider
    {
        #region Public Methods

        public void Abort()
        {
            Interlocked.Exchange(ref m_isAborted, 1);
        }

        public void CopyDirectoryAsync(string p_sourceDirectory, string p_destDirectory)
        {
            Interlocked.Exchange(ref m_isAborted, 0);
            m_directoryCopyThread = new Thread(directoryCopyThreadStart);
            m_directoryCopyThread.IsBackground = true;
            m_directoryCopyThread.Start(new object[] {p_sourceDirectory, p_destDirectory});
        }

        public void CopyFileAsync(string p_sourceFilePath, string p_destFilePath)
        {
            SourceFilePath = p_sourceFilePath;
            DestFilePath = p_destFilePath;
            string destDirectory = Path.GetDirectoryName(p_destFilePath);
            if (destDirectory == null)
                throw new
                    ArgumentException("AsyncFilesCopyProvider, destDirectory should not be null", "p_destFilePath");
            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);
            bool bResult = CopyFileEx(p_sourceFilePath, p_destFilePath, CopyProgressHandler, IntPtr.Zero, ref m_pbCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);
            if (bResult) return;
            SourceFilePath = null;
            DestFilePath = null;
            throw new Win32Exception(string.Format("AsyncFilesCopyProvider, {0} SourceFilePath=[{1}] DestFilePath=[{2}]", Marshal.GetLastWin32Error(), p_sourceFilePath, p_destFilePath));
        }

        #endregion

        #region Public Properties

        public string DestFilePath { get; private set; }

        public string SourceFilePath { get; private set; }

        #endregion

        #region Events

        public event EventHandler<CopyDirectoryEventArgs> CopyDirectoryEnded = delegate { };

        public event EventHandler<CopyDirectoryEventArgs> CopyDirectoryError = delegate { };

        public event EventHandler<CopyDirectoryEventArgs> CopyDirectoryStarted = delegate { };

        public event EventHandler<CopyFileEventArgs> CopyFileCompleted = delegate { };

        public event EventHandler<CopyFileEventArgs> CopyFileProgressUpdated = delegate { };

        #endregion

        #region Private Methods

        private void copyDirectoryAsyncRecurse(string p_sourceDirectory, string p_destDirectory)
        {
            try
            {
                CopyDirectoryStarted(this, new CopyDirectoryEventArgs(p_sourceDirectory, p_destDirectory));
                DirectoryInfo directorylnfo = new DirectoryInfo(p_sourceDirectory);
                foreach (FileInfo filelnfo in directorylnfo.GetFiles())
                {
                    //Copy only if not aborted
                    if (Interlocked.Read(ref m_isAborted) == 0)
                    {
                        CopyFileAsync(filelnfo.FullName, Path.Combine(p_destDirectory, filelnfo.Name));
                        m_fileCopySyncEvent.WaitOne();
                    }
                }
                foreach (DirectoryInfo subDirectorylnfo in directorylnfo.GetDirectories())
                {
                    //Calling recurse only if not aborted
                    if (Interlocked.Read(ref m_isAborted) == 0)
                    {
                        copyDirectoryAsyncRecurse(subDirectorylnfo.FullName,
                            Path.Combine(p_destDirectory, subDirectorylnfo.Name));
                    }
                }
                CopyDirectoryEnded(this, new CopyDirectoryEventArgs(p_sourceDirectory, p_destDirectory));
            }
            catch (Exception)
            {
                CopyDirectoryError(this, new CopyDirectoryEventArgs(p_sourceDirectory, p_destDirectory));
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string p_lpExistingFileName, string p_lpNewFileName, CopyProgressRoutine p_lpProgressRoutine, IntPtr p_lpData, ref Int32 p_pbCancel, CopyFileFlags p_dwCopyFlags);

        private CopyProgressResult CopyProgressHandler(long p_total, long p_transferred, long p_streamSize, long p_streamByteTrans, uint p_dwStreamNumber,
            CopyProgressCallbackReason p_reason, IntPtr p_hSourceFile, IntPtr p_hDestinationFile, IntPtr p_lpData)
        {
            CopyFileProgressUpdated(this, new CopyFileEventArgs(SourceFilePath, DestFilePath, p_total, p_transferred));
            if (p_total == p_transferred)
            {
                CopyFileCompleted(this, new CopyFileEventArgs(SourceFilePath, DestFilePath, p_total, p_transferred));
                SourceFilePath = null;
                DestFilePath = null;
                m_fileCopySyncEvent.Set();
            }
            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        private void directoryCopyThreadStart(object p_obj)
        {
            object[] objArray = (object[]) p_obj;
            string sourceDirectory = (string) objArray[0];
            string destDirectory = (string) objArray[1];
            copyDirectoryAsyncRecurse(sourceDirectory, destDirectory);
        }

        #endregion

        #region Fields

        private Thread m_directoryCopyThread;
        private readonly ManualResetEvent m_fileCopySyncEvent = new ManualResetEvent(false);
        private long m_isAborted;
        private int m_pbCancel;

        #endregion

        private delegate CopyProgressResult CopyProgressRoutine(
            long p_totalFileSize,
            long p_totalBytesTransferred,
            long p_streamSize,
            long p_streamBytesTransferred,
            uint p_dwStreamNumber,
            CopyProgressCallbackReason p_dwCallbackReason,
            IntPtr p_hSourceFile,
            IntPtr p_hDestinationFile,
            IntPtr p_lpData);
    }
}