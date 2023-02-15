#region Assembly log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a
// D:\DEV\GIT\github-.NET\AnignoraLibrary\packages\log4net.2.0.15\lib\net45\log4net.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using log4net.Core;
using log4net.Util;
using LoggingProvider;

namespace LoggingInitiator
{
    //
    // Summary:
    //     Appender that rolls log files based on size or date or both.
    //
    // Remarks:
    //     RollingFileAppender can roll log files based on size or date or both depending
    //     on the setting of the log4net.Appender.RollingFileAppender.RollingStyle property.
    //     When set to log4net.Appender.RollingFileAppender.RollingMode.Size the log file
    //     will be rolled once its size exceeds the log4net.Appender.RollingFileAppender.MaximumFileSize.
    //     When set to log4net.Appender.RollingFileAppender.RollingMode.Date the log file
    //     will be rolled once the date boundary specified in the log4net.Appender.RollingFileAppender.DatePattern
    //     property is crossed. When set to log4net.Appender.RollingFileAppender.RollingMode.Composite
    //     the log file will be rolled once the date boundary specified in the log4net.Appender.RollingFileAppender.DatePattern
    //     property is crossed, but within a date boundary the file will also be rolled
    //     once its size exceeds the log4net.Appender.RollingFileAppender.MaximumFileSize.
    //     When set to log4net.Appender.RollingFileAppender.RollingMode.Once the log file
    //     will be rolled when the appender is configured. This effectively means that the
    //     log file can be rolled once per program execution.
    //     A of few additional optional features have been added:
    //     • Attach date pattern for current log file log4net.Appender.RollingFileAppender.StaticLogFileName
    //     • Backup number increments for newer files log4net.Appender.RollingFileAppender.CountDirection
    //     • Infinite number of backups by file size log4net.Appender.RollingFileAppender.MaxSizeRollBackups
    //     For large or infinite numbers of backup files a log4net.Appender.RollingFileAppender.CountDirection
    //     greater than zero is highly recommended, otherwise all the backup files need
    //     to be renamed each time a new backup is created.
    //     When Date/Time based rolling is used setting log4net.Appender.RollingFileAppender.StaticLogFileName
    //     to true will reduce the number of file renamings to few or none.
    //     Changing log4net.Appender.RollingFileAppender.StaticLogFileName or log4net.Appender.RollingFileAppender.CountDirection
    //     without clearing the log file directory of backup files will cause unexpected
    //     and unwanted side effects.
    //     If Date/Time based rolling is enabled this appender will attempt to roll existing
    //     files in the directory without a Date/Time tag based on the last write date of
    //     the base log file. The appender only rolls the log file when a message is logged.
    //     If Date/Time based rolling is enabled then the appender will not roll the log
    //     file at the Date/Time boundary but at the point when the next message is logged
    //     after the boundary has been crossed.
    //     The log4net.Appender.RollingFileAppender extends the log4net.Appender.FileAppender
    //     and has the same behavior when opening the log file. The appender will first
    //     try to open the file for writing when log4net.Appender.RollingFileAppender.ActivateOptions
    //     is called. This will typically be during configuration. If the file cannot be
    //     opened for writing the appender will attempt to open the file again each time
    //     a message is logged to the appender. If the file cannot be opened for writing
    //     when a message is logged then the message will be discarded by this appender.
    //     When rolling a backup file necessitates deleting an older backup file the file
    //     to be deleted is moved to a temporary name before being deleted.
    //     A maximum number of backup files when rolling on date/time boundaries is not
    //     supported.
    public class RollingFileAppenderStartWithOne : FileAppenderExtended
    {
        //
        // Summary:
        //     Style of rolling to use
        //
        // Remarks:
        //     Style of rolling to use
        public enum RollingMode
        {
            //
            // Summary:
            //     Roll files once per program execution
            //
            // Remarks:
            //     Roll files once per program execution. Well really once each time this appender
            //     is configured.
            //     Setting this option also sets AppendToFile to false on the RollingFileAppender,
            //     otherwise this appender would just be a normal file appender.
            Once,
            //
            // Summary:
            //     Roll files based only on the size of the file
            Size,
            //
            // Summary:
            //     Roll files based only on the date
            Date,
            //
            // Summary:
            //     Roll files based on both the size and date of the file
            Composite
        }

        //
        // Summary:
        //     The code assumes that the following 'time' constants are in a increasing sequence.
        //
        // Remarks:
        //     The code assumes that the following 'time' constants are in a increasing sequence.
        protected enum RollPoint
        {
            //
            // Summary:
            //     Roll the log not based on the date
            InvalidRollPoint = -1,
            //
            // Summary:
            //     Roll the log for each minute
            TopOfMinute,
            //
            // Summary:
            //     Roll the log for each hour
            TopOfHour,
            //
            // Summary:
            //     Roll the log twice a day (midday and midnight)
            HalfDay,
            //
            // Summary:
            //     Roll the log each day (midnight)
            TopOfDay,
            //
            // Summary:
            //     Roll the log each week
            TopOfWeek,
            //
            // Summary:
            //     Roll the log each month
            TopOfMonth
        }

        //
        // Summary:
        //     This interface is used to supply Date/Time information to the log4net.Appender.RollingFileAppender.
        //
        // Remarks:
        //     This interface is used to supply Date/Time information to the log4net.Appender.RollingFileAppender.
        //     Used primarily to allow test classes to plug themselves in so they can supply
        //     test date/times.
        public interface IDateTime
        {
            //
            // Summary:
            //     Gets the current time.
            //
            // Value:
            //     The current time.
            //
            // Remarks:
            //     Gets the current time.
            DateTime Now { get; }
        }

        //
        // Summary:
        //     Default implementation of log4net.Appender.RollingFileAppender.IDateTime that
        //     returns the current time.
        private class LocalDateTime : IDateTime
        {
            //
            // Summary:
            //     Gets the current time.
            //
            // Value:
            //     The current time.
            //
            // Remarks:
            //     Gets the current time.
            public DateTime Now => DateTime.Now;
        }

        //
        // Summary:
        //     Implementation of log4net.Appender.RollingFileAppender.IDateTime that returns
        //     the current time as the coordinated universal time (UTC).
        private class UniversalDateTime : IDateTime
        {
            //
            // Summary:
            //     Gets the current time.
            //
            // Value:
            //     The current time.
            //
            // Remarks:
            //     Gets the current time.
            public DateTime Now => DateTime.UtcNow;
        }

        //
        // Summary:
        //     The fully qualified type of the RollingFileAppender class.
        //
        // Remarks:
        //     Used by the internal logger to record the Type of the log message.
        private static readonly Type declaringType = typeof(RollingFileAppenderStartWithOne);

        //
        // Summary:
        //     This object supplies the current date/time. Allows test code to plug in a method
        //     to control this class when testing date/time based rolling. The default implementation
        //     uses the underlying value of DateTime.Now.
        private IDateTime m_dateTime;

        //
        // Summary:
        //     The date pattern. By default, the pattern is set to ".yyyy-MM-dd" meaning daily
        //     rollover.
        private string m_datePattern = ".yyyy-MM-dd";

        //
        // Summary:
        //     The actual formatted filename that is currently being written to or will be the
        //     file transferred to on roll over (based on staticLogFileName).
        private string m_scheduledFilename;

        //
        // Summary:
        //     The timestamp when we shall next recompute the filename.
        private DateTime m_nextCheck = DateTime.MaxValue;

        //
        // Summary:
        //     Holds date of last roll over
        private DateTime m_now;

        //
        // Summary:
        //     The type of rolling done
        private RollPoint m_rollPoint;

        //
        // Summary:
        //     The default maximum file size is 10MB
        private long m_maxFileSize = 10485760L;

        //
        // Summary:
        //     There is zero backup files by default
        private int m_maxSizeRollBackups;

        //
        // Summary:
        //     How many sized based backups have been made so far
        private int m_curSizeRollBackups;

        //
        // Summary:
        //     The rolling file count direction.
        private int m_countDirection = -1;

        //
        // Summary:
        //     The rolling mode used in this appender.
        private RollingMode m_rollingStyle = RollingMode.Composite;

        //
        // Summary:
        //     Cache flag set if we are rolling by date.
        private bool m_rollDate = true;

        //
        // Summary:
        //     Cache flag set if we are rolling by size.
        private bool m_rollSize = true;

        //
        // Summary:
        //     Value indicating whether to always log to the same file.
        private bool m_staticLogFileName = true;

        //
        // Summary:
        //     Value indicating whether to preserve the file name extension when rolling.
        private bool m_preserveLogFileNameExtension;

        //
        // Summary:
        //     FileName provided in configuration. Used for rolling properly
        private string m_baseFileName;

        //
        // Summary:
        //     A mutex that is used to lock rolling of files.
        private Mutex m_mutexForRolling;

        //
        // Summary:
        //     The 1st of January 1970 in UTC
        private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

        //
        // Summary:
        //     Gets or sets the strategy for determining the current date and time. The default
        //     implementation is to use LocalDateTime which internally calls through to DateTime.Now.
        //     DateTime.UtcNow may be used on frameworks newer than .NET 1.0 by specifying log4net.Appender.RollingFileAppender.UniversalDateTime.
        //
        // Value:
        //     An implementation of the log4net.Appender.RollingFileAppender.IDateTime interface
        //     which returns the current date and time.
        //
        // Remarks:
        //     Gets or sets the log4net.Appender.RollingFileAppender.IDateTime used to return
        //     the current date and time.
        //     There are two built strategies for determining the current date and time, log4net.Appender.RollingFileAppender.LocalDateTime
        //     and log4net.Appender.RollingFileAppender.UniversalDateTime.
        //     The default strategy is log4net.Appender.RollingFileAppender.LocalDateTime.
        public IDateTime DateTimeStrategy
        {
            get
            {
                return m_dateTime;
            }
            set
            {
                m_dateTime = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the date pattern to be used for generating file names when rolling
        //     over on date.
        //
        // Value:
        //     The date pattern to be used for generating file names when rolling over on date.
        //
        // Remarks:
        //     Takes a string in the same format as expected by log4net.DateFormatter.SimpleDateFormatter.
        //     This property determines the rollover schedule when rolling over on date.
        public string DatePattern
        {
            get
            {
                return m_datePattern;
            }
            set
            {
                m_datePattern = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the maximum number of backup files that are kept before the oldest
        //     is erased.
        //
        // Value:
        //     The maximum number of backup files that are kept before the oldest is erased.
        //
        // Remarks:
        //     If set to zero, then there will be no backup files and the log file will be truncated
        //     when it reaches log4net.Appender.RollingFileAppender.MaxFileSize.
        //     If a negative number is supplied then no deletions will be made. Note that this
        //     could result in very slow performance as a large number of files are rolled over
        //     unless log4net.Appender.RollingFileAppender.CountDirection is used.
        //     The maximum applies to each time based group of files and not the total.
        public int MaxSizeRollBackups
        {
            get
            {
                return m_maxSizeRollBackups;
            }
            set
            {
                m_maxSizeRollBackups = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the maximum size that the output file is allowed to reach before
        //     being rolled over to backup files.
        //
        // Value:
        //     The maximum size in bytes that the output file is allowed to reach before being
        //     rolled over to backup files.
        //
        // Remarks:
        //     This property is equivalent to log4net.Appender.RollingFileAppender.MaximumFileSize
        //     except that it is required for differentiating the setter taking a System.Int64
        //     argument from the setter taking a System.String argument.
        //     The default maximum file size is 10MB (10*1024*1024).
        public long MaxFileSize
        {
            get
            {
                return m_maxFileSize;
            }
            set
            {
                m_maxFileSize = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the maximum size that the output file is allowed to reach before
        //     being rolled over to backup files.
        //
        // Value:
        //     The maximum size that the output file is allowed to reach before being rolled
        //     over to backup files.
        //
        // Remarks:
        //     This property allows you to specify the maximum size with the suffixes "KB",
        //     "MB" or "GB" so that the size is interpreted being expressed respectively in
        //     kilobytes, megabytes or gigabytes.
        //     For example, the value "10KB" will be interpreted as 10240 bytes.
        //     The default maximum file size is 10MB.
        //     If you have the option to set the maximum file size programmatically consider
        //     using the log4net.Appender.RollingFileAppender.MaxFileSize property instead as
        //     this allows you to set the size in bytes as a System.Int64.
        public string MaximumFileSize
        {
            get
            {
                return m_maxFileSize.ToString(NumberFormatInfo.InvariantInfo);
            }
            set
            {
                m_maxFileSize = OptionConverter.ToFileSize(value, m_maxFileSize + 1);
            }
        }

        //
        // Summary:
        //     Gets or sets the rolling file count direction.
        //
        // Value:
        //     The rolling file count direction.
        //
        // Remarks:
        //     Indicates if the current file is the lowest numbered file or the highest numbered
        //     file.
        //     By default newer files have lower numbers (log4net.Appender.RollingFileAppender.CountDirection
        //     < 0), i.e. log.1 is most recent, log.5 is the 5th backup, etc...
        //     log4net.Appender.RollingFileAppender.CountDirection >= 0 does the opposite i.e.
        //     log.1 is the first backup made, log.5 is the 5th backup made, etc. For infinite
        //     backups use log4net.Appender.RollingFileAppender.CountDirection >= 0 to reduce
        //     rollover costs.
        //     The default file count direction is -1.
        public int CountDirection
        {
            get
            {
                return m_countDirection;
            }
            set
            {
                m_countDirection = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the rolling style.
        //
        // Value:
        //     The rolling style.
        //
        // Remarks:
        //     The default rolling style is log4net.Appender.RollingFileAppender.RollingMode.Composite.
        //     When set to log4net.Appender.RollingFileAppender.RollingMode.Once this appender's
        //     log4net.Appender.FileAppender.AppendToFile property is set to false, otherwise
        //     the appender would append to a single file rather than rolling the file each
        //     time it is opened.
        public RollingMode RollingStyle
        {
            get
            {
                return m_rollingStyle;
            }
            set
            {
                m_rollingStyle = value;
                switch (m_rollingStyle)
                {
                    case RollingMode.Once:
                        m_rollDate = false;
                        m_rollSize = false;
                        base.AppendToFile = false;
                        break;
                    case RollingMode.Size:
                        m_rollDate = false;
                        m_rollSize = true;
                        break;
                    case RollingMode.Date:
                        m_rollDate = true;
                        m_rollSize = false;
                        break;
                    case RollingMode.Composite:
                        m_rollDate = true;
                        m_rollSize = true;
                        break;
                }
            }
        }

        //
        // Summary:
        //     Gets or sets a value indicating whether to preserve the file name extension when
        //     rolling.
        //
        // Value:
        //     true if the file name extension should be preserved.
        //
        // Remarks:
        //     By default file.log is rolled to file.log.yyyy-MM-dd or file.log.curSizeRollBackup.
        //     However, under Windows the new file name will loose any program associations
        //     as the extension is changed. Optionally file.log can be renamed to file.yyyy-MM-dd.log
        //     or file.curSizeRollBackup.log to maintain any program associations.
        public bool PreserveLogFileNameExtension
        {
            get
            {
                return m_preserveLogFileNameExtension;
            }
            set
            {
                m_preserveLogFileNameExtension = value;
            }
        }

        //
        // Summary:
        //     Gets or sets a value indicating whether to always log to the same file.
        //
        // Value:
        //     true if always should be logged to the same file, otherwise false.
        //
        // Remarks:
        //     By default file.log is always the current file. Optionally file.log.yyyy-mm-dd
        //     for current formatted datePattern can by the currently logging file (or file.log.curSizeRollBackup
        //     or even file.log.yyyy-mm-dd.curSizeRollBackup).
        //     This will make time based rollovers with a large number of backups much faster
        //     as the appender it won't have to rename all the backups!
        public bool StaticLogFileName
        {
            get
            {
                return m_staticLogFileName;
            }
            set
            {
                m_staticLogFileName = value;
            }
        }

        //
        // Summary:
        //     Initializes a new instance of the log4net.Appender.RollingFileAppender class.
        //
        // Remarks:
        //     Default constructor.
        public RollingFileAppenderStartWithOne()
        {
        }

        //
        // Summary:
        //     Cleans up all resources used by this appender.
        ~RollingFileAppenderStartWithOne()
        {
            if (m_mutexForRolling != null)
            {
                m_mutexForRolling.Dispose();
                m_mutexForRolling = null;
            }
        }

        //
        // Summary:
        //     Sets the quiet writer being used.
        //
        // Parameters:
        //   writer:
        //     the writer to set
        //
        // Remarks:
        //     This method can be overridden by sub classes.
        protected override void SetQWForFiles(TextWriter writer)
        {
            base.QuietWriter = new CountingQuietTextWriter(writer, ErrorHandler);
        }

        //
        // Summary:
        //     Write out a logging event.
        //
        // Parameters:
        //   loggingEvent:
        //     the event to write to file.
        //
        // Remarks:
        //     Handles append time behavior for RollingFileAppender. This checks if a roll over
        //     either by date (checked first) or time (checked second) is need and then appends
        //     to the file last.
        protected override void Append(LoggingEvent loggingEvent)
        {
            AdjustFileBeforeAppend();
            base.Append(loggingEvent);
        }

        //
        // Summary:
        //     Write out an array of logging events.
        //
        // Parameters:
        //   loggingEvents:
        //     the events to write to file.
        //
        // Remarks:
        //     Handles append time behavior for RollingFileAppender. This checks if a roll over
        //     either by date (checked first) or time (checked second) is need and then appends
        //     to the file last.
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            AdjustFileBeforeAppend();
            base.Append(loggingEvents);
        }

        //
        // Summary:
        //     Performs any required rolling before outputting the next event
        //
        // Remarks:
        //     Handles append time behavior for RollingFileAppender. This checks if a roll over
        //     either by date (checked first) or time (checked second) is need and then appends
        //     to the file last.
        protected virtual void AdjustFileBeforeAppend()
        {
            try
            {
                if (m_mutexForRolling != null)
                {
                    m_mutexForRolling.WaitOne();
                }

                if (m_rollDate)
                {
                    DateTime now = m_dateTime.Now;
                    if (now >= m_nextCheck)
                    {
                        m_now = now;
                        m_nextCheck = NextCheckDate(m_now, m_rollPoint);
                        RollOverTime(fileIsOpen: true);
                    }
                }

                if (m_rollSize && File != null && ((CountingQuietTextWriter)base.QuietWriter).Count >= m_maxFileSize)
                {
                    RollOverSize();
                }
            }
            finally
            {
                if (m_mutexForRolling != null)
                {
                    m_mutexForRolling.ReleaseMutex();
                }
            }
        }

        //
        // Summary:
        //     Creates and opens the file for logging. If log4net.Appender.RollingFileAppender.StaticLogFileName
        //     is false then the fully qualified name is determined and used.
        //
        // Parameters:
        //   fileName:
        //     the name of the file to open
        //
        //   append:
        //     true to append to existing file
        //
        // Remarks:
        //     This method will ensure that the directory structure for the fileName specified
        //     exists.
        protected override void OpenFile(string fileName, bool append)
        {
            lock (this)
            {
                fileName = GetNextOutputFileName(fileName);
                long count = 0L;
                if (append)
                {
                    using (base.SecurityContext.Impersonate(this))
                    {
                        if (System.IO.File.Exists(fileName))
                        {
                            count = new FileInfo(fileName).Length;
                        }
                    }
                }
                else if (LogLog.IsErrorEnabled && m_maxSizeRollBackups != 0 && FileExists(fileName))
                {
                    LogLog.Error(declaringType, "RollingFileAppender: INTERNAL ERROR. Append is False but OutputFile [" + fileName + "] already exists.");
                }

                if (!m_staticLogFileName)
                {
                    m_scheduledFilename = fileName;
                }

                base.OpenFile(fileName, append);
                ((CountingQuietTextWriter)base.QuietWriter).Count = count;
            }
        }

        //
        // Summary:
        //     Get the current output file name
        //
        // Parameters:
        //   fileName:
        //     the base file name
        //
        // Returns:
        //     the output file name
        //
        // Remarks:
        //     The output file name is based on the base fileName specified. If log4net.Appender.RollingFileAppender.StaticLogFileName
        //     is set then the output file name is the same as the base file passed in. Otherwise
        //     the output file depends on the date pattern, on the count direction or both.
        protected string GetNextOutputFileName(string fileName)
        {
            if (!m_staticLogFileName)
            {
                fileName = fileName.Trim();
                if (m_rollDate)
                {
                    fileName = CombinePath(fileName, m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo));
                }

                if (m_countDirection >= 0)
                {
                    fileName = CombinePath(fileName, "." + m_curSizeRollBackups);
                }
            }

            return fileName;
        }

        //
        // Summary:
        //     Determines curSizeRollBackups (only within the current roll point)
        private void DetermineCurSizeRollBackups()
        {
            //RoniG, start rolling with 1
            m_curSizeRollBackups = 1;
            string text = null;
            string baseFile = null;
            using (base.SecurityContext.Impersonate(this))
            {
                text = Path.GetFullPath(m_baseFileName);
                baseFile = Path.GetFileName(text);
            }

            ArrayList existingFiles = GetExistingFiles(text);
            InitializeRollBackups(baseFile, existingFiles);
            LogLog.Debug(declaringType, "curSizeRollBackups starts at [" + m_curSizeRollBackups + "]");
        }

        //
        // Summary:
        //     Generates a wildcard pattern that can be used to find all files that are similar
        //     to the base file name.
        //
        // Parameters:
        //   baseFileName:
        private string GetWildcardPatternForFile(string baseFileName)
        {
            if (m_preserveLogFileNameExtension)
            {
                return Path.GetFileNameWithoutExtension(baseFileName) + "*" + Path.GetExtension(baseFileName);
            }

            return baseFileName + "*";
        }

        //
        // Summary:
        //     Builds a list of filenames for all files matching the base filename plus a file
        //     pattern.
        //
        // Parameters:
        //   baseFilePath:
        private ArrayList GetExistingFiles(string baseFilePath)
        {
            ArrayList arrayList = new ArrayList();
            string text = null;
            using (base.SecurityContext.Impersonate(this))
            {
                string fullPath = Path.GetFullPath(baseFilePath);
                text = Path.GetDirectoryName(fullPath);
                if (Directory.Exists(text))
                {
                    string fileName = Path.GetFileName(fullPath);
                    string[] files = Directory.GetFiles(text, GetWildcardPatternForFile(fileName));
                    if (files != null)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileName2 = Path.GetFileName(files[i]);
                            if (fileName2.StartsWith(Path.GetFileNameWithoutExtension(fileName)))
                            {
                                arrayList.Add(fileName2);
                            }
                        }
                    }
                }
            }

            LogLog.Debug(declaringType, "Searched for existing files in [" + text + "]");
            return arrayList;
        }

        //
        // Summary:
        //     Initiates a roll over if needed for crossing a date boundary since the last run.
        private void RollOverIfDateBoundaryCrossing()
        {
            if (m_staticLogFileName && m_rollDate && FileExists(m_baseFileName))
            {
                DateTime dateTime;
                using (base.SecurityContext.Impersonate(this))
                {
                    dateTime = ((!(DateTimeStrategy is UniversalDateTime)) ? System.IO.File.GetLastWriteTime(m_baseFileName) : System.IO.File.GetLastWriteTimeUtc(m_baseFileName));
                }

                LogLog.Debug(declaringType, "[" + dateTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo) + "] vs. [" + m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo) + "]");
                if (!dateTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo).Equals(m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo)))
                {
                    m_scheduledFilename = CombinePath(m_baseFileName, dateTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo));
                    LogLog.Debug(declaringType, "Initial roll over to [" + m_scheduledFilename + "]");
                    RollOverTime(fileIsOpen: false);
                    LogLog.Debug(declaringType, "curSizeRollBackups after rollOver at [" + m_curSizeRollBackups + "]");
                }
            }
        }

        //
        // Summary:
        //     Initializes based on existing conditions at time of log4net.Appender.RollingFileAppender.ActivateOptions.
        //
        // Remarks:
        //     Initializes based on existing conditions at time of log4net.Appender.RollingFileAppender.ActivateOptions.
        //     The following is done
        //     • determine curSizeRollBackups (only within the current roll point)
        //     • initiates a roll over if needed for crossing a date boundary since the last
        //     run.
        protected void ExistingInit()
        {
            DetermineCurSizeRollBackups();
            RollOverIfDateBoundaryCrossing();
            if (base.AppendToFile)
            {
                return;
            }

            string nextOutputFileName = GetNextOutputFileName(m_baseFileName);
            bool flag;
            using (base.SecurityContext.Impersonate(this))
            {
                flag = System.IO.File.Exists(nextOutputFileName);
            }

            if (flag)
            {
                if (m_maxSizeRollBackups == 0)
                {
                    LogLog.Debug(declaringType, "Output file [" + nextOutputFileName + "] already exists. MaxSizeRollBackups is 0; cannot roll. Overwriting existing file.");
                    return;
                }

                LogLog.Debug(declaringType, "Output file [" + nextOutputFileName + "] already exists. Not appending to file. Rolling existing file out of the way.");
                RollOverRenameFiles(nextOutputFileName);
            }
        }

        //
        // Summary:
        //     Does the work of bumping the 'current' file counter higher to the highest count
        //     when an incremental file name is seen. The highest count is either the first
        //     file (when count direction is greater than 0) or the last file (when count direction
        //     less than 0). In either case, we want to know the highest count that is present.
        //
        // Parameters:
        //   baseFile:
        //
        //   curFileName:
        private void InitializeFromOneFile(string baseFile, string curFileName)
        {
            curFileName = curFileName.ToLower();
            baseFile = baseFile.ToLower();
            if (!curFileName.StartsWith(Path.GetFileNameWithoutExtension(baseFile)) || curFileName.Equals(baseFile))
            {
                return;
            }

            if (m_rollDate && !m_staticLogFileName)
            {
                string text = m_dateTime.Now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo).ToLower();
                string value = (m_preserveLogFileNameExtension ? (Path.GetFileNameWithoutExtension(baseFile) + text) : (baseFile + text)).ToLower();
                string value2 = (m_preserveLogFileNameExtension ? Path.GetExtension(baseFile).ToLower() : "");
                if (!curFileName.StartsWith(value) || !curFileName.EndsWith(value2))
                {
                    LogLog.Debug(declaringType, "Ignoring file [" + curFileName + "] because it is from a different date period");
                    return;
                }
            }

            try
            {
                int backUpIndex = GetBackUpIndex(curFileName);
                if (backUpIndex <= m_curSizeRollBackups)
                {
                    return;
                }

                if (m_maxSizeRollBackups != 0)
                {
                    if (-1 == m_maxSizeRollBackups)
                    {
                        m_curSizeRollBackups = backUpIndex;
                    }
                    else if (m_countDirection >= 0)
                    {
                        m_curSizeRollBackups = backUpIndex;
                    }
                    else if (backUpIndex <= m_maxSizeRollBackups)
                    {
                        m_curSizeRollBackups = backUpIndex;
                    }
                }

                LogLog.Debug(declaringType, "File name [" + curFileName + "] moves current count to [" + m_curSizeRollBackups + "]");
            }
            catch (FormatException)
            {
                LogLog.Debug(declaringType, "Encountered a backup file not ending in .x [" + curFileName + "]");
            }
        }

        //
        // Summary:
        //     Attempts to extract a number from the end of the file name that indicates the
        //     number of the times the file has been rolled over.
        //
        // Parameters:
        //   curFileName:
        //
        // Remarks:
        //     Certain date pattern extensions like yyyyMMdd will be parsed as valid backup
        //     indexes.
        private int GetBackUpIndex(string curFileName)
        {
            int val = -1;
            string text = curFileName;
            if (m_preserveLogFileNameExtension)
            {
                text = Path.GetFileNameWithoutExtension(text);
            }

            int num = text.LastIndexOf(".");
            if (num > 0)
            {
                SystemInfo.TryParse(text.Substring(num + 1), out val);
            }

            return val;
        }

        //
        // Summary:
        //     Takes a list of files and a base file name, and looks for 'incremented' versions
        //     of the base file. Bumps the max count up to the highest count seen.
        //
        // Parameters:
        //   baseFile:
        //
        //   arrayFiles:
        private void InitializeRollBackups(string baseFile, ArrayList arrayFiles)
        {
            if (arrayFiles == null)
            {
                return;
            }

            string baseFile2 = baseFile.ToLowerInvariant();
            foreach (string arrayFile in arrayFiles)
            {
                InitializeFromOneFile(baseFile2, arrayFile.ToLowerInvariant());
            }
        }

        //
        // Summary:
        //     Calculates the RollPoint for the datePattern supplied.
        //
        // Parameters:
        //   datePattern:
        //     the date pattern to calculate the check period for
        //
        // Returns:
        //     The RollPoint that is most accurate for the date pattern supplied
        //
        // Remarks:
        //     Essentially the date pattern is examined to determine what the most suitable
        //     roll point is. The roll point chosen is the roll point with the smallest period
        //     that can be detected using the date pattern supplied. i.e. if the date pattern
        //     only outputs the year, month, day and hour then the smallest roll point that
        //     can be detected would be and hourly roll point as minutes could not be detected.
        private RollPoint ComputeCheckPeriod(string datePattern)
        {
            string text = s_date1970.ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
            for (int i = 0; i <= 5; i++)
            {
                string text2 = NextCheckDate(s_date1970, (RollPoint)i).ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
                LogLog.Debug(declaringType, "Type = [" + i + "], r0 = [" + text + "], r1 = [" + text2 + "]");
                if (text != null && text2 != null && !text.Equals(text2))
                {
                    return (RollPoint)i;
                }
            }

            return RollPoint.InvalidRollPoint;
        }

        //
        // Summary:
        //     Initialize the appender based on the options set
        //
        // Remarks:
        //     This is part of the log4net.Core.IOptionHandler delayed object activation scheme.
        //     The log4net.Appender.RollingFileAppender.ActivateOptions method must be called
        //     on this object after the configuration properties have been set. Until log4net.Appender.RollingFileAppender.ActivateOptions
        //     is called this object is in an undefined state and must not be used.
        //     If any of the configuration properties are modified then log4net.Appender.RollingFileAppender.ActivateOptions
        //     must be called again.
        //     Sets initial conditions including date/time roll over information, first check,
        //     scheduledFilename, and calls log4net.Appender.RollingFileAppender.ExistingInit
        //     to initialize the current number of backups.
        public override void ActivateOptions()
        {
            if (m_dateTime == null)
            {
                m_dateTime = new LocalDateTime();
            }

            if (m_rollDate && m_datePattern != null)
            {
                m_now = m_dateTime.Now;
                m_rollPoint = ComputeCheckPeriod(m_datePattern);
                if (m_rollPoint == RollPoint.InvalidRollPoint)
                {
                    throw new ArgumentException("Invalid RollPoint, unable to parse [" + m_datePattern + "]");
                }

                m_nextCheck = NextCheckDate(m_now, m_rollPoint);
            }
            else if (m_rollDate)
            {
                ErrorHandler.Error("Either DatePattern or rollingStyle options are not set for [" + base.Name + "].");
            }

            if (base.SecurityContext == null)
            {
                base.SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }

            using (base.SecurityContext.Impersonate(this))
            {
                base.File = FileAppenderExtended.ConvertToFullPath(base.File.Trim());
                m_baseFileName = base.File;
            }

            m_mutexForRolling = new Mutex(initiallyOwned: false, m_baseFileName.Replace("\\", "_").Replace(":", "_").Replace("/", "_") + "_rolling");
            if (m_rollDate && File != null && m_scheduledFilename == null)
            {
                m_scheduledFilename = CombinePath(File, m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo));
            }

            ExistingInit();
            base.ActivateOptions();
        }

        //
        // Parameters:
        //   path1:
        //
        //   path2:
        //     .1, .2, .3, etc.
        private string CombinePath(string path1, string path2)
        {
            string extension = Path.GetExtension(path1);
            if (m_preserveLogFileNameExtension && extension.Length > 0)
            {
                return Path.Combine(Path.GetDirectoryName(path1), Path.GetFileNameWithoutExtension(path1) + path2 + extension);
            }

            //int extPos=path1.LastIndexOf(".1");
            //if (extPos > 0) path1=path1.Substringk(0, extPos);
            return path1 + path2;
        }

        //
        // Summary:
        //     Rollover the file(s) to date/time tagged file(s).
        //
        // Parameters:
        //   fileIsOpen:
        //     set to true if the file to be rolled is currently open
        //
        // Remarks:
        //     Rollover the file(s) to date/time tagged file(s). Resets curSizeRollBackups.
        //     If fileIsOpen is set then the new file is opened (through SafeOpenFile).
        protected void RollOverTime(bool fileIsOpen)
        {
            if (m_staticLogFileName)
            {
                if (m_datePattern == null)
                {
                    ErrorHandler.Error("Missing DatePattern option in rollOver().");
                    return;
                }

                string path = m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
                if (m_scheduledFilename.Equals(CombinePath(File, path)))
                {
                    ErrorHandler.Error("Compare " + m_scheduledFilename + " : " + CombinePath(File, path));
                    return;
                }

                if (fileIsOpen)
                {
                    CloseFile();
                }

                for (int i = 1; i <= m_curSizeRollBackups; i++)
                {
                    string fromFile = CombinePath(File, "." + i);
                    string toFile = CombinePath(m_scheduledFilename, "." + i);
                    RollFile(fromFile, toFile);
                }

                RollFile(File, m_scheduledFilename);
            }

            m_curSizeRollBackups = 0;
            m_scheduledFilename = CombinePath(File, m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo));
            if (fileIsOpen)
            {
                SafeOpenFile(m_baseFileName, append: false);
            }
        }

        //
        // Summary:
        //     Renames file fromFile to file toFile.
        //
        // Parameters:
        //   fromFile:
        //     Name of existing file to roll.
        //
        //   toFile:
        //     New name for file.
        //
        // Remarks:
        //     Renames file fromFile to file toFile. It also checks for existence of target
        //     file and deletes if it does.
        protected void RollFile(string fromFile, string toFile)
        {
            if (FileExists(fromFile))
            {
                DeleteFile(toFile);
                try
                {
                    LogLog.Debug(declaringType, "Moving [" + fromFile + "] -> [" + toFile + "]");
                    using (base.SecurityContext.Impersonate(this))
                    {
                        System.IO.File.Move(fromFile, toFile);
                    }
                }
                catch (Exception e)
                {
                    ErrorHandler.Error("Exception while rolling file [" + fromFile + "] -> [" + toFile + "]", e, ErrorCode.GenericFailure);
                }
            }
            else
            {
                LogLog.Warn(declaringType, "Cannot RollFile [" + fromFile + "] -> [" + toFile + "]. Source does not exist");
            }
        }

        //
        // Summary:
        //     Test if a file exists at a specified path
        //
        // Parameters:
        //   path:
        //     the path to the file
        //
        // Returns:
        //     true if the file exists
        //
        // Remarks:
        //     Test if a file exists at a specified path
        protected bool FileExists(string path)
        {
            using (base.SecurityContext.Impersonate(this))
            {
                return System.IO.File.Exists(path);
            }
        }

        //
        // Summary:
        //     Deletes the specified file if it exists.
        //
        // Parameters:
        //   fileName:
        //     The file to delete.
        //
        // Remarks:
        //     Delete a file if is exists. The file is first moved to a new filename then deleted.
        //     This allows the file to be removed even when it cannot be deleted, but it still
        //     can be moved.
        protected void DeleteFile(string fileName)
        {
            if (!FileExists(fileName))
            {
                return;
            }

            string text = fileName;
            string text2 = fileName + "." + Environment.TickCount + ".DeletePending";
            try
            {
                using (base.SecurityContext.Impersonate(this))
                {
                    System.IO.File.Move(fileName, text2);
                }

                text = text2;
            }
            catch (Exception exception)
            {
                LogLog.Debug(declaringType, "Exception while moving file to be deleted [" + fileName + "] -> [" + text2 + "]", exception);
            }

            try
            {
                using (base.SecurityContext.Impersonate(this))
                {
                    System.IO.File.Delete(text);
                }

                LogLog.Debug(declaringType, "Deleted file [" + fileName + "]");
            }
            catch (Exception ex)
            {
                if (text == fileName)
                {
                    ErrorHandler.Error("Exception while deleting file [" + text + "]", ex, ErrorCode.GenericFailure);
                }
                else
                {
                    LogLog.Debug(declaringType, "Exception while deleting temp file [" + text + "]", ex);
                }
            }
        }

        //
        // Summary:
        //     Implements file roll base on file size.
        //
        // Remarks:
        //     If the maximum number of size based backups is reached (curSizeRollBackups ==
        //     maxSizeRollBackups) then the oldest file is deleted -- its index determined by
        //     the sign of countDirection. If countDirection < 0, then files {File.1, ..., File.curSizeRollBackups
        //     -1} are renamed to {File.2, ..., File.curSizeRollBackups}. Moreover, File is
        //     renamed File.1 and closed.
        //     A new file is created to receive further log output.
        //     If maxSizeRollBackups is equal to zero, then the File is truncated with no backup
        //     files created.
        //     If maxSizeRollBackups < 0, then File is renamed if needed and no files are deleted.
        protected void RollOverSize()
        {
            CloseFile();
            LogLog.Debug(declaringType, "rolling over count [" + ((CountingQuietTextWriter)base.QuietWriter).Count + "]");
            LogLog.Debug(declaringType, "maxSizeRollBackups [" + m_maxSizeRollBackups + "]");
            LogLog.Debug(declaringType, "curSizeRollBackups [" + m_curSizeRollBackups + "]");
            LogLog.Debug(declaringType, "countDirection [" + m_countDirection + "]");
            RollOverRenameFiles(File);
            if (!m_staticLogFileName && m_countDirection >= 0)
            {
                m_curSizeRollBackups++;
            }

            SafeOpenFile(m_baseFileName, append: false);
        }

        //
        // Summary:
        //     Implements file roll.
        //
        // Parameters:
        //   baseFileName:
        //     the base name to rename
        //
        // Remarks:
        //     If the maximum number of size based backups is reached (curSizeRollBackups ==
        //     maxSizeRollBackups) then the oldest file is deleted -- its index determined by
        //     the sign of countDirection. If countDirection < 0, then files {File.1, ..., File.curSizeRollBackups
        //     -1} are renamed to {File.2, ..., File.curSizeRollBackups}.
        //     If maxSizeRollBackups is equal to zero, then the File is truncated with no backup
        //     files created.
        //     If maxSizeRollBackups < 0, then File is renamed if needed and no files are deleted.
        //     This is called by log4net.Appender.RollingFileAppender.RollOverSize to rename
        //     the files.
        protected void RollOverRenameFiles(string baseFileName)
        {
            if (m_maxSizeRollBackups == 0)
            {
                return;
            }

            if (m_countDirection < 0)
            {
                if (m_curSizeRollBackups == m_maxSizeRollBackups)
                {
                    DeleteFile(CombinePath(baseFileName, "." + m_maxSizeRollBackups));
                    m_curSizeRollBackups--;
                }

                for (int num = m_curSizeRollBackups; num >= 1; num--)
                {
                    RollFile(CombinePath(baseFileName, "." + num), CombinePath(baseFileName, "." + (num + 1)));
                }

                m_curSizeRollBackups++;
                //RollFile(baseFileName, CombinePath(baseFileName, ".1"));
                RollFile(baseFileName, CombinePath(baseFileName, ""));
                return;
            }

            if (m_curSizeRollBackups >= m_maxSizeRollBackups && m_maxSizeRollBackups > 0)
            {
                int num2 = m_curSizeRollBackups - m_maxSizeRollBackups;
                if (m_staticLogFileName)
                {
                    num2++;
                }

                string text = baseFileName;
                if (!m_staticLogFileName)
                {
                    if (m_preserveLogFileNameExtension)
                    {
                        string extension = Path.GetExtension(text);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
                        int num3 = fileNameWithoutExtension.LastIndexOf(".");
                        if (num3 >= 0)
                        {
                            text = fileNameWithoutExtension.Substring(0, num3) + extension;
                        }
                    }
                    else
                    {
                        int num4 = text.LastIndexOf(".");
                        if (num4 >= 0)
                        {
                            text = text.Substring(0, num4);
                        }
                    }
                }

                DeleteFile(CombinePath(text, "." + num2));
            }

            if (m_staticLogFileName)
            {
                m_curSizeRollBackups++;
                RollFile(baseFileName, CombinePath(baseFileName, "." + m_curSizeRollBackups));
            }
        }

        //
        // Summary:
        //     Get the start time of the next window for the current rollpoint
        //
        // Parameters:
        //   currentDateTime:
        //     the current date
        //
        //   rollPoint:
        //     the type of roll point we are working with
        //
        // Returns:
        //     the start time for the next roll point an interval after the currentDateTime
        //     date
        //
        // Remarks:
        //     Returns the date of the next roll point after the currentDateTime date passed
        //     to the method.
        //     The basic strategy is to subtract the time parts that are less significant than
        //     the rollpoint from the current time. This should roll the time back to the start
        //     of the time window for the current rollpoint. Then we add 1 window worth of time
        //     and get the start time of the next window for the rollpoint.
        protected DateTime NextCheckDate(DateTime currentDateTime, RollPoint rollPoint)
        {
            DateTime result = currentDateTime;
            switch (rollPoint)
            {
                case RollPoint.TopOfMinute:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second).AddMinutes(1.0);
                    break;
                case RollPoint.TopOfHour:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second);
                    result = result.AddMinutes(-result.Minute).AddHours(1.0);
                    break;
                case RollPoint.HalfDay:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second);
                    result = result.AddMinutes(-result.Minute);
                    result = ((result.Hour >= 12) ? result.AddHours(-result.Hour).AddDays(1.0) : result.AddHours(12 - result.Hour));
                    break;
                case RollPoint.TopOfDay:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second);
                    result = result.AddMinutes(-result.Minute);
                    result = result.AddHours(-result.Hour).AddDays(1.0);
                    break;
                case RollPoint.TopOfWeek:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second);
                    result = result.AddMinutes(-result.Minute);
                    result = result.AddHours(-result.Hour);
                    result = result.AddDays((double)(7 - result.DayOfWeek));
                    break;
                case RollPoint.TopOfMonth:
                    result = result.AddMilliseconds(-result.Millisecond);
                    result = result.AddSeconds(-result.Second);
                    result = result.AddMinutes(-result.Minute);
                    result = result.AddHours(-result.Hour);
                    result = result.AddDays(1 - result.Day).AddMonths(1);
                    break;
            }

            return result;
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
