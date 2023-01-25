using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace AnignoraIO.FileCopyNoneBlocking
{
    public partial class FileCopyNoneBlockingControl : UserControl
    {
        #region Constructors

        public FileCopyNoneBlockingControl()
        {
            InitializeComponent();
            _fcManager.OnFileCopyEnded += _fcManager_OnFileCopyEnded;
            _fcManager.OnFileCopyError += _fcManager_OnFileCopyError;
            _fcManager.OnFileCopyInProgress += _fcManager_OnFileCopyInProgress;
            _fcManager.OnFileCopyStarted += _fcManager_OnFileCopyStarted;
            _fcManager.OnFileCopyAborted += _fcManager_OnFileCopyAborted;
            OnFileCopyInProgress += FileCopyNoneBlockingControl_OnFileCopyInProgress;
            OnFileCopyEnded += FileCopyNoneBlockingControl_OnFileCopyEnded;
        }

        #endregion

        #region Public Methods

        public void StartCopy(string source, string destination)
        {
            labelSource.Text = source;
            labelDestination.Text = destination;
            _fcManager.Copy(source, destination);
            sw.Reset();
            sw.Start();
        }

        #endregion

        #region Events

        [Category(CATEGORY_STRING)]
        public event FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler OnFileCopyAborted;

        [Category(CATEGORY_STRING)]
        public event FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler OnFileCopyEnded;

        [Category(CATEGORY_STRING)]
        public event FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler OnFileCopyError;

        [Category(CATEGORY_STRING)]
        public event FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler OnFileCopyInProgress;

        [Category(CATEGORY_STRING)]
        public event FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler OnFileCopyStarted;

        #endregion

        #region Private Methods

        private void _fcManager_OnFileCopyAborted(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            RaiseEvent(OnFileCopyAborted, sourceFile, destinationFile, result, message);
        }

        private void _fcManager_OnFileCopyEnded(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            RaiseEvent(OnFileCopyEnded, sourceFile, destinationFile, result, message);
        }

        private void _fcManager_OnFileCopyError(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            RaiseEvent(OnFileCopyError, sourceFile, destinationFile, result, message);
        }

        private void _fcManager_OnFileCopyInProgress(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            RaiseEvent(OnFileCopyInProgress, sourceFile, destinationFile, result, message);
        }

        private void _fcManager_OnFileCopyStarted(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            RaiseEvent(OnFileCopyStarted, sourceFile, destinationFile, result, message);
        }

        private void buttonAbort_Click(object sender, System.EventArgs e)
        {
            _fcManager.Abort();
        }

        private void FileCopyNoneBlockingControl_OnFileCopyEnded(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            labelSource.Text = labelDestination.Text = "";
            anignoProgressBarFlatCopyProgress.Value = 0;
            anignoProgressBarFlatCopyProgress.Text = "0%";
            Refresh();
        }

        private void FileCopyNoneBlockingControl_OnFileCopyInProgress(string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            long milSec = sw.ElapsedMilliseconds;
            if (milSec == 0) milSec = 1;
            int progress = (int) (_fcManager.CurrentCopiedBytes*100/_fcManager.CurrentTotalBytes);
            anignoProgressBarFlatCopyProgress.Value = progress;
            anignoProgressBarFlatCopyProgress.ProgressBarText = progress + "% at " + COPY_BUFFER_SIZE/milSec + " kb/sec";
            sw.Reset();
            sw.Start();
        }

        private void RaiseEvent(FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler fileCopyEventHandler, string sourceFile, string destinationFile, FileCopyNoneBlockingManager.FileCopyOperationResultEnum result, string message)
        {
            if (fileCopyEventHandler == null) return;
            if (InvokeRequired)
            {
                Invoke(new FileCopyNoneBlockingManager.FileCopyNoneBlockingEventHandler(fileCopyEventHandler), sourceFile, destinationFile, result, message);
            }
            else
            {
                fileCopyEventHandler(sourceFile, destinationFile, result, message);
            }
        }

        #endregion

        #region Fields

        private readonly FileCopyNoneBlockingManager _fcManager = new FileCopyNoneBlockingManager(COPY_BUFFER_SIZE);
        private readonly Stopwatch sw = new Stopwatch();

        #endregion

        #region Constants

        public const string CATEGORY_STRING = " FileCopyNoneBlockingControl";
        public const int COPY_BUFFER_SIZE = 1024*1024*16;

        #endregion
    }
}