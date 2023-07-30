using System;
using System.Drawing;
using System.Windows.Forms;
using AnignoFileSync.Data;
using System.Threading;
using LoggingProvider;
using AnignoraDataTypes.DataManager;
using AnignoraUI.Common;
using AnignoraIO;

namespace AnignoFileSync2010
{
    public partial class FormMain : Form
    {


		#region (------------------  Enums  ------------------)
        private enum ActivityEnum
        {
            Copy,
            Delete,
            Start,
            End,
            Error
        }
		#endregion (------------------  Enums  ------------------)


		#region (------------------  Fields  ------------------)
        private readonly ApplicationData data = DataManager<ApplicationData>.DataItem;
private readonly Color color_copy = Color.Green;
        private readonly Color color_delete = Color.Purple;
        private readonly Color color_start = Color.Blue;
        private readonly Color color_end = Color.DarkBlue;
        private readonly Color color_error = Color.Red;
        private Thread SyncThread;
		#endregion (------------------  Fields  ------------------)


		#region (------------------  Constructors  ------------------)
        public FormMain()
        {
            InitializeComponent();
        }
		#endregion (------------------  Constructors  ------------------)


		#region (------------------  Delegates  ------------------)
        private delegate void ListViewItemDelegate(ListViewItem item);
        private delegate void ListViewSyncItemDelegate(SyncItem item);
		#endregion (------------------  Delegates  ------------------)


		#region (------------------  Event Handlers  ------------------)
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Logger.Log();
            if (SyncThread != null && SyncThread.IsAlive)
            {
                SyncThread.Abort();
                Logger.LogWarning("Aborted");
            }
            Close();
        }

        void ds_OnDirectoryEnter(string path)
        {
            CrossThreadActivities.Do(labelCurrent, c => c.Text = path);
        }

        void ds_OnEmptyDirDelete(string path)
        {
            AddActivity(ActivityEnum.Delete,"Delete empty directory: " + path);
        }

        void ds_OnEnded(string path)
        {
            Logger.LogDebug(path);
            AddActivity(ActivityEnum.End,"Ended: " + path);
        }

        void ds_OnError(string path, string text)
        {
            AddActivity(ActivityEnum.Error,path + " " + text);
        }

        void ds_OnFileCopyStart(string path)
        {
            Logger.LogDebug(path);
            AddActivity(ActivityEnum.Copy,"File to copy: " + path);
        }

        void ds_OnFileCopyToHistory(string path)
        {
            AddActivity(ActivityEnum.Copy,"File to history: " + path);
        }

        void ds_OnFileDelete(string path)
        {
            Logger.LogDebug(path);
            AddActivity(ActivityEnum.Delete,"File to delete: " + path);
        }

        void ds_OnStarted(string path)
        {
            Logger.LogDebug(path);
            AddActivity(ActivityEnum.Start,"Started: " + path);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Logger.Log();
            Text += " " + Application.ProductVersion;
            foreach (SyncItem syncItem in data.SyncItems)
            {
                ListViewItem item = new ListViewItem(new[] { syncItem.SourceDir, syncItem.DestDir, syncItem.KeepHistory.ToString() });
                item.Tag = syncItem;
                listViewSyncItems.Items.Add(item);
                Logger.LogDebug("SyncItem loaded source:{0} dest:{1} keepHistory:{2}", syncItem.SourceDir, syncItem.DestDir, syncItem.KeepHistory);
            }
            SyncThread = new Thread(SyncThreadStart);
            SyncThread.Start();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            labelDate.Text = DateTime.Now.ToString();
        }
		#endregion (------------------  Event Handlers  ------------------)


		#region (------------------  Overridden Methods  ------------------)
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Logger.Log();
            base.OnFormClosing(e);
            if (SyncThread != null && SyncThread.IsAlive)
            {
                SyncThread.Abort();
            }
        }
		#endregion (------------------  Overridden Methods  ------------------)


		#region (------------------  Private Methods  ------------------)
        private void AddActivity(ActivityEnum activity, string text)
        {
            Logger.LogDebug(text);
            ListViewItem item = new ListViewItem(new[] { DateTime.Now.ToLongTimeString(), text });
            switch (activity)
            {
                case ActivityEnum.Copy:
                    item.ForeColor = color_copy;
                    break;
                case ActivityEnum.Delete:
                    item.ForeColor = color_delete;
                    break;
                case ActivityEnum.End:
                    item.ForeColor = color_end;
                    break;
                case ActivityEnum.Error:
                    item.ForeColor = color_error;
                    break;
                case ActivityEnum.Start:
                    item.ForeColor = color_start;
                    break;
            }
            CrossThreadActivities.Do(listViewActivities, ctrl => ctrl.Items.Add(item));
            ItemEnsureVisibleInvoked(item);
        }

        private void ItemEnsureVisibleInvoked(ListViewItem item)
        {
            if (listViewActivities.InvokeRequired)
            {
                Invoke(new ListViewItemDelegate(ItemEnsureVisibleInvoked), item);
            }
            else
            {
                item.EnsureVisible();
            }
        }

        private void SyncThreadStart()
        {
            Logger.Log();
            foreach (SyncItem syncItem in data.SyncItems)
            {
                UpdateListViewSyncItems(syncItem);
                DirectorySyncronizer ds = new DirectorySyncronizer();
                ds.OnDirectoryEnter += new DirectorySyncronizer.DirectorySyncronizerDelegate(ds_OnDirectoryEnter);
                ds.OnStarted += ds_OnStarted;
                ds.OnFileCopyStart += ds_OnFileCopyStart;
                ds.OnFileDelete += ds_OnFileDelete;
                ds.OnEnded += ds_OnEnded;
                ds.OnError += ds_OnError;
                ds.OnEmptyDirDelete += ds_OnEmptyDirDelete;
                ds.OnFileCopyToHistory += ds_OnFileCopyToHistory;
                ds.Sync(syncItem.SourceDir, syncItem.DestDir, syncItem.KeepHistory,syncItem.Include,syncItem.Exclude);
                ds.OnStarted -= ds_OnStarted;
                ds.OnFileCopyStart -= ds_OnFileCopyStart;
                ds.OnFileDelete -= ds_OnFileDelete;
                ds.OnEnded -= ds_OnEnded;
                ds.OnError -= ds_OnError;
                ds.OnEmptyDirDelete -= ds_OnEmptyDirDelete;
                ds.OnFileCopyToHistory -= ds_OnFileCopyToHistory;
            }
            AddActivity(ActivityEnum.End,"------ Ended All ------");
        }

        private void UpdateListViewSyncItems(SyncItem item)
        {
            if (listViewSyncItems.InvokeRequired)
            {
                Invoke(new ListViewSyncItemDelegate(UpdateListViewSyncItems), item);
            }
            else
            {
                foreach (ListViewItem lvi in listViewSyncItems.Items)
                {
                    if (lvi.Tag == item)
                    {
                        lvi.ForeColor = Color.Blue;
                    }
                }
            }
        }
		#endregion (------------------  Private Methods  ------------------)
    }
}