using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class FilesAndFoldersHelper
    {
        #region Public Methods

        public static string[][] ReadCsvFile(string filename)
        {
            string[] splitArray = {","};
            List<string[]> lRet = new List<string[]>();
            TextReader tr = new StreamReader(filename);
            string s;
            do
            {
                s = tr.ReadLine();
                if (s != null)
                {
                    string[] parts = s.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                    lRet.Add(parts);
                }
            } while (s != null);
            return lRet.ToArray();
        }

        /// <summary>
        /// Replaces the Directory.GetFiles(path,pattern), allow multiple patterns for given directory (No recursive run)
        /// </summary>
        public static string[] DirectoryGetFiles(string path, params string[] patterns)
        {
            List<string> FilesReturn = new List<string>();
            try
            {
                foreach (string pattern in patterns)
                {
                    string[] files = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
                    FilesReturn.AddRange(files);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //System folders exception, Do nothing
            }
            return FilesReturn.ToArray();
        }

        /// <summary>
        /// Replace the Path.GetDirectoryName(), can get value of longer then 255 chars path
        /// </summary>
        public static string GetDirectoryNameEx(string filePath)
        {
            int lastDirIndex = filePath.LastIndexOf(DIRECTORY_SEPERATOR);
            //If path contains no directory seperator, return empty string
            if (lastDirIndex < 0) return "";
            return filePath.Substring(0, lastDirIndex);
        }

        /// <summary>
        /// Replace the Path.GetExtension(), can get value of longer then 255 chars path
        /// </summary>
        public static string GetExtensionNameEx(string filePath)
        {
            filePath = GetFileNameEx(filePath);
            int lastSeperatorIndex = filePath.LastIndexOf(EXTENSION_SEPERATOR);
            //If path contains no extension seperator, return filePath
            if (lastSeperatorIndex < 0) return "";
            return filePath.Substring(lastSeperatorIndex + 1);
        }

        /// <summary>
        /// Replace the Path.GetFileName(), can get value of longer then 255 chars path
        /// </summary>
        public static string GetFileNameEx(string filePath)
        {
            int lastDirIndex = filePath.LastIndexOf(DIRECTORY_SEPERATOR);
            //If path contains no directory seperator, return filePath
            if (lastDirIndex < 0) return filePath;
            return filePath.Substring(lastDirIndex + 1);
        }

        /// <summary>
        /// Replace the Path.GetFileNameWithoutExtension(), can get value of longer then 255 chars path
        /// </summary>
        public static string GetFileNameWithoutExtensionEx(string filePath)
        {
            filePath = GetFileNameEx(filePath);
            int lastSeperatorIndex = filePath.LastIndexOf(EXTENSION_SEPERATOR);
            //If no extention return filePath
            if (lastSeperatorIndex < 0) return filePath;
            return filePath.Substring(0, lastSeperatorIndex);
        }

        /// <summary>
        /// Rar a directory, Recurse subdirectories, with password
        /// </summary>
        /// <param name="winRarPath">Path to WinRar.exe file</param>
        /// <param name="destFile">Destination path and file name without .RAR extension</param>
        /// <param name="sourceDir">Source directory to archive</param>
        /// <param name="password">Password to protect the archive, if left empty no password protection is added</param>
        /// <returns>true if archiving succedded, else return false</returns>
        public static bool RarDirectory(string winRarPath, string destFile, string sourceDir, string password)
        {
            //Usage: RarDirectory(@"C:\Program Files\WinRAR\winrar.exe", @"c:\test", @"e:\_temp\2", "1234");
            string tempFilename = DateTime.Now.Ticks.ToString();
            string tempDestFilePath = Path.GetDirectoryName(destFile) + "\\" + tempFilename;
            //a -p1234 -r c:\test e:\_temp\2
            string passwordString = "-p" + password;
            if (password == "") passwordString = "";
            string commandline = " a " + passwordString + " -r " + tempDestFilePath + " " + sourceDir;
            Process p = new Process();
            p.StartInfo.FileName = winRarPath;
            p.StartInfo.Arguments = commandline;
            p.Start();
            p.WaitForExit();
            if (File.Exists(tempDestFilePath + ".rar"))
            {
                File.Delete(destFile + ".rar");
                File.Move(tempDestFilePath + ".rar", destFile + ".rar");
                return true;
            }
            return false;
        }

        #endregion

        #region Constants

        public const string DIRECTORY_SEPERATOR = @"\";
        public const string EXTENSION_SEPERATOR = @".";

        #endregion
    }
}