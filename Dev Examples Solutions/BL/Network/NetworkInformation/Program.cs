#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace NetworkInformation
{
    internal class Program
    {
        #region Public Methods

        public static void DisplayIpsFromAdapters()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in nics)
            {
                Console.WriteLine("\n");
                Console.WriteLine("NetworkInterface:  [{0}] [{1}]", networkInterface.Name, networkInterface.Description);
                IPInterfaceProperties adapterProperties = networkInterface.GetIPProperties();
                UnicastIPAddressInformationCollection unicastAddresses = adapterProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation addressInformation in unicastAddresses)
                {
                    Console.WriteLine("[{0}] {1}", addressInformation.Address, addressInformation.PrefixOrigin);
                }
            }
        }


        public static Dictionary<string, IPAddress> GetAdapterNameIpAddresses()
        {
            Dictionary<string, IPAddress> ipAddressesDictionary = new Dictionary<string, IPAddress>();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in nics)
            {
                IPInterfaceProperties adapterProperties = networkInterface.GetIPProperties();
                UnicastIPAddressInformationCollection unicastAddresses = adapterProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation addressInformation in unicastAddresses)
                {
                    if (addressInformation.PrefixOrigin == PrefixOrigin.Dhcp || addressInformation.PrefixOrigin == PrefixOrigin.Manual)
                    {
                        ipAddressesDictionary.Add(networkInterface.Name, addressInformation.Address);
                    }
                }
            }
            return ipAddressesDictionary;
        }

        public static void DisplayIPv4NetworkInterfaces()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            Console.WriteLine("IPv4 interface information for {0}.{1}",
                properties.HostName, properties.DomainName);
            Console.WriteLine();

            foreach (NetworkInterface adapter in nics)
            {
                // Only display informatin for interfaces that support IPv4. 
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                {
                    continue;
                }
                Console.WriteLine(adapter.Description);
                // Underline the description.
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                Console.WriteLine(">**********************************************************");
                ShowIPAddresses(adapterProperties);
                Console.WriteLine("***********************************************************");


                // Try to get the IPv4 interface properties.
                IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

                if (p == null)
                {
                    Console.WriteLine("No IPv4 information is available for this interface.");
                    Console.WriteLine();
                    continue;
                }
                // Display the IPv4 specific data.
                Console.WriteLine("  Index ............................. : {0}", p.Index);
                Console.WriteLine("  MTU ............................... : {0}", p.Mtu);
                Console.WriteLine("  APIPA active....................... : {0}",
                    p.IsAutomaticPrivateAddressingActive);
                Console.WriteLine("  APIPA enabled...................... : {0}",
                    p.IsAutomaticPrivateAddressingEnabled);
                Console.WriteLine("  Forwarding enabled................. : {0}",
                    p.IsForwardingEnabled);
                Console.WriteLine("  Uses WINS ......................... : {0}",
                    p.UsesWins);
                Console.WriteLine();
            }
        }


        public static void ShowIPAddresses(IPInterfaceProperties adapterProperties)
        {
            IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
            if (dnsServers != null)
            {
                foreach (IPAddress dns in dnsServers)
                {
                    Console.WriteLine("  DNS Servers ............................. : {0}",
                        dns.ToString()
                        );
                }
            }
            IPAddressInformationCollection anyCast = adapterProperties.AnycastAddresses;
            if (anyCast != null)
            {
                foreach (IPAddressInformation any in anyCast)
                {
                    Console.WriteLine("  Anycast Address .......................... : {0} {1} {2}",
                        any.Address,
                        any.IsTransient ? "Transient" : "",
                        any.IsDnsEligible ? "DNS Eligible" : ""
                        );
                }
                Console.WriteLine();
            }

            MulticastIPAddressInformationCollection multiCast = adapterProperties.MulticastAddresses;
            if (multiCast != null)
            {
                foreach (IPAddressInformation multi in multiCast)
                {
                    Console.WriteLine("  Multicast Address ....................... : {0} {1} {2}",
                        multi.Address,
                        multi.IsTransient ? "Transient" : "",
                        multi.IsDnsEligible ? "DNS Eligible" : ""
                        );
                }
                Console.WriteLine();
            }
            UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
            if (uniCast != null)
            {
                string lifeTimeFormat = "dddd, MMMM dd, yyyy  hh:mm:ss tt";
                foreach (UnicastIPAddressInformation uni in uniCast)
                {
                    DateTime when;

                    Console.WriteLine("  Unicast Address ......................... : {0}", uni.Address);
                    Console.WriteLine("     Prefix Origin ........................ : {0}", uni.PrefixOrigin);
                    Console.WriteLine("     Suffix Origin ........................ : {0}", uni.SuffixOrigin);
                    Console.WriteLine("     Duplicate Address Detection .......... : {0}",
                        uni.DuplicateAddressDetectionState);

                    // Format the lifetimes as Sunday, February 16, 2003 11:33:44 PM 
                    // if en-us is the current culture. 

                    // Calculate the date and time at the end of the lifetimes.    
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressValidLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Valid Life Time ...................... : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                        );
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressPreferredLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Preferred life time .................. : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                        );

                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.DhcpLeaseLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     DHCP Leased Life Time ................ : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                        );
                }
            }
        }


        public static void ShowInboundIPStatistics()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();
            Console.WriteLine("  Inbound Packet Data:");
            Console.WriteLine("      Received ............................ : {0}",
                ipstat.ReceivedPackets);
            Console.WriteLine("      Forwarded ........................... : {0}",
                ipstat.ReceivedPacketsForwarded);
            Console.WriteLine("      Delivered ........................... : {0}",
                ipstat.ReceivedPacketsDelivered);
            Console.WriteLine("      Discarded ........................... : {0}",
                ipstat.ReceivedPacketsDiscarded);
        }


        public static void DisplayDnsAddresses()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress dns in dnsServers)
                    {
                        Console.WriteLine("  DNS Servers ............................. : {0}",
                            dns.ToString());
                    }
                    Console.WriteLine();
                }
            }
        }


        public static void ShowIcmpV4Statistics()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IcmpV4Statistics stat = properties.GetIcmpV4Statistics();
            Console.WriteLine("ICMP V4 Statistics:");

            Console.WriteLine("  Messages ............................ Sent: {0,-10}   Received: {1,-10}",
                stat.MessagesSent, stat.MessagesReceived);
            Console.WriteLine("  Errors .............................. Sent: {0,-10}   Received: {1,-10}",
                stat.ErrorsSent, stat.ErrorsReceived);

            Console.WriteLine("  Echo Requests ....................... Sent: {0,-10}   Received: {1,-10}",
                stat.EchoRequestsSent, stat.EchoRequestsReceived);
            Console.WriteLine("  Echo Replies ........................ Sent: {0,-10}   Received: {1,-10}",
                stat.EchoRepliesSent, stat.EchoRepliesReceived);

            Console.WriteLine("  Destination Unreachables ............ Sent: {0,-10}   Received: {1,-10}",
                stat.DestinationUnreachableMessagesSent, stat.DestinationUnreachableMessagesReceived);

            Console.WriteLine("  Source Quenches ..................... Sent: {0,-10}   Received: {1,-10}",
                stat.SourceQuenchesSent, stat.SourceQuenchesReceived);

            Console.WriteLine("  Redirects ........................... Sent: {0,-10}   Received: {1,-10}",
                stat.RedirectsSent, stat.RedirectsReceived);

            Console.WriteLine("  TimeExceeded ........................ Sent: {0,-10}   Received: {1,-10}",
                stat.TimeExceededMessagesSent, stat.TimeExceededMessagesReceived);

            Console.WriteLine("  Parameter Problems .................. Sent: {0,-10}   Received: {1,-10}",
                stat.ParameterProblemsSent, stat.ParameterProblemsReceived);

            Console.WriteLine("  Timestamp Requests .................. Sent: {0,-10}   Received: {1,-10}",
                stat.TimestampRequestsSent, stat.TimestampRequestsReceived);
            Console.WriteLine("  Timestamp Replies ................... Sent: {0,-10}   Received: {1,-10}",
                stat.TimestampRepliesSent, stat.TimestampRepliesReceived);

            Console.WriteLine("  Address Mask Requests ............... Sent: {0,-10}   Received: {1,-10}",
                stat.AddressMaskRequestsSent, stat.AddressMaskRequestsReceived);
            Console.WriteLine("  Address Mask Replies ................ Sent: {0,-10}   Received: {1,-10}",
                stat.AddressMaskRepliesSent, stat.AddressMaskRepliesReceived);
            Console.WriteLine("");
        }


        public static void ShowNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}",
                    adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                // Create a display string for the supported IP versions. 
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                ShowIPAddresses(properties);

                // The following information is not useful for loopback adapters. 
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);

                string label;
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {
                        IPAddressCollection winsServers = properties.WinsServersAddresses;
                        if (winsServers.Count > 0)
                        {
                            label = "  WINS Servers ............................ :";
                            //ShowIPAddresses(label, winsServers);
                        }
                    }
                }

                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
                Console.WriteLine("  Receive Only ............................ : {0}",
                    adapter.IsReceiveOnly);
                Console.WriteLine("  Multicast ............................... : {0}",
                    adapter.SupportsMulticast);
                //ShowInterfaceStatistics(adapter);

                Console.WriteLine();
            }
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            //ShowIcmpV4Statistics();
            //DisplayDnsAddresses();
            //ShowInboundIPStatistics();
            //DisplayIPv4NetworkInterfaces();
            //ShowNetworkInterfaces();
            //DisplayIpsFromAdapters();
            Dictionary<string, IPAddress> adapterNameIpAddresses = GetAdapterNameIpAddresses();
            Console.ReadLine();
        }

        #endregion
    }
}