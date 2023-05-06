using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlCommon;

namespace WorkingWithActiveDirectory
{
    class Program
    {
        public Program()
        {
            string domainName = ConfigurationManager.AppSettings["DomainName"];
            string groupName = ConfigurationManager.AppSettings["GroupName"];
            int maxUsers = int.Parse(ConfigurationManager.AppSettings["MaxUsers"]);

            IEnumerable<string> groupUsers = ActiveDirectoryUtils.GetUsersInGroup(domainName, groupName, maxUsers);
            TestUtils.ConsoleLog(ConsoleColor.Yellow, groupUsers.ToString(","));
        }

        private static void Main(string[] args)
        {
            new Program();
            TestUtils.WaitForEnter();
        }
    }
}
