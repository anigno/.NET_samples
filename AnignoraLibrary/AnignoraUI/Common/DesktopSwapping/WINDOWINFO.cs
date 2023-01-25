namespace AnignoraUI.Common.DesktopSwapping
{
    public struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow; //holds the coords of the window
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;
    }
}
