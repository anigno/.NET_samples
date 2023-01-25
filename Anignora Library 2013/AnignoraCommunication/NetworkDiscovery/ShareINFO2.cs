using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>Share information, NT, level 2</summary>
    /// <remarks>
    /// Requires admin rights to work. 
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ShareINFO2
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetName;
        public ShareType ShareType;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        public int Permissions;
        public int MaxUsers;
        public int CurrentUsers;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
    }
}
