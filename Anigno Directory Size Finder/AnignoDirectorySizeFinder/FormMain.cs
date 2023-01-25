using AnignoraIO.DirectoryWalking;
using AnignoraUI.Common;
using System.Windows.Forms;

namespace AnignoDirectorySizeFinder
{
    public partial class FormMain : Form
    {
        DirectoryWalker m_directoryWalker=new DirectoryWalker();

        public FormMain()
        {
            InitializeComponent();
            m_directoryWalker.DirectoryDiscovered += new System.EventHandler<DirectoryWalkerActivityEventArgs>(m_directoryWalker_DirectoryDiscovered);
            m_directoryWalker.DirectoryLeave += new System.EventHandler<DirectoryDataEventArgs>(m_directoryWalker_DirectoryLeave);
            m_directoryWalker.FileDiscovered += new System.EventHandler<DirectoryWalkerActivityEventArgs>(m_directoryWalker_FileDiscovered);
            m_directoryWalker.WalkingEnded += new System.EventHandler<DirectoryWalkerActivityEventArgs>(m_directoryWalker_WalkingEnded);
            m_directoryWalker.WalkingStarted += new System.EventHandler<DirectoryWalkerActivityEventArgs>(m_directoryWalker_WalkingStarted);
        }

        void m_directoryWalker_WalkingStarted(object sender, DirectoryWalkerActivityEventArgs e)
        {
            CrossThreadActivities.Do(this, p_main =>
                                               {
                                                   buttonStart.Enabled = false;
                                                   buttonStop.Enabled = true;
                                                   listViewResults.Items.Clear();
                                               });
        }

        void m_directoryWalker_WalkingEnded(object sender, DirectoryWalkerActivityEventArgs e)
        {
            CrossThreadActivities.Do(this, p_main =>
                                               {
                                                   buttonStart.Enabled = true;
                                                   buttonStop.Enabled = false;
                                               });
        }

        void m_directoryWalker_FileDiscovered(object sender, DirectoryWalkerActivityEventArgs e)
        {
            //Nothing
        }

        void m_directoryWalker_DirectoryLeave(object sender, DirectoryDataEventArgs e)
        {
            if (e.Size / 1024 / 1024 < numericUpDownMinSize.Value) return;
            CrossThreadActivities.Do(this, p_main =>
                                               {
                                                   string size = (e.Size/1024/1024).ToString();
                                                   string files = e.FilesCount.ToString();
                                                   ListViewItem item = new ListViewItem(new[] {e.Path, files, size});
                                                   listViewResults.Items.Add(item);
                                               });
        }

        void m_directoryWalker_DirectoryDiscovered(object sender, DirectoryWalkerActivityEventArgs e)
        {
            CrossThreadActivities.Do(this, p_main => { labelCurrentPath.Text = e.Path; });
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            listViewResults.Items.Clear();
            m_directoryWalker.BeginWalk(textBoxBrowseRoot.Text,WalkingStyleEnum.DirectoriesFirst);
        }

        private void listViewResults_Click(object sender, System.EventArgs e)
        {
            if (listViewResults.SelectedItems.Count < 1) return;
            string selectedPath = listViewResults.SelectedItems[0].SubItems[0].Text;
            Clipboard.SetText(selectedPath);
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            m_directoryWalker.AbortWalk();
        }

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            Text += " " + Application.ProductVersion;
        }
    }
}
