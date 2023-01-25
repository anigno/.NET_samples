using System;
using System.Windows.Input;

namespace SpecialListOfItems
{
    public class CommandImplementor : ICommand
    {
        #region Constructors

        public CommandImplementor(Action<object> p_executeMethod, Func<object, bool> p_canExecuteMethod)
        {
            m_ExecuteMethod = p_executeMethod;
            m_CanExecuteMethod = p_canExecuteMethod;
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return m_CanExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            m_ExecuteMethod(parameter);
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Fields

        private readonly Action<object> m_ExecuteMethod;
        private readonly Func<object, bool> m_CanExecuteMethod;

        #endregion
    }
}