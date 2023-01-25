using System.Runtime.InteropServices;
namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>Unc name</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct UniversalNameINFO
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpUniversalName;
    }
}
