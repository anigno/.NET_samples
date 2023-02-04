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
        int id = 111;
        string? name = "aaa";
        string time = "00:00:00";
        public readonly Timer TimerSeconds = new Timer();

        public MainWindowModel()
        {
            TimerSeconds.Interval = 1000;
            TimerSeconds.Elapsed += Timer_Elapsed;
            TimerSeconds.Start();
        }



        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time = DateTime.Now.ToString("T");
        }

        public int Id { get => id; set => id = value; }
        public string? Name { get => name; set => name = value; }
        public string Time { get => time; set => time = value; }
    }
}

