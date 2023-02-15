#region Assembly log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a
// D:\DEV\GIT\github-.NET\AnignoraLibrary\packages\log4net.2.0.15\lib\net45\log4net.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace LoggingProvider
{
    //
    // Summary:
    //     Appends logging events to a file.
    //
    // Remarks:
    //     Logging events are sent to the file specified by the log4net.Appender.FileAppender.File
    //     property.
    //     The file can be opened in either append or overwrite mode by specifying the log4net.Appender.FileAppender.AppendToFile
    //     property. If the file path is relative it is taken as relative from the application
    //     base directory. The file encoding can be specified by setting the log4net.Appender.FileAppender.Encoding
    //     property.
    //     The layout's log4net.Layout.ILayout.Header and log4net.Layout.ILayout.Footer
    //     values will be written each time the file is opened and closed respectively.
    //     If the log4net.Appender.FileAppender.AppendToFile property is true then the file
    //     may contain multiple copies of the header and footer.
    //     This appender will first try to open the file for writing when log4net.Appender.FileAppender.ActivateOptions
    //     is called. This will typically be during configuration. If the file cannot be
    //     opened for writing the appender will attempt to open the file again each time
    //     a message is logged to the appender. If the file cannot be opened for writing
    //     when a message is logged then the message will be discarded by this appender.
    //     The log4net.Appender.FileAppender supports pluggable file locking models via
    //     the log4net.Appender.FileAppender.LockingModel property. The default behavior,
    //     implemented by log4net.Appender.FileAppender.ExclusiveLock is to obtain an exclusive
    //     write lock on the file until this appender is closed. The alternative models
    //     only hold a write lock while the appender is writing a logging event (log4net.Appender.FileAppender.MinimalLock)
    //     or synchronize by using a named system wide Mutex (log4net.Appender.FileAppender.InterProcessLock).
    //     All locking strategies have issues and you should seriously consider using a
    //     different strategy that avoids having multiple processes logging to the same
    //     file.
    public class FileAppenderExtended : TextWriterAppender
    {
        //
        // Summary:
        //     Write only System.IO.Stream that uses the log4net.Appender.FileAppender.LockingModelBase
        //     to manage access to an underlying resource.
        private sealed class LockingStream : Stream, IDisposable
        {
            [Serializable]
            public sealed class LockStateException : LogException
            {
                public LockStateException(string message)
                    : base(message)
                {
                }

                public LockStateException()
                {
                }

                public LockStateException(string message, Exception innerException)
                    : base(message, innerException)
                {
                }

                private LockStateException(SerializationInfo info, StreamingContext context)
                    : base(info, context)
                {
                }
            }

            private Stream m_realStream;

            private LockingModelBase m_lockingModel;

            private int m_lockLevel;

            private int m_readTotal = -1;

            public override bool CanRead => false;

            public override bool CanSeek
            {
                get
                {
                    AssertLocked();
                    return m_realStream.CanSeek;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    AssertLocked();
                    return m_realStream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    AssertLocked();
                    return m_realStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    AssertLocked();
                    return m_realStream.Position;
                }
                set
                {
                    AssertLocked();
                    m_realStream.Position = value;
                }
            }

            public LockingStream(LockingModelBase locking)
            {
                if (locking == null)
                {
                    throw new ArgumentException("Locking model may not be null", "locking");
                }

                m_lockingModel = locking;
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                AssertLocked();
                IAsyncResult asyncResult = m_realStream.BeginRead(buffer, offset, count, callback, state);
                m_readTotal = EndRead(asyncResult);
                return asyncResult;
            }

            //
            // Summary:
            //     True asynchronous writes are not supported, the implementation forces a synchronous
            //     write.
            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                AssertLocked();
                IAsyncResult asyncResult = m_realStream.BeginWrite(buffer, offset, count, callback, state);
                EndWrite(asyncResult);
                return asyncResult;
            }

            public override void Close()
            {
                m_lockingModel.CloseFile();
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                AssertLocked();
                return m_readTotal;
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
            }

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                AssertLocked();
                return m_realStream.ReadAsync(buffer, offset, count, cancellationToken);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                AssertLocked();
                return base.WriteAsync(buffer, offset, count, cancellationToken);
            }

            public override void Flush()
            {
                AssertLocked();
                m_realStream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return m_realStream.Read(buffer, offset, count);
            }

            public override int ReadByte()
            {
                return m_realStream.ReadByte();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                AssertLocked();
                return m_realStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                AssertLocked();
                m_realStream.SetLength(value);
            }

            void IDisposable.Dispose()
            {
                Close();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                AssertLocked();
                m_realStream.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                AssertLocked();
                m_realStream.WriteByte(value);
            }

            private void AssertLocked()
            {
                if (m_realStream == null)
                {
                    throw new LockStateException("The file is not currently locked");
                }
            }

            public bool AcquireLock()
            {
                bool result = false;
                lock (this)
                {
                    if (m_lockLevel == 0)
                    {
                        m_realStream = m_lockingModel.AcquireLock();
                    }

                    if (m_realStream != null)
                    {
                        m_lockLevel++;
                        return true;
                    }

                    return result;
                }
            }

            public void ReleaseLock()
            {
                lock (this)
                {
                    m_lockLevel--;
                    if (m_lockLevel == 0)
                    {
                        m_lockingModel.ReleaseLock();
                        m_realStream = null;
                    }
                }
            }
        }

        //
        // Summary:
        //     Locking model base class
        //
        // Remarks:
        //     Base class for the locking models available to the log4net.Appender.FileAppender
        //     derived loggers.
        public abstract class LockingModelBase
        {
            private FileAppenderExtended m_appender;

            //
            // Summary:
            //     Gets or sets the log4net.Appender.FileAppender for this LockingModel
            //
            // Value:
            //     The log4net.Appender.FileAppender for this LockingModel
            //
            // Remarks:
            //     The file appender this locking model is attached to and working on behalf of.
            //     The file appender is used to locate the security context and the error handler
            //     to use.
            //     The value of this property will be set before log4net.Appender.FileAppender.LockingModelBase.OpenFile(System.String,System.Boolean,System.Text.Encoding)
            //     is called.
            public FileAppenderExtended CurrentAppender
            {
                get
                {
                    return m_appender;
                }
                set
                {
                    m_appender = value;
                }
            }

            //
            // Summary:
            //     Open the output file
            //
            // Parameters:
            //   filename:
            //     The filename to use
            //
            //   append:
            //     Whether to append to the file, or overwrite
            //
            //   encoding:
            //     The encoding to use
            //
            // Remarks:
            //     Open the file specified and prepare for logging. No writes will be made until
            //     log4net.Appender.FileAppender.LockingModelBase.AcquireLock is called. Must be
            //     called before any calls to log4net.Appender.FileAppender.LockingModelBase.AcquireLock,
            //     log4net.Appender.FileAppender.LockingModelBase.ReleaseLock and log4net.Appender.FileAppender.LockingModelBase.CloseFile.
            public abstract void OpenFile(string filename, bool append, Encoding encoding);

            //
            // Summary:
            //     Close the file
            //
            // Remarks:
            //     Close the file. No further writes will be made.
            public abstract void CloseFile();

            //
            // Summary:
            //     Initializes all resources used by this locking model.
            public abstract void ActivateOptions();

            //
            // Summary:
            //     Disposes all resources that were initialized by this locking model.
            public abstract void OnClose();

            //
            // Summary:
            //     Acquire the lock on the file
            //
            // Returns:
            //     A stream that is ready to be written to.
            //
            // Remarks:
            //     Acquire the lock on the file in preparation for writing to it. Return a stream
            //     pointing to the file. log4net.Appender.FileAppender.LockingModelBase.ReleaseLock
            //     must be called to release the lock on the output file.
            public abstract Stream AcquireLock();

            //
            // Summary:
            //     Release the lock on the file
            //
            // Remarks:
            //     Release the lock on the file. No further writes will be made to the stream until
            //     log4net.Appender.FileAppender.LockingModelBase.AcquireLock is called again.
            public abstract void ReleaseLock();

            //
            // Summary:
            //     Helper method that creates a FileStream under CurrentAppender's SecurityContext.
            //
            // Parameters:
            //   filename:
            //
            //   append:
            //
            //   fileShare:
            //
            // Remarks:
            //     Typically called during OpenFile or AcquireLock.
            //     If the directory portion of the filename does not exist, it is created via Directory.CreateDirecctory.
            protected Stream CreateStream(string filename, bool append, FileShare fileShare)
            {
                using (CurrentAppender.SecurityContext.Impersonate(this))
                {
                    string directoryName = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    FileMode mode = (append ? FileMode.Append : FileMode.Create);
                    return new FileStream(filename, mode, FileAccess.Write, fileShare);
                }
            }

            //
            // Summary:
            //     Helper method to close stream under CurrentAppender's SecurityContext.
            //
            // Parameters:
            //   stream:
            //
            // Remarks:
            //     Does not set stream to null.
            protected void CloseStream(Stream stream)
            {
                using (CurrentAppender.SecurityContext.Impersonate(this))
                {
                    stream.Dispose();
                }
            }
        }

        //
        // Summary:
        //     Hold an exclusive lock on the output file
        //
        // Remarks:
        //     Open the file once for writing and hold it open until log4net.Appender.FileAppender.ExclusiveLock.CloseFile
        //     is called. Maintains an exclusive lock on the file during this time.
        public class ExclusiveLock : LockingModelBase
        {
            private Stream m_stream;

            //
            // Summary:
            //     Open the file specified and prepare for logging.
            //
            // Parameters:
            //   filename:
            //     The filename to use
            //
            //   append:
            //     Whether to append to the file, or overwrite
            //
            //   encoding:
            //     The encoding to use
            //
            // Remarks:
            //     Open the file specified and prepare for logging. No writes will be made until
            //     log4net.Appender.FileAppender.ExclusiveLock.AcquireLock is called. Must be called
            //     before any calls to log4net.Appender.FileAppender.ExclusiveLock.AcquireLock,
            //     log4net.Appender.FileAppender.ExclusiveLock.ReleaseLock and log4net.Appender.FileAppender.ExclusiveLock.CloseFile.
            public override void OpenFile(string filename, bool append, Encoding encoding)
            {
                try
                {
                    m_stream = CreateStream(filename, append, FileShare.Read);
                }
                catch (Exception ex)
                {
                    base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
                }
            }

            //
            // Summary:
            //     Close the file
            //
            // Remarks:
            //     Close the file. No further writes will be made.
            public override void CloseFile()
            {
                CloseStream(m_stream);
                m_stream = null;
            }

            //
            // Summary:
            //     Acquire the lock on the file
            //
            // Returns:
            //     A stream that is ready to be written to.
            //
            // Remarks:
            //     Does nothing. The lock is already taken
            public override Stream AcquireLock()
            {
                return m_stream;
            }

            //
            // Summary:
            //     Release the lock on the file
            //
            // Remarks:
            //     Does nothing. The lock will be released when the file is closed.
            public override void ReleaseLock()
            {
            }

            //
            // Summary:
            //     Initializes all resources used by this locking model.
            public override void ActivateOptions()
            {
            }

            //
            // Summary:
            //     Disposes all resources that were initialized by this locking model.
            public override void OnClose()
            {
            }
        }

        //
        // Summary:
        //     Acquires the file lock for each write
        //
        // Remarks:
        //     Opens the file once for each log4net.Appender.FileAppender.MinimalLock.AcquireLock/log4net.Appender.FileAppender.MinimalLock.ReleaseLock
        //     cycle, thus holding the lock for the minimal amount of time. This method of locking
        //     is considerably slower than log4net.Appender.FileAppender.ExclusiveLock but allows
        //     other processes to move/delete the log file whilst logging continues.
        public class MinimalLock : LockingModelBase
        {
            private string m_filename;

            private bool m_append;

            private Stream m_stream;

            //
            // Summary:
            //     Prepares to open the file when the first message is logged.
            //
            // Parameters:
            //   filename:
            //     The filename to use
            //
            //   append:
            //     Whether to append to the file, or overwrite
            //
            //   encoding:
            //     The encoding to use
            //
            // Remarks:
            //     Open the file specified and prepare for logging. No writes will be made until
            //     log4net.Appender.FileAppender.MinimalLock.AcquireLock is called. Must be called
            //     before any calls to log4net.Appender.FileAppender.MinimalLock.AcquireLock, log4net.Appender.FileAppender.MinimalLock.ReleaseLock
            //     and log4net.Appender.FileAppender.MinimalLock.CloseFile.
            public override void OpenFile(string filename, bool append, Encoding encoding)
            {
                m_filename = filename;
                m_append = append;
            }

            //
            // Summary:
            //     Close the file
            //
            // Remarks:
            //     Close the file. No further writes will be made.
            public override void CloseFile()
            {
            }

            //
            // Summary:
            //     Acquire the lock on the file
            //
            // Returns:
            //     A stream that is ready to be written to.
            //
            // Remarks:
            //     Acquire the lock on the file in preparation for writing to it. Return a stream
            //     pointing to the file. log4net.Appender.FileAppender.MinimalLock.ReleaseLock must
            //     be called to release the lock on the output file.
            public override Stream AcquireLock()
            {
                if (m_stream == null)
                {
                    try
                    {
                        m_stream = CreateStream(m_filename, m_append, FileShare.Read);
                        m_append = true;
                    }
                    catch (Exception ex)
                    {
                        base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + m_filename + ". " + ex.Message);
                    }
                }

                return m_stream;
            }

            //
            // Summary:
            //     Release the lock on the file
            //
            // Remarks:
            //     Release the lock on the file. No further writes will be made to the stream until
            //     log4net.Appender.FileAppender.MinimalLock.AcquireLock is called again.
            public override void ReleaseLock()
            {
                CloseStream(m_stream);
                m_stream = null;
            }

            //
            // Summary:
            //     Initializes all resources used by this locking model.
            public override void ActivateOptions()
            {
            }

            //
            // Summary:
            //     Disposes all resources that were initialized by this locking model.
            public override void OnClose()
            {
            }
        }

        //
        // Summary:
        //     Provides cross-process file locking.
        public class InterProcessLock : LockingModelBase
        {
            private Mutex m_mutex;

            private Stream m_stream;

            private int m_recursiveWatch;

            //
            // Summary:
            //     Open the file specified and prepare for logging.
            //
            // Parameters:
            //   filename:
            //     The filename to use
            //
            //   append:
            //     Whether to append to the file, or overwrite
            //
            //   encoding:
            //     The encoding to use
            //
            // Remarks:
            //     Open the file specified and prepare for logging. No writes will be made until
            //     log4net.Appender.FileAppender.InterProcessLock.AcquireLock is called. Must be
            //     called before any calls to log4net.Appender.FileAppender.InterProcessLock.AcquireLock,
            //     -log4net.Appender.FileAppender.InterProcessLock.ReleaseLock and log4net.Appender.FileAppender.InterProcessLock.CloseFile.
            [SecuritySafeCritical]
            public override void OpenFile(string filename, bool append, Encoding encoding)
            {
                try
                {
                    m_stream = CreateStream(filename, append, FileShare.ReadWrite);
                }
                catch (Exception ex)
                {
                    base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
                }
            }

            //
            // Summary:
            //     Close the file
            //
            // Remarks:
            //     Close the file. No further writes will be made.
            public override void CloseFile()
            {
                try
                {
                    CloseStream(m_stream);
                    m_stream = null;
                }
                finally
                {
                    ReleaseLock();
                }
            }

            //
            // Summary:
            //     Acquire the lock on the file
            //
            // Returns:
            //     A stream that is ready to be written to.
            //
            // Remarks:
            //     Does nothing. The lock is already taken
            public override Stream AcquireLock()
            {
                if (m_mutex != null)
                {
                    m_mutex.WaitOne();
                    m_recursiveWatch++;
                    if (m_stream != null && m_stream.CanSeek)
                    {
                        m_stream.Seek(0L, SeekOrigin.End);
                    }
                }
                else
                {
                    base.CurrentAppender.ErrorHandler.Error("Programming error, no mutex available to acquire lock! From here on things will be dangerous!");
                }

                return m_stream;
            }

            //
            // Summary:
            //     Releases the lock and allows others to acquire a lock.
            public override void ReleaseLock()
            {
                if (m_mutex != null)
                {
                    if (m_recursiveWatch > 0)
                    {
                        m_recursiveWatch--;
                        m_mutex.ReleaseMutex();
                    }
                }
                else
                {
                    base.CurrentAppender.ErrorHandler.Error("Programming error, no mutex available to release the lock!");
                }
            }

            //
            // Summary:
            //     Initializes all resources used by this locking model.
            public override void ActivateOptions()
            {
                if (m_mutex == null)
                {
                    string name = base.CurrentAppender.File.Replace("\\", "_").Replace(":", "_").Replace("/", "_");
                    m_mutex = new Mutex(initiallyOwned: false, name);
                }
                else
                {
                    base.CurrentAppender.ErrorHandler.Error("Programming error, mutex already initialized!");
                }
            }

            //
            // Summary:
            //     Disposes all resources that were initialized by this locking model.
            public override void OnClose()
            {
                if (m_mutex != null)
                {
                    m_mutex.Dispose();
                    m_mutex = null;
                }
                else
                {
                    base.CurrentAppender.ErrorHandler.Error("Programming error, mutex not initialized!");
                }
            }
        }

        //
        // Summary:
        //     Flag to indicate if we should append to the file or overwrite the file. The default
        //     is to append.
        private bool m_appendToFile = true;

        //
        // Summary:
        //     The name of the log file.
        private string m_fileName;

        //
        // Summary:
        //     The encoding to use for the file stream.
        private Encoding m_encoding = Encoding.GetEncoding(0);

        //
        // Summary:
        //     The security context to use for privileged calls
        private log4net.Core.SecurityContext m_securityContext;

        //
        // Summary:
        //     The stream to log to. Has added locking semantics
        private LockingStream m_stream;

        //
        // Summary:
        //     The locking model to use
        private LockingModelBase m_lockingModel = new ExclusiveLock();

        //
        // Summary:
        //     The fully qualified type of the FileAppender class.
        //
        // Remarks:
        //     Used by the internal logger to record the Type of the log message.
        private static readonly Type declaringType = typeof(FileAppender);

        //
        // Summary:
        //     Gets or sets the path to the file that logging will be written to.
        //
        // Value:
        //     The path to the file that logging will be written to.
        //
        // Remarks:
        //     If the path is relative it is taken as relative from the application base directory.
        public virtual string File
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
            }
        }

        //
        // Summary:
        //     Gets or sets a flag that indicates whether the file should be appended to or
        //     overwritten.
        //
        // Value:
        //     Indicates whether the file should be appended to or overwritten.
        //
        // Remarks:
        //     If the value is set to false then the file will be overwritten, if it is set
        //     to true then the file will be appended to.
        //     The default value is true.
        public bool AppendToFile
        {
            get
            {
                return m_appendToFile;
            }
            set
            {
                m_appendToFile = value;
            }
        }

        //
        // Summary:
        //     Gets or sets log4net.Appender.FileAppender.Encoding used to write to the file.
        //
        // Value:
        //     The log4net.Appender.FileAppender.Encoding used to write to the file.
        //
        // Remarks:
        //     The default encoding set is System.Text.Encoding.Default which is the encoding
        //     for the system's current ANSI code page.
        public Encoding Encoding
        {
            get
            {
                return m_encoding;
            }
            set
            {
                m_encoding = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the log4net.Appender.FileAppender.SecurityContext used to write
        //     to the file.
        //
        // Value:
        //     The log4net.Appender.FileAppender.SecurityContext used to write to the file.
        //
        // Remarks:
        //     Unless a log4net.Appender.FileAppender.SecurityContext specified here for this
        //     appender the log4net.Core.SecurityContextProvider.DefaultProvider is queried
        //     for the security context to use. The default behavior is to use the security
        //     context of the current thread.
        public log4net.Core.SecurityContext SecurityContext
        {
            get
            {
                return m_securityContext;
            }
            set
            {
                m_securityContext = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the log4net.Appender.FileAppender.LockingModel used to handle locking
        //     of the file.
        //
        // Value:
        //     The log4net.Appender.FileAppender.LockingModel used to lock the file.
        //
        // Remarks:
        //     Gets or sets the log4net.Appender.FileAppender.LockingModel used to handle locking
        //     of the file.
        //     There are three built in locking models, log4net.Appender.FileAppender.ExclusiveLock,
        //     log4net.Appender.FileAppender.MinimalLock and log4net.Appender.FileAppender.InterProcessLock
        //     . The first locks the file from the start of logging to the end, the second locks
        //     only for the minimal amount of time when logging each message and the last synchronizes
        //     processes using a named system wide Mutex.
        //     The default locking model is the log4net.Appender.FileAppender.ExclusiveLock.
        public LockingModelBase LockingModel
        {
            get
            {
                return m_lockingModel;
            }
            set
            {
                m_lockingModel = value;
            }
        }

        //
        // Summary:
        //     Default constructor
        //
        // Remarks:
        //     Default constructor
        public FileAppenderExtended()
        {
        }

        //
        // Summary:
        //     Construct a new appender using the layout, file and append mode.
        //
        // Parameters:
        //   layout:
        //     the layout to use with this appender
        //
        //   filename:
        //     the full path to the file to write to
        //
        //   append:
        //     flag to indicate if the file should be appended to
        //
        // Remarks:
        //     Obsolete constructor.
        [Obsolete("Instead use the default constructor and set the Layout, File & AppendToFile properties")]
        public FileAppenderExtended(ILayout layout, string filename, bool append)
        {
            Layout = layout;
            File = filename;
            AppendToFile = append;
            ActivateOptions();
        }

        //
        // Summary:
        //     Construct a new appender using the layout and file specified. The file will be
        //     appended to.
        //
        // Parameters:
        //   layout:
        //     the layout to use with this appender
        //
        //   filename:
        //     the full path to the file to write to
        //
        // Remarks:
        //     Obsolete constructor.
        [Obsolete("Instead use the default constructor and set the Layout & File properties")]
        public FileAppenderExtended(ILayout layout, string filename)
            : this(layout, filename, append: true)
        {
        }

        //
        // Summary:
        //     Activate the options on the file appender.
        //
        // Remarks:
        //     This is part of the log4net.Core.IOptionHandler delayed object activation scheme.
        //     The log4net.Appender.FileAppender.ActivateOptions method must be called on this
        //     object after the configuration properties have been set. Until log4net.Appender.FileAppender.ActivateOptions
        //     is called this object is in an undefined state and must not be used.
        //     If any of the configuration properties are modified then log4net.Appender.FileAppender.ActivateOptions
        //     must be called again.
        //     This will cause the file to be opened.
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (m_securityContext == null)
            {
                m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }

            if (m_lockingModel == null)
            {
                m_lockingModel = new ExclusiveLock();
            }

            m_lockingModel.CurrentAppender = this;
            m_lockingModel.ActivateOptions();
            if (m_fileName != null)
            {
                using (SecurityContext.Impersonate(this))
                {
                    m_fileName = ConvertToFullPath(m_fileName.Trim());
                }

                SafeOpenFile(m_fileName, m_appendToFile);
            }
            else
            {
                LogLog.Warn(declaringType, "FileAppender: File option not set for appender [" + base.Name + "].");
                LogLog.Warn(declaringType, "FileAppender: Are you using FileAppender instead of ConsoleAppender?");
            }
        }

        //
        // Summary:
        //     Closes any previously opened file and calls the parent's log4net.Appender.TextWriterAppender.Reset.
        //
        // Remarks:
        //     Resets the filename and the file stream.
        protected override void Reset()
        {
            base.Reset();
            m_fileName = null;
        }

        //
        // Summary:
        //     Close this appender instance. The underlying stream or writer is also closed.
        protected override void OnClose()
        {
            base.OnClose();
            m_lockingModel.OnClose();
        }

        //
        // Summary:
        //     Called to initialize the file writer
        //
        // Remarks:
        //     Will be called for each logged message until the file is successfully opened.
        protected override void PrepareWriter()
        {
            SafeOpenFile(m_fileName, m_appendToFile);
        }

        //
        // Summary:
        //     This method is called by the AppenderSkeleton.DoAppend(LoggingEvent) method.
        //
        // Parameters:
        //   loggingEvent:
        //     The event to log.
        //
        // Remarks:
        //     Writes a log statement to the output stream if the output stream exists and is
        //     writable.
        //     The format of the output will depend on the appender's layout.
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (m_stream.AcquireLock())
            {
                try
                {
                    base.Append(loggingEvent);
                }
                finally
                {
                    m_stream.ReleaseLock();
                }
            }
        }

        //
        // Summary:
        //     This method is called by the AppenderSkeleton.DoAppend(LoggingEvent[]) method.
        //
        // Parameters:
        //   loggingEvents:
        //     The array of events to log.
        //
        // Remarks:
        //     Acquires the output file locks once before writing all the events to the stream.
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            if (m_stream.AcquireLock())
            {
                try
                {
                    base.Append(loggingEvents);
                }
                finally
                {
                    m_stream.ReleaseLock();
                }
            }
        }

        //
        // Summary:
        //     Writes a footer as produced by the embedded layout's log4net.Layout.ILayout.Footer
        //     property.
        //
        // Remarks:
        //     Writes a footer as produced by the embedded layout's log4net.Layout.ILayout.Footer
        //     property.
        protected override void WriteFooter()
        {
            if (m_stream != null)
            {
                m_stream.AcquireLock();
                try
                {
                    base.WriteFooter();
                }
                finally
                {
                    m_stream.ReleaseLock();
                }
            }
        }

        //
        // Summary:
        //     Writes a header produced by the embedded layout's log4net.Layout.ILayout.Header
        //     property.
        //
        // Remarks:
        //     Writes a header produced by the embedded layout's log4net.Layout.ILayout.Header
        //     property.
        protected override void WriteHeader()
        {
            if (m_stream != null && m_stream.AcquireLock())
            {
                try
                {
                    base.WriteHeader();
                }
                finally
                {
                    m_stream.ReleaseLock();
                }
            }
        }

        //
        // Summary:
        //     Closes the underlying System.IO.TextWriter.
        //
        // Remarks:
        //     Closes the underlying System.IO.TextWriter.
        protected override void CloseWriter()
        {
            if (m_stream != null)
            {
                m_stream.AcquireLock();
                try
                {
                    base.CloseWriter();
                }
                finally
                {
                    m_stream.ReleaseLock();
                }
            }
        }

        //
        // Summary:
        //     Closes the previously opened file.
        //
        // Remarks:
        //     Writes the log4net.Layout.ILayout.Footer to the file and then closes the file.
        protected void CloseFile()
        {
            WriteFooterAndCloseWriter();
        }

        //
        // Summary:
        //     Sets and opens the file where the log output will go. The specified file must
        //     be writable.
        //
        // Parameters:
        //   fileName:
        //     The path to the log file. Must be a fully qualified path.
        //
        //   append:
        //     If true will append to fileName. Otherwise will truncate fileName
        //
        // Remarks:
        //     Calls log4net.Appender.FileAppender.OpenFile(System.String,System.Boolean) but
        //     guarantees not to throw an exception. Errors are passed to the log4net.Appender.TextWriterAppender.ErrorHandler.
        protected virtual void SafeOpenFile(string fileName, bool append)
        {
            try
            {
                OpenFile(fileName, append);
            }
            catch (Exception e)
            {
                ErrorHandler.Error("OpenFile(" + fileName + "," + append + ") call failed.", e, ErrorCode.FileOpenFailure);
            }
        }

        //
        // Summary:
        //     Sets and opens the file where the log output will go. The specified file must
        //     be writable.
        //
        // Parameters:
        //   fileName:
        //     The path to the log file. Must be a fully qualified path.
        //
        //   append:
        //     If true will append to fileName. Otherwise will truncate fileName
        //
        // Remarks:
        //     If there was already an opened file, then the previous file is closed first.
        //     This method will ensure that the directory structure for the fileName specified
        //     exists.
        protected virtual void OpenFile(string fileName, bool append)
        {
            if (LogLog.IsErrorEnabled)
            {
                bool flag = false;
                using (SecurityContext.Impersonate(this))
                {
                    flag = Path.IsPathRooted(fileName);
                }

                if (!flag)
                {
                    LogLog.Error(declaringType, "INTERNAL ERROR. OpenFile(" + fileName + "): File name is not fully qualified.");
                }
            }

            lock (this)
            {
                Reset();
                LogLog.Debug(declaringType, "Opening file for writing [" + fileName + "] append [" + append + "]");
                m_fileName = fileName;
                m_appendToFile = append;
                LockingModel.CurrentAppender = this;
                LockingModel.OpenFile(fileName, append, m_encoding);
                m_stream = new LockingStream(LockingModel);
                if (m_stream != null)
                {
                    m_stream.AcquireLock();
                    try
                    {
                        SetQWForFiles(m_stream);
                    }
                    finally
                    {
                        m_stream.ReleaseLock();
                    }
                }

                WriteHeader();
            }
        }

        //
        // Summary:
        //     Sets the quiet writer used for file output
        //
        // Parameters:
        //   fileStream:
        //     the file stream that has been opened for writing
        //
        // Remarks:
        //     This implementation of SetQWForFiles(Stream) creates a System.IO.StreamWriter
        //     over the fileStream and passes it to the SetQWForFiles(TextWriter) method.
        //     This method can be overridden by sub classes that want to wrap the System.IO.Stream
        //     in some way, for example to encrypt the output data using a System.Security.Cryptography.CryptoStream.
        protected virtual void SetQWForFiles(Stream fileStream)
        {
            StreamWriter qWForFiles = new StreamWriter(fileStream, m_encoding);
            SetQWForFiles(qWForFiles);
        }

        //
        // Summary:
        //     Sets the quiet writer being used.
        //
        // Parameters:
        //   writer:
        //     the writer over the file stream that has been opened for writing
        //
        // Remarks:
        //     This method can be overridden by sub classes that want to wrap the System.IO.TextWriter
        //     in some way.
        protected virtual void SetQWForFiles(TextWriter writer)
        {
            base.QuietWriter = new QuietTextWriter(writer, ErrorHandler);
        }

        //
        // Summary:
        //     Convert a path into a fully qualified path.
        //
        // Parameters:
        //   path:
        //     The path to convert.
        //
        // Returns:
        //     The fully qualified path.
        //
        // Remarks:
        //     Converts the path specified to a fully qualified path. If the path is relative
        //     it is taken as relative from the application base directory.
        protected static string ConvertToFullPath(string path)
        {
            return SystemInfo.ConvertToFullPath(path);
        }
    }
}
#if false // Decompilation log
'12' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll'
------------------
Resolve: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Web.dll'
------------------
Resolve: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll'
------------------
Resolve: 'System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Configuration.dll'
------------------
Resolve: 'System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Could not find by name: 'System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
#endif
