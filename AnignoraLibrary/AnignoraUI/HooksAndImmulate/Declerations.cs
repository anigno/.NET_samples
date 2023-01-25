using System;
using System.Runtime.InteropServices;

namespace AnignoraUI.HooksAndImmulate
{

    public struct keyboardHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }


    public struct MOUSEINPUT
    {
        public IntPtr dwExtraInfo;
        public uint dwFlags;
        public int dx;
        public int dy;
        public int mouseData;
        public uint time;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamH;
        public short wParamL;
    }



    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public MOUSEINPUT mi;
    }

   

  


}
