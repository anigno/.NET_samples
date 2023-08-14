using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples
{
    internal class DelegatesActionAndFunc
    {
        public DelegatesActionAndFunc()
        {
            Action WriteHello = () =>
            {
                Console.WriteLine("Hello");
            };
            Action<int, int> WriteSum = (int a, int b) =>
            {
                Console.WriteLine(a + b);
            };
            WriteHello();
            WriteSum(2, 3);
            Func<float, float, float> Add = (float a, float b) =>
            {
                Console.WriteLine(a + b);
                return a + b;
            };
            float a = Add(4, 7);
            Console.WriteLine(a);

            Operation Div = (int a, int b) => { return a / b; };
            Console.WriteLine(Div(4, 2));
        }
        delegate int Operation(int a, int b);

        public event EventHandler<string> OnEventWithString = delegate
        {
        };

        public event EventHandler<EventArgs> OnEvent1;

        public void EventsAndDelegates()
        {
            OnEventWithString(this, "event raised");
            OnEventWithString += DelegatesActionAndFunc_OnEventWithString;
            OnEventWithString(this, "event raised");

        }

        private void DelegatesActionAndFunc_OnEventWithString(object? sender, string e)
        {
            Console.WriteLine(e);
        }
    }
}
