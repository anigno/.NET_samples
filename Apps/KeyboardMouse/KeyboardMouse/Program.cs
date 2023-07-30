using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnignoraUI.HooksAndImmulate;

namespace KeyboardMouse
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        KeyboardHookController keyHook = new KeyboardHookController();
        MouseHookController mouseHook = new MouseHookController();

        public Program()
        {
            keyHook.KeyDown += KeyHook_KeyDown;
            keyHook.KeyUp += KeyHook_KeyUp;
            while(true)
            {
                Console.ReadKey(false);
            }
        }

        private void KeyHook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.Write('u');
        }

        private void KeyHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.Write('d');
        }
    }
}
