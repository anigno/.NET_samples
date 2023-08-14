using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples
{
    internal class EventsSample
    {
        public event EventHandler OnEventsReceived;

        public event EventHandler<string> OnStringEvent;

        public event Action<int, int> OnIntsEvent;

        public event EventHandler OnNoSubscriptionEvent;

        public event EventHandler<SpecialEventArgs> OnSpecialEvent = delegate { };

        public EventsSample()
        {
            OnEventsReceived += EventsSample_OnEventsReceived;
            OnStringEvent += EventsSample_OnStringEvent;
            OnIntsEvent += EventsSample_OnIntsEvent;
            OnSpecialEvent += EventsSample_OnSpecialEvent;

            OnEventsReceived(this, EventArgs.Empty);
            OnEventsReceived.Invoke(this, EventArgs.Empty);
            OnStringEvent(this, "Hello");
            OnStringEvent.Invoke(this, "Hello");
            OnIntsEvent(3, 2);
            OnIntsEvent.Invoke(3, 2);

            OnNoSubscriptionEvent?.Invoke(this, EventArgs.Empty);

            OnSpecialEvent(this, new SpecialEventArgs(17, "hi"));


        }

        private void EventsSample_OnSpecialEvent(object? sender, SpecialEventArgs e)
        {
            Console.WriteLine($"event occured {e.myInt} {e.myString}");
        }

        private void EventsSample_OnIntsEvent(int arg1, int arg2)
        {
            Console.WriteLine($"event occured {arg1} {arg2}");
        }

        private void EventsSample_OnStringEvent(object? sender, string e)
        {
            Console.WriteLine($"event occured {e}");
        }

        private void EventsSample_OnEventsReceived(object? sender, EventArgs e)
        {
            Console.WriteLine($"event occured ");
        }
    }
    public class SpecialEventArgs : EventArgs
    {
        public int myInt { get; private set; }
        public string myString { get; private set; }

        public SpecialEventArgs(int i, string s)
        {
            myInt = i;
            myString = s;
        }
    }
}
