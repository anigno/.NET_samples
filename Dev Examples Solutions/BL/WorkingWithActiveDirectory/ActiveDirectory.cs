#region

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

#endregion

namespace WorkingWithActiveDirectory
{
    public static class ActiveDirectoryUtils
    {
        #region Public Methods

        /// <summary>
        /// Gets a list of groups for a given user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNestedGroupMembershipsByUsername(string username)
        {
            try
            {
                List<string> nestedGroups = new List<string>();
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), username);
                if (userPrincipal != null)
                    foreach (Principal group in userPrincipal.GetGroups())
                    {
                        nestedGroups.Add(@group.SamAccountName);
                    }
                return nestedGroups;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                return null;
            }
        }


        /// <summary>
        /// Gets a list of users for given domain name
        /// </summary>
        /// <param name="p_domainName"></param>
        /// <param name="maxUsers"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDomainUsers(string p_domainName, int maxUsers)
        {
            try
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, p_domainName))
                {
                    using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(principalContext)))
                    {
                        IEnumerable<Principal> principalSearchResult = searcher.FindAll().Take(maxUsers);
                        Principal[] principals = principalSearchResult.ToArray();
                        foreach (Principal principal in principals)
                        {
                            ret.Add(principal.SamAccountName, principal.DisplayName);
                        }
                        return ret;
                    }
                }
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                return null;
            }
        }

        /// <summary>
        /// Checks if a user is active or disabled
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsUserActive(string domainName, string username)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, domainName))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, username))
                {
                    if (foundUser == null) return false;
                    if (foundUser.Enabled == null) return false;
                    return foundUser.Enabled.Value;
                }
            }
        }

        /// <summary>
        /// Gets all users in a given group and domain
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="groupName"></param>
        /// <param name="maxUsers">Takes the first p_maxUsers users from the domain to prevent too many users when testing with real domain</param>
        /// <returns></returns>
        public static IEnumerable<string> GetUsersInGroup(string domainName, string groupName, int maxUsers)
        {
            try
            {
                List<string> ret = new List<string>();
                Dictionary<string, string> users = GetDomainUsers(domainName, maxUsers);
                foreach (KeyValuePair<string, string> user in users)
                {
                    IEnumerable<string> groups = GetNestedGroupMembershipsByUsername(user.Key) ?? new String[0];
                    if (groups.Contains(groupName))
                        if (IsUserActive(domainName, user.Key))
                            ret.Add(user.Value);
                }
                return ret;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                return null;
            }
        }

        #endregion

        #region Public Properties

        public static string LastError { get; private set; }

        #endregion
    }
}