using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WpfSimple
{
    internal class MainWindowModel
    {

        public readonly Timer PeriodicTimer = new Timer();
        private string name = "NO_NAME";

        public MainWindowModel()
        {
            PeriodicTimer.Interval = 1000;
            PeriodicTimer.Elapsed += OnTimerElapsed;
            PeriodicTimer.Start();
        }
        public string Name
        {
            get => name; set
            {
                name = value;
                Console.WriteLine($"name has changed to {name}");
            }
        }

        public string Time { get; set; } = "NO_TIME";

        public event EventHandler<string>? OnTimeChanged;

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Time = DateTime.Now.ToString("T");
            OnTimeChanged?.Invoke(this, Time);
        }

        
    }
}

