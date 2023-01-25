using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace AnignoraCommunication.NetworkDiscovery
{
    public sealed class NetworkBrowser
    {
		#region (------  Public static Methods  ------)
        //declare the Netapi32 : NetApiBufferFree method import
        [DllImport("Netapi32", SetLastError = true),
        SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetApiBufferFree(IntPtr p_pBuf);

        //declare the Netapi32 : NetServerEnum method import
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true),
        SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetServerEnum(
            string p_serverNane, // must be null
            int p_dwLevel,
            ref IntPtr p_pBuf,
            int p_dwPrefMaxLen,
            out int p_dwEntriesRead,
            out int p_dwTotalEntries,
            int p_dwServerType,
            string p_domain, // null for login domain
            out int p_dwResumeHandle
            );
		#endregion (------  Public static Methods  ------)

		#region (------  Public Methods  ------)
        /// <summary>
        /// Uses the DllImport : NetServerEnum with all its required parameters
        /// (see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
        /// for full details or method signature) to retrieve a list of domain SV_TYPE_WORKSTATION
        /// and SV_TYPE_SERVER PC's
        /// </summary>
        /// <returns>Arraylist that represents all the SV_TYPE_WORKSTATION and SV_TYPE_SERVER
        /// PC's in the Domain</returns>
        public string[] GetNetworkComputers()
        {
            //local fields
            List<string> networkComputers = new List<string>();
            const int MAX_PREFERRED_LENGTH = -1;
            const int SV_TYPE_WORKSTATION = 1;
            const int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            int sizeofINFO = Marshal.SizeOf(typeof(ServerInfo100));


            try
            {
                //call the DllImport : NetServerEnum with all its required parameters
                //see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
                //for full details of method signature
                int entriesRead;
                int resHandle;
                int totalEntries;
                int ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH,
                    out entriesRead,
                    out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out 
					resHandle);
                //if the returned with a NERR_Success (C++ term), =0 for C#
                if (ret == 0)
                {
                    //loop through all SV_TYPE_WORKSTATION and SV_TYPE_SERVER PC's
                    for (int i = 0; i < totalEntries; i++)
                    {
                        //get pointer to, Pointer to the buffer that received the data from
                        //the call to NetServerEnum. Must ensure to use correct size of 
                        //STRUCTURE to ensure correct location in memory is pointed to
                        IntPtr tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
                        //Have now got a pointer to the list of SV_TYPE_WORKSTATION and 
                        //SV_TYPE_SERVER PC's, which is unmanaged memory
                        //Needs to Marshal data from an unmanaged block of memory to a 
                        //managed object, again using STRUCTURE to ensure the correct data
                        //is marshalled 
                        ServerInfo100 svrInfo = (ServerInfo100)
                            Marshal.PtrToStructure(tmpBuffer, typeof(ServerInfo100));

                        //add the PC names to the ArrayList
                        networkComputers.Add(svrInfo.sv100_name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with acessing network computers in NetworkBrowser " +
                    "\r\n\r\n\r\n" + ex.Message,
                    "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                //The NetApiBufferFree function frees 
                //the memory that the NetApiBufferAllocate function allocates
                NetApiBufferFree(buffer);
            }
            //return entries found
            return networkComputers.ToArray();

        }
		#endregion (------  Public Methods  ------)
    }
}
