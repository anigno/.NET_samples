using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AnignoraCommonAndHelpers
{
    public class CommandLineParser
    {
        #region Constructors

        public CommandLineParser(string commandLineString)
        {
            MatchCollection mco = Regex.Matches(commandLineString, @"(?<DATA>[^ ]*=(""[^""]*""))|(?<DATA>([^ ]*=[^ ]*))");
            foreach (Match m in mco)
            {
                string part = m.Value;
                string[] itemParts = part.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);
                commands.Add(itemParts[0].Trim().ToUpper(), itemParts[1].Trim(new[] {'\"', ' '}));
            }
        }

        public CommandLineParser()
            : this("")
        {
        }

        #endregion

        #region Public Properties

        public string this[string key]
        {
            get
            {
                if (commands.ContainsKey(key.ToUpper()))
                {
                    return commands[key.ToUpper()];
                }
                return null;
            }
        }

        #endregion

        #region Fields

        private Dictionary<string, string> commands = new Dictionary<string, string>();

        #endregion
    }
}