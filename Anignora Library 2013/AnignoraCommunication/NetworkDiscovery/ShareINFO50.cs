using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>Share information, Win9x</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ShareINFO50
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string NetName;

        public byte bShareType;
        public ushort Flags;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Path;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string PasswordRW;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string PasswordRO;

        public ShareType ShareType
        {
            get { return (ShareType)((int)bShareType & 0x7F); }
        }
    }
}
