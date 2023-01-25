#region

using System;
using System.Windows.Input;

#endregion

namespace WpfInteractions
{
    public class SimpleCommand : ICommand
    {
        #region Constructors

        public SimpleCommand(Func<object, bool> p_canExecute, Action<object> p_execute)
        {
            m_canExecute = p_canExecute;
            m_execute = p_execute;
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object p_parameter)
        {
            return m_canExecute(p_parameter);
        }

        public void Execute(object p_parameter)
        {
            m_execute(p_parameter);
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion

        #region Fields

        private readonly Func<object, bool> m_canExecute;
        private readonly Action<object> m_execute;

        #endregion
    }
}