using System.IO;

namespace AnignoraCommunication.NetworkDiscovery
{
    public class NetworkDiscoveryService
    {
        private readonly NetworkBrowser m_networkBrowser = new NetworkBrowser();
        
        public string[] DiscoverComputers()
        {
            string[] networkComputers=m_networkBrowser.GetNetworkComputers();
            return networkComputers;
        }

        public ShareCollection GetShares(string p_computer)
        {
            ShareCollection shareCollection=ShareCollection.GetShares(p_computer);
            return shareCollection;
        }

        public DirectoryInfo[] GetShareDirectories(Share p_share)
        {
            DirectoryInfo[] directoryInfos=p_share.DirectoryInfoRoot.GetDirectories();
            return directoryInfos;
        }
    }
}
