using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    //create a _SERVER_INFO_100 STRUCTURE
    [StructLayout(LayoutKind.Sequential)]
    public struct ServerInfo100
    {
        internal int sv100_platform_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string sv100_name;
    }
}
