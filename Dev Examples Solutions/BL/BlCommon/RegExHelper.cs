#region

using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace BlCommon
{
    public static class RegExHelper
    {
        #region Public Methods

        public static string[] MatchesSubStrings(string p_source, string p_start, string p_end, RegexOptions p_regexOptions = RegexOptions.IgnoreCase)
        {
            List<string> resultsList = new List<string>();
            p_start = p_start.Replace("$", @"\$");
            p_end = p_end.Replace("$", @"\$");
            string pattern = string.Format("{0}(?<DATA>((?!{1}).)+)", p_start, p_end); //
            Regex regexCurrentClose = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regexCurrentClose.Matches(p_source);
            foreach (Match match in matchCollection)
            {
                var s = match.Result(@"${DATA}");
                resultsList.Add(s);
            }
            return resultsList.ToArray();
        }

        #endregion
    }
}