using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfSimple
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CommandImplementation : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExcute;

        public CommandImplementation(Action action, Func<bool> canExecute)
        {
            _canExcute = canExecute;
            _action = action;
        }

        public event EventHandler? CanExecuteChanged;

        public void OnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(sender, e);
        }

        public bool CanExecute(object? parameter) { return _canExcute(); }

        public void Execute(object? parameter) {_action(); }
    }
}
