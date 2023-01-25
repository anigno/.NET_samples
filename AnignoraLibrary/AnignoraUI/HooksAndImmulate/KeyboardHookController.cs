using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnignoraUI.HooksAndImmulate
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    public class KeyboardHookController
    {
		#region (------  Enums  ------)

        public enum KeyEventEnum : uint
        {
            KeyDown = 0x0000,
            KeyUp = 0x0002
        }

		#endregion (------  Enums  ------)

		#region (------  Const Fields  ------)

        const int INPUT_KEYBOARD = 1;
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;

		#endregion (------  Fields  ------)

		#region (------  Delegates  ------)

        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

		#endregion (------  Delegates  ------)

		#region (------  Events  ------)

        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        /// <summary>
        /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
        /// </summary>
        public KeyboardHookController()
        {
            hook();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="KeyboardHookController"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~KeyboardHookController()
        {
            unhook();
        }

		#endregion (------  Constructors  ------)

		#region (------  Static Methods  ------)

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /*
        bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        NumLock = (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
        bool ScrollLock = (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
        */
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        #endregion (------  Static Methods  ------)

        #region (------  Public Methods  ------)

        public uint EmulateKey(ushort keyCode,KeyEventEnum keyEvent)
        {
            INPUT[] inp=new INPUT[1];
            inp[0].type = INPUT_KEYBOARD;
            KEYBDINPUT keyInput = new KEYBDINPUT();
            keyInput.wVk = keyCode;
            keyInput.wScan = 0;
            keyInput.time = 0;
            keyInput.dwExtraInfo = IntPtr.Zero;
            keyInput.dwFlags = (uint)keyEvent;
            inp[0].ki = keyInput;
            return SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        /// <summary>
        /// Installs the global hook
        /// </summary>
        private void hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hInstance, 0);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        private int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                KeyEventArgs kea = new KeyEventArgs(key);
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                {
                    KeyDown(this, kea);
                }
                else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                {
                    KeyUp(this, kea);
                }
                if (kea.Handled)
                    return 1;
            }
            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        private void unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

		#endregion (------  Private Methods  ------)


        
        [StructLayout(LayoutKind.Explicit)]
        struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
    }
}