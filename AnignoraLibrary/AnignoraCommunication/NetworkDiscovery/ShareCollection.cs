using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace AnignoraCommunication.NetworkDiscovery
{
    /// <summary>
    /// A collection of shares
    /// </summary>
    public class ShareCollection : ReadOnlyCollectionBase
    {
		#region (------  Constants  ------)
        /// <summary>Access denied</summary>
        protected const int ERROR_ACCESS_DENIED = 5;
        /// <summary>More data available</summary>
        protected const int ERROR_MORE_DATA = 234;
        /// <summary>Not connected</summary>
        protected const int ERROR_NOT_CONNECTED = 2250;
        /// <summary>Access denied</summary>
        protected const int ERROR_WRONG_LEVEL = 124;
        /// <summary>Maximum path length</summary>
        protected const int MAX_PATH = 260;
        /// <summary>Max extries (9x)</summary>
        protected const int MAX_SI50_ENTRIES = 20;
        /// <summary>No error</summary>
        protected const int NO_ERROR = 0;
        /// <summary>Level 1</summary>
        protected const int UNIVERSAL_NAME_INFO_LEVEL = 1;
		#endregion (------  Constants  ------)

		#region (------  Fields  ------)
        /// <summary>The name of the server this collection represents</summary>
        private readonly string m_server;
        /// <summary>The local shares</summary>
        private static ShareCollection s_local = null;
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
        /// <summary>
        /// Constructor
        /// </summary>
        public ShareCollection(string p_server) 
        {
            m_server = p_server;
            EnumerateShares(m_server, this);
        }

        /// <summary>
        /// Default constructor - local machine
        /// </summary>
        public ShareCollection() 
        {
            m_server = string.Empty;
            EnumerateShares(m_server, this);
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        /// <summary>
        /// Is this an NT platform?
        /// </summary>
        protected static bool IsNt
        {
            get { return (PlatformID.Win32NT == Environment.OSVersion.Platform); }
        }

        /// <summary>
        /// Returns true if this is Windows 2000 or higher
        /// </summary>
        protected static bool IsW2KUp
        {
            get 
            {
                OperatingSystem os = Environment.OSVersion;
                return (PlatformID.Win32NT == os.Platform && os.Version.Major >= 5);
            }
        }

        /// <summary>
        /// Return the local shares
        /// </summary>
        public static ShareCollection LocalShares 
        {
            get 
            {
                if (null == s_local) 
                    s_local = new ShareCollection();
				
                return s_local;
            }
        }

        /// <summary>
        /// Returns the name of the server this collection represents
        /// </summary>
        public string Server 
        {
            get { return m_server; }
        }

        /// <summary>
        /// Returns the <see cref="Share"/> at the specified index.
        /// </summary>
        public Share this[int p_index] 
        {
            get { return (Share)InnerList[p_index]; }
        }

        /// <summary>
        /// Returns the <see cref="Share"/> which matches a given local path
        /// </summary>
        /// <param name="p_path">The path to match</param>
        public Share this[string p_path] 
        {
            get 
            {
                if (string.IsNullOrEmpty(p_path)) return null;
				
                p_path = Path.GetFullPath(p_path);
                if (!IsValidFilePath(p_path)) return null;

                Share match = null;
				
                foreach (object t in InnerList)
                {
                    Share s = (Share)t;
					
                    if (s.IsFileSystem && s.MatchesPath(p_path)) 
                    {
                        //Store first match
                        if (null == match) 
                            match = s;

                            // If this has a longer path,
                            // and this is a disk share or match is a special share, 
                            // then this is a better match
                        else if (match.Path.Length < s.Path.Length) 
                        {
                            if (ShareType.Disk == s.ShareType || ShareType.Disk != match.ShareType)
                                match = s;
                        }
                    }
                }

                return match;
            }
        }
		#endregion (------  Properties  ------)

		#region (------  Public static Methods  ------)
        /// <summary>
        /// Return the shares for a specified machine
        /// </summary>
        /// <param name="p_server"></param>
        /// <returns></returns>
        public static ShareCollection GetShares(string p_server)
        {
            return new ShareCollection(p_server);
        }

        /// <summary>
        /// Returns true if fileName is a valid local file-name of the form:
        /// X:\, where X is a drive letter from A-Z
        /// </summary>
        /// <param name="p_fileName">The filename to check</param>
        /// <returns></returns>
        public static bool IsValidFilePath(string p_fileName) 
        {
            if (string.IsNullOrEmpty(p_fileName)) return false;

            char drive = char.ToUpper(p_fileName[0]);
            if ('A' > drive || drive > 'Z') 
                return false;

            if (Path.VolumeSeparatorChar != p_fileName[1]) 
                return false;
            if (Path.DirectorySeparatorChar != p_fileName[2]) 
                return false;
            return true;
        }

        /// <summary>
        /// Returns the local <see cref="Share"/> object with the best match
        /// to the specified path.
        /// </summary>
        /// <param name="p_fileName"></param>
        /// <returns></returns>
        public static Share PathToShare(string p_fileName) 
        {
            if (string.IsNullOrEmpty(p_fileName)) return null;

            p_fileName = Path.GetFullPath(p_fileName);
            if (!IsValidFilePath(p_fileName)) return null;

            ShareCollection shi = LocalShares;
            if (null == shi) return null;
            return shi[p_fileName];
        }

        /// <summary>
        /// Returns the UNC path for a mapped drive or local share.
        /// </summary>
        /// <param name="p_fileName">The path to map</param>
        /// <returns>The UNC path (if available)</returns>
        public static string PathToUnc(string p_fileName) 
        {
            if (string.IsNullOrEmpty(p_fileName)) return string.Empty;
			
            p_fileName = Path.GetFullPath(p_fileName);
            if (!IsValidFilePath(p_fileName)) return p_fileName;
			
            int nRet;
            UniversalNameINFO rni = new UniversalNameINFO();
            int bufferSize = Marshal.SizeOf(rni);

            nRet = WNetGetUniversalName(
                p_fileName, UNIVERSAL_NAME_INFO_LEVEL, 
                ref rni, ref bufferSize);
			
            if (ERROR_MORE_DATA == nRet) 
            {
                IntPtr pBuffer = Marshal.AllocHGlobal(bufferSize);
                try 
                {
                    nRet = WNetGetUniversalName(
                        p_fileName, UNIVERSAL_NAME_INFO_LEVEL, 
                        pBuffer, ref bufferSize);

                    if (NO_ERROR == nRet) 
                    {
                        rni = (UniversalNameINFO)Marshal.PtrToStructure(pBuffer,
                                                                          typeof(UniversalNameINFO));
                    }
                }
                finally 
                {
                    Marshal.FreeHGlobal(pBuffer);
                }
            }

            switch (nRet) 
            {
                case NO_ERROR:
                    return rni.lpUniversalName;

                case ERROR_NOT_CONNECTED:
                    //Local file-name
                    ShareCollection shi = LocalShares;
                    if (null != shi)
                    {
                        Share share = shi[p_fileName];
                        if (null != share)
                        {
                            string path = share.Path;
                            if (!string.IsNullOrEmpty(path))
                            {
                                int index = path.Length;
                                if (Path.DirectorySeparatorChar != path[path.Length - 1])
                                    index++;
								
                                p_fileName = (index < p_fileName.Length) ? p_fileName.Substring(index) : string.Empty;
								
                                p_fileName = Path.Combine(share.ToString(), p_fileName);
                            }
                        }
                    }
					
                    return p_fileName;

                default:
                    Console.WriteLine("Unknown return value: {0}", nRet);
                    return string.Empty;
            }
        }
		#endregion (------  Public static Methods  ------)

		#region (------  Public Methods  ------)
        /// <summary>
        /// Copy this collection to an array
        /// </summary>
        /// <param name="p_array"></param>
        /// <param name="p_index"></param>
        public void CopyTo(Share[] p_array, int p_index) 
        {
            InnerList.CopyTo(p_array, p_index);
        }
		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)
        protected void Add(Share p_share)
        {
            InnerList.Add(p_share);
        }

        protected void Add(string p_netName, string p_path, ShareType p_shareType, string p_remark)
        {
            InnerList.Add(new Share(m_server, p_netName, p_path, p_shareType, p_remark));
        }

        /// <summary>
        /// Enumerates the shares
        /// </summary>
        /// <param name="p_server">The server name</param>
        /// <param name="p_shares">The ShareCollection</param>
        protected static void EnumerateShares(string p_server, ShareCollection p_shares)
        {
            if (!string.IsNullOrEmpty(p_server) && !IsW2KUp) 
            {
                p_server = p_server.ToUpper();
				
                // On NT4, 9x and Me, server has to start with "\\"
                if (!('\\' == p_server[0] && '\\' == p_server[1])) 
                    p_server = @"\\" + p_server;
            }
		
            if (IsNt)
                EnumerateSharesNt(p_server, p_shares);
            else
                EnumerateShares9X(p_server, p_shares);
        }

        /// <summary>
        /// Enumerates the shares on Windows 9x
        /// </summary>
        /// <param name="p_server">The server name</param>
        /// <param name="p_shares">The ShareCollection</param>
        protected static void EnumerateShares9X(string p_server, ShareCollection p_shares)
        {
            int level = 50;

            Type t = typeof(ShareINFO50);
            int size = Marshal.SizeOf(t);
            ushort cbBuffer = (ushort)(MAX_SI50_ENTRIES * size);
            //On Win9x, must allocate buffer before calling API
            IntPtr pBuffer = Marshal.AllocHGlobal(cbBuffer);

            try 
            {
                ushort entriesRead;
                ushort totalEntries;
                int nRet = NetShareEnum(p_server, level, pBuffer, cbBuffer, 
                                        out entriesRead, out totalEntries);
				
                if (ERROR_WRONG_LEVEL == nRet)
                {
                    level = 1;
                    t = typeof(ShareINFO1_9X);
                    size = Marshal.SizeOf(t);
					
                    nRet = NetShareEnum(p_server, level, pBuffer, cbBuffer, 
                                        out entriesRead, out totalEntries);
                }

                if (NO_ERROR == nRet || ERROR_MORE_DATA == nRet) 
                {
                    for (int i=0, lpItem=pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=size) 
                    {
                        IntPtr pItem = new IntPtr(lpItem);
						
                        if (1 == level)
                        {
                            ShareINFO1_9X si = (ShareINFO1_9X)Marshal.PtrToStructure(pItem, t);
                            p_shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                        }
                        else
                        {
                            ShareINFO50 si = (ShareINFO50)Marshal.PtrToStructure(pItem, t);
                            p_shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                        }
                    }
                }
                else
                    Console.WriteLine(nRet);
				
            }
            finally 
            {
                //Clean up buffer
                Marshal.FreeHGlobal(pBuffer);
            }
        }

        /// <summary>
        /// Enumerates the shares on Windows NT
        /// </summary>
        /// <param name="p_server">The server name</param>
        /// <param name="p_shares">The ShareCollection</param>
        protected static void EnumerateSharesNt(string p_server, ShareCollection p_shares)
        {
            int level = 2;
            int hResume = 0;
            IntPtr pBuffer = IntPtr.Zero;

            try 
            {
                int entriesRead;
                int totalEntries;
                int nRet = NetShareEnum(p_server, level, out pBuffer, -1, 
                                        out entriesRead, out totalEntries, ref hResume);

                if (ERROR_ACCESS_DENIED == nRet) 
                {
                    //Need admin for level 2, drop to level 1
                    level = 1;
                    nRet = NetShareEnum(p_server, level, out pBuffer, -1, 
                                        out entriesRead, out totalEntries, ref hResume);
                }

                if (NO_ERROR == nRet && entriesRead > 0) 
                {
                    Type t = (2 == level) ? typeof(ShareINFO2) : typeof(ShareINFO1);
                    int offset = Marshal.SizeOf(t);

                    for (int i=0, lpItem=pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=offset) 
                    {
                        IntPtr pItem = new IntPtr(lpItem);
                        if (1 == level) 
                        {
                            ShareINFO1 si = (ShareINFO1)Marshal.PtrToStructure(pItem, t);
                            p_shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                        }
                        else 
                        {
                            ShareINFO2 si = (ShareINFO2)Marshal.PtrToStructure(pItem, t);
                            p_shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                        }
                    }
                }
				
            }
            finally 
            {
                // Clean up buffer allocated by system
                if (IntPtr.Zero != pBuffer) 
                    NetApiBufferFree(pBuffer);
            }
        }

        /// <summary>Free the buffer (NT)</summary>
        [DllImport("netapi32")]
        protected static extern int NetApiBufferFree(IntPtr p_lpBuffer);

        /// <summary>Enumerate shares (NT)</summary>
        [DllImport("netapi32", CharSet=CharSet.Unicode)]
        protected static extern int NetShareEnum (string p_lpServerName, int p_dwLevel,
                                                  out IntPtr p_lpBuffer, int p_dwPrefMaxLen, out int p_entriesRead,
                                                  out int p_totalEntries, ref int p_hResume);

        /// <summary>Get a UNC name</summary>
        [DllImport("mpr", CharSet=CharSet.Auto)]
        protected static extern int WNetGetUniversalName (string p_lpLocalPath,
                                                          int p_dwInfoLevel, ref UniversalNameINFO p_lpBuffer, ref int p_lpBufferSize);

        /// <summary>Get a UNC name</summary>
        [DllImport("mpr", CharSet=CharSet.Auto)]
        protected static extern int WNetGetUniversalName (string p_lpLocalPath,
                                                          int p_dwInfoLevel, IntPtr p_lpBuffer, ref int p_lpBufferSize);
		#endregion (------  Protected Methods  ------)

        /// <summary>Enumerate shares (9x)</summary>
        [DllImport("svrapi", CharSet=CharSet.Ansi)]
        protected static extern int NetShareEnum(
            [MarshalAs(UnmanagedType.LPTStr)] string p_lpServerName, int p_dwLevel,
            IntPtr p_lpBuffer, ushort p_cbBuffer, out ushort p_entriesRead,
            out ushort p_totalEntries);
    }
}