#region

using System.ComponentModel;

#endregion

namespace MarkupExamples
{
    public class TheViewModel : INotifyPropertyChanged
    {
        #region Public Properties

        public string ViewModelLabel
        {
            get { return m_viewModelLabel; }
            set
            {
                m_viewModelLabel = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ViewModelLabel"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private string m_viewModelLabel = "The View Model's string property";

        #endregion
    }
}