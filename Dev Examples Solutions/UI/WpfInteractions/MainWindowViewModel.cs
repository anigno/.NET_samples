#region

using System.ComponentModel;
using System.Windows.Input;

#endregion

namespace WpfInteractions
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Methods

        public void OnLabelMouseEnter()
        {
            WindowTitle = "OnLabelMouseEnter";
        }

        #endregion

        #region Public Properties

        public ICommand WindowCommand
        {
            get { return new SimpleCommand(p_o => true, onWindowCommand); }
        }

        //Binded method just for example result display
        public string WindowTitle
        {
            get { return m_windowTitle; }
            set
            {
                m_windowTitle = value;
                PropertyChanged(this, new PropertyChangedEventArgs("WindowTitle"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Private Methods

        private void onWindowCommand(object p_param)
        {
            WindowTitle = p_param.ToString();
        }

        #endregion

        #region Fields

        private string m_windowTitle;

        #endregion
    }
}