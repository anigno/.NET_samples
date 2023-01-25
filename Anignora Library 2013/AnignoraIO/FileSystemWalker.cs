using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace AnignoraIO
{
    public class FileSystemWalker
    {
        #region Public Methods

        public static bool IsPatternContains(string filepath, string sPattern)
        {
            string[] parts = sPattern.Split(patternSplitter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                string rx = WildcardToRegex(part);
                if (Regex.Match(filepath, rx, RegexOptions.IgnoreCase).Success) return true;
            }
            return false;
        }

        public static Thread WalkAsync(string rootDir, string sInclude, string sExclude)
        {
            walkThread = new Thread(WalkThreadStart);
            walkThread.Name = "FileSystemWalker: " + rootDir;
            string[] sParams = new string[3];
            sParams[0] = rootDir;
            sParams[1] = sInclude;
            sParams[2] = sExclude;
            walkThread.Start(sParams);
            return walkThread;
        }

        public static void WalkSync(string rootDir, string sInclude, string sExclude)
        {
            if (OnStart != null) OnStart(rootDir);
            WalkRecurse(rootDir, sInclude, sExclude);
            if (OnEnd != null) OnEnd(rootDir);
        }

        public static string WildcardToRegex(string pattern)
        {
            string s = "^" + Regex.Escape(pattern);
            s = s.Replace(@"\*", ".*");
            s = s.Replace(@"\?", ".") + "$";
            return s;
        }

        #endregion

        #region Events

        public static event FileSystemWalkerDelegate OnDirectoryEnter;

        public static event FileSystemWalkerDelegate OnDirectoryExit;

        public static event FileSystemWalkerDelegate OnEnd;

        public static event FileSystemWalkerDelegate OnFileFound;

        public static event FileSystemWalkerDelegate OnStart;

        #endregion

        #region Private Methods

        private static void WalkRecurse(string rootDir, string sInclude, string sExclude)
        {
            if (OnDirectoryEnter != null) OnDirectoryEnter(rootDir);
            try
            {
                if (!Directory.Exists(rootDir)) return;
                foreach (string file in Directory.GetFiles(rootDir))
                {
                    if (IsPatternContains(file, sInclude) && !IsPatternContains(file, sExclude))
                    {
                        if (OnFileFound != null) OnFileFound(file);
                    }
                }
                foreach (string childDir in Directory.GetDirectories(rootDir))
                {
                    WalkRecurse(childDir, sInclude, sExclude);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //System folders
            }
            if (OnDirectoryExit != null) OnDirectoryExit(rootDir);
        }

        private static void WalkThreadStart(object o)
        {
            string[] sParams = (string[]) o;
            string rootDir = sParams[0];
            string sInclude = sParams[1];
            string sExclude = sParams[2];
            if (OnStart != null) OnStart(rootDir);
            WalkRecurse(rootDir, sInclude, sExclude);
            if (OnEnd != null) OnEnd(rootDir);
        }

        #endregion

        #region Fields

        private static readonly string[] patternSplitter = new[] {";"};
        private static Thread walkThread;

        #endregion

        public delegate void FileSystemWalkerDelegate(string path);
    }
}