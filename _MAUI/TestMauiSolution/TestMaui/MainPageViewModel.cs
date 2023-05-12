using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestMaui
{
    internal class MainPageViewModel: ViewModelBase
    {
        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand PasswordChangedCommand { get; }

        public MainPageViewModel()
        {
            PasswordChangedCommand = new Command(() =>
            {
                // Do any logic you need to handle the password change
            });
        }
    }
}
