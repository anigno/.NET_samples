using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WpfSimple
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindowModel _model = new MainWindowModel();
        private ICommand? _someButton;

        public MainWindowViewModel()
        {
        }

        private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public int Id
        {
            get { return _model.Id; }
            set
            {
                if (value != _model.Id)
                {
                    _model.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string? Name
        {
            get { return _model.Name; }
            set
            {
                if (value != _model.Name)
                {
                    _model.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public ICommand OkButton
        {
            get
            {
                if (_someButton == null)
                {
                    Action action = () => MessageBox.Show($"{_model.Name} {_model.Id}");
                    Func<bool> canExecute = () =>
                    {
                        return true;
                    };
                    _someButton = new CommandImplementation(action, canExecute);
                }
                return _someButton;
            }
        }
        public string? Time
        {
            get { return _model.Time; }
            set
            {
                if (value != _model.Time)
                {
                    _model.Name = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }
    }
}
