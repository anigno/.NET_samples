using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>Share information, NT, level 1</summary>
    /// <remarks>
    /// Fallback when no admin rights.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ShareINFO1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetName;
        public ShareType ShareType;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
    }
}
