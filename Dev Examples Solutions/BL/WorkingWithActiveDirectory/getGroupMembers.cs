using System;
using System.Collections;
using System.DirectoryServices;

namespace WorkingWithActiveDirectory
{
    public class GetGroupMembers
    {
        /// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
        /// searchedGroups will contain all groups already searched, in order to
        /// prevent endless loops when there are circular structures in the groups.
        /// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
        static Hashtable searchedGroups = null;

        /// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
        /// x will return all users in the group passed in as a parameter
        /// the names returned are the SAM Account Name of the users.
        /// The function will recursively search all nested groups.
        /// Remark: if there are multiple groups with the same name, 
        /// this function will just
        /// use the first one it finds.
        /// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;param name="strGroupName">Name of the group, &lt;/span>
        /// which the users should be retrieved from&lt;span class="code-SummaryComment">&lt;/param>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;returns>ArrayList containing the SAM Account Names &lt;/span>
        /// of all users in this group and any nested groups&lt;span class="code-SummaryComment">&lt;/returns>&lt;/span>
        static public ArrayList x(string strGroupName)
        {
            ArrayList groupMembers = new ArrayList();
            searchedGroups = new Hashtable();

            // find group
            DirectorySearcher search = new DirectorySearcher("LDAP://DC=company,DC=com");
            search.Filter = String.Format
                ("(&(objectCategory=group)(cn={0}))", strGroupName);
            search.PropertiesToLoad.Add("distinguishedName");
            SearchResult sru = null;
            DirectoryEntry group;

            try
            {
                sru = search.FindOne();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            group = sru.GetDirectoryEntry();

            groupMembers = getUsersInGroup
                (group.Properties["distinguishedName"].Value.ToString());

            return groupMembers;
        }

        /// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
        /// getUsersInGroup will return all users in the group passed in as a parameter
        /// the names returned are the SAM Account Name of the users.
        /// The function will recursively search all nested groups.
        /// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;param name="strGroupDN">DN of the group, &lt;/span>
        /// which the users should be retrieved from&lt;span class="code-SummaryComment">&lt;/param>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;returns>ArrayList containing the SAM Account Names &lt;/span>
        /// of all users in this group and any nested groups&lt;span class="code-SummaryComment">&lt;/returns>&lt;/span>
        private static ArrayList getUsersInGroup(string strGroupDN)
        {
            ArrayList groupMembers = new ArrayList();
            searchedGroups.Add(strGroupDN, strGroupDN);

            // find all users in this group
            DirectorySearcher ds = new DirectorySearcher("LDAP://DC=company,DC=com");
            ds.Filter = String.Format
                ("(&(memberOf={0})(objectClass=person))", strGroupDN);

            ds.PropertiesToLoad.Add("distinguishedName");
            ds.PropertiesToLoad.Add("givenname");
            ds.PropertiesToLoad.Add("samaccountname");
            ds.PropertiesToLoad.Add("sn");

            foreach (SearchResult sr in ds.FindAll())
            {
                groupMembers.Add(sr.Properties["samaccountname"][0].ToString());
            }

            // get nested groups
            ArrayList al = getNestedGroups(strGroupDN);
            foreach (object g in al)
            {
                // only if we haven't searched this group before - avoid endless loops
                if (!searchedGroups.ContainsKey(g))
                {
                    // get members in nested group
                    ArrayList ml = getUsersInGroup(g as string);
                    // add them to result list
                    foreach (object s in ml)
                    {
                        groupMembers.Add(s as string);
                    }
                }
            }

            return groupMembers;
        }

        /// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
        /// getNestedGroups will return an array with the DNs of all groups contained
        /// in the group that was passed in as a parameter
        /// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;param name="strGroupDN">DN of the group, &lt;/span>
        /// which the nested groups should be retrieved from&lt;span class="code-SummaryComment">&lt;/param>&lt;/span>
        /// &lt;span class="code-SummaryComment">&lt;returns>ArrayList containing the DNs of each group &lt;/span>
        /// contained in the group passed in as a parameter&lt;span class="code-SummaryComment">&lt;/returns>&lt;/span>
        private static ArrayList getNestedGroups(string strGroupDN)
        {
            ArrayList groupMembers = new ArrayList();

            // find all nested groups in this group
            DirectorySearcher ds = new DirectorySearcher("LDAP://DC=company,DC=com");
            ds.Filter = String.Format
                ("(&(memberOf={0})(objectClass=group))", strGroupDN);

            ds.PropertiesToLoad.Add("distinguishedName");

            foreach (SearchResult sr in ds.FindAll())
            {
                groupMembers.Add(sr.Properties["distinguishedName"][0].ToString());
            }

            return groupMembers;
        }
    }
}