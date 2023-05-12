using System;
using System.Windows;
using System.Windows.Input;

namespace WpfSimple
{
    internal class MainWindowViewModel : ViewModelBase
    {
        // create and refference to the model
        private readonly MainWindowModel model = new MainWindowModel();

        // declare viewmodel variables behind the properties
        private ICommand? someButton;
        private string someName = "";
        private string someTimeString = "";

        public MainWindowViewModel()
        {
            // register to model events
            model.OnTimeChanged += Model_OnTimeChanged;
        }

        // viewmodel properties supporting the view
        public string Name
        {
            get { return someName; }
            set
            {
                if (value != someName)
                {
                    someName = value;
                    OnPropertyChanged(nameof(Name));
                    //update the model with the changes
                    model.Name = someName;
                }
            }
        }

        public string Time
        {
            get { return someTimeString; }
            set
            {
                if (value != someTimeString)
                {
                    someTimeString = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }

        public ICommand OkButton
        {
            get
            {
                if (someButton == null)
                {
                    Action action = () =>
                    {
                        MessageBox.Show($"{Name}");
                    };
                    Func<bool> canExecute = () => true;
                    someButton = new CommandImplementation(action, canExecute);
                }
                return someButton;
            }
        }

        //model event handler
        private void Model_OnTimeChanged(object? sender, string timeString)
        {
            Time = timeString;
        }

        //private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    OnPropertyChanged(e.PropertyName);
        //}



        

    }
}
