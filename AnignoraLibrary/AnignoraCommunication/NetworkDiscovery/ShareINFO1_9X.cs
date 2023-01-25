using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>Share information level 1, Win9x</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ShareINFO1_9X
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string NetName;
        public byte Padding;

        public ushort bShareType;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;

        public ShareType ShareType
        {
            get { return (ShareType)((int)bShareType & 0x7FFF); }
        }
    }

}
