using AnignoraUI.HooksAndImmulate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardMouseForm
{
    class KeyboardMouseProvider
    {
        private KeyboardHookController keyHook = new KeyboardHookController();
        private MouseHookController mouseHook = new MouseHookController();
        private Dictionary<Keys, bool> keysDown = new Dictionary<Keys, bool>();

        public KeyboardMouseProvider()
        {
            keyHook.KeyDown += KeyHook_KeyDown;
            keyHook.KeyUp += KeyHook_KeyUp;
        }

        public bool IsKeyDown(Keys key)
        {
            if (!keysDown.ContainsKey(key)) return false;
            return keysDown[key];
        }

        public void MoveMouse(int dx, int dy)
        {
            Point p = mouseHook.GetMousePosition();
            mouseHook.SetMousePosition(p.X+dx,p.Y+dy);
        }

        public void MouseButton(MouseEventFlags mouseFlag)
        {
            mouseHook.EmulateMouseButton(mouseFlag);
        }

        public bool GetKetState(int keyCode)
        {
            return KeyboardHookController.GetKeyState(keyCode)!=0;
        }

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;


        private void KeyHook_KeyUp(object sender, KeyEventArgs e)
        {
            if (!keysDown.ContainsKey(e.KeyCode)) keysDown.Add(e.KeyCode, false);
            keysDown[e.KeyCode] = false;
            KeyUp(this, e);
        }

        private void KeyHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keysDown.ContainsKey(e.KeyCode)) keysDown.Add(e.KeyCode, true);
            keysDown[e.KeyCode] = true;
            KeyDown(this, e);
        }

    }
}
