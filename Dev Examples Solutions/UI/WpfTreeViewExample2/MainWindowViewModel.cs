#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Rafael.Infra.WPF.Commands;

#endregion

namespace WpfTreeViewExample2
{
    public class MyElement : TreeViewItem
    {
        private string m_myName;

        public string MyName
        {
            get { return m_myName; }
            set
            {
                m_myName = value;
                Header = m_myName;
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            Test = "jubba";
            m_foldersSource = new List<Folder>();
            //add Root items
            FoldersSource.Add(new Folder { FolderLabel = "Dummy1", FullPath = @"C:\dummy1" });
            FoldersSource.Add(new Folder { FolderLabel = "Dummy2", FullPath = @"C:\dummy2" });
            FoldersSource.Add(new Folder { FolderLabel = "Dummy3", FullPath = @"C:\dummy3" });
            FoldersSource.Add(new Folder { FolderLabel = "Dummy4", FullPath = @"C:\dummy4" });
            //add sub items
            FoldersSource[0].Folders = new List<Folder>();
            FoldersSource[0].Folders.Add(new Folder { FolderLabel = "Dummy11", FullPath = @"C:\dummy11" });
            FoldersSource[0].Folders.Add(new Folder { FolderLabel = "Dummy12", FullPath = @"C:\dummy12" });
            FoldersSource[0].Folders.Add(new Folder { FolderLabel = "Dummy13", FullPath = @"C:\dummy13" });
            FoldersSource[0].Folders.Add(new Folder { FolderLabel = "Dummy14", FullPath = @"C:\dummy14" });

            ////////////////////////////////////
            TreeViewItemSource.Add(new TreeViewItem { Header = "Item1", Tag = "Item 1 Tag" });
            TreeViewItemSource.Add(new TreeViewItem { Header = "Item2", Tag = "Item 2 Tag" });
            TreeViewItem treeViewItem = new TreeViewItem { Header = "Item3", Tag = "Item 3 Tag" };
            TreeViewItemSource.Add(treeViewItem);
            treeViewItem.Items.Add(new TreeViewItem { Header = "Child1", Tag = "Child 1 Tag" });

            MessageLabel = "Select to get Item's Tag";

            /////////////////////////////////////
            MyTreeViewItemSource.Add(new MyElement { MyName = "Name1", Tag = "Item 1 Tag" });
            MyTreeViewItemSource.Add(new MyElement { MyName = "Name2", Tag = "Item 2 Tag" });
            MyElement myTreeViewItem = new MyElement { MyName = "Name3", Tag = "Item 3 Tag" };
            MyTreeViewItemSource.Add(myTreeViewItem);
            myTreeViewItem.Items.Add(new MyElement { MyName = "Name of Child1", Tag = "Child 1 Tag" });

        }

        #endregion

        #region Public Properties

        public string Test { get; set; }

        public List<Folder> FoldersSource
        {
            get { return m_foldersSource; }
            set
            {
                m_foldersSource = value;
                NotifiyPropertyChanged("FoldersSource");
            }
        }

        public ObservableCollection<TreeViewItem> TreeViewItemSource
        {
            get { return m_treeViewItemSource; }
            set
            {
                m_treeViewItemSource = value;
                NotifiyPropertyChanged("TreeViewItemSource");
            }
        }

        public ICommand SetSelectedItemCommand
        {
            get { return new CommandEx(p_o => { MessageLabel = ((TreeViewItem)p_o).Tag.ToString(); }, p_o => true); }
        }

        public string MessageLabel
        {
            get { return m_messageLabel; }
            set
            {
                m_messageLabel = value;
                NotifiyPropertyChanged("MessageLabel");
            }
        }

        public ICommand AddItemClicked
        {
            get
            {
                return new CommandEx(p_o =>
                {
                    TreeViewItemSource.Add(new TreeViewItem() { Header = DateTime.Now.Second, Tag = "TAG :" + DateTime.Now });

                }, p_o => true);
            }
        }

        public ObservableCollection<MyElement> MyTreeViewItemSource
        {
            get { return m_myTreeViewItemSource; }
            set
            {
                m_myTreeViewItemSource = value;
                NotifiyPropertyChanged("MyTreeViewItemSource");
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        private void RunOverDispatcher(Action p_action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                //Already run from dispatcher
                p_action();
                return;
            }
            Application.Current.Dispatcher.BeginInvoke(p_action, DispatcherPriority.Normal);
        }

        private void NotifiyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Fields

        private List<Folder> m_foldersSource;
        private ObservableCollection<TreeViewItem> m_treeViewItemSource = new ObservableCollection<TreeViewItem>();
        private string m_messageLabel;
        private ObservableCollection<MyElement> m_myTreeViewItemSource = new ObservableCollection<MyElement>();

        #endregion
    }
}