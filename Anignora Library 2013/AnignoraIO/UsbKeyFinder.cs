using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnignoraDataTypes;

namespace AnignoraIO
{
    public static class UsbKeyFinder
    {
        #region Public Methods

        public static string[] GetKey()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                IEnumerable<DriveInfo> removables = drives.Where(p_info => p_info.DriveType == DriveType.Removable);
                foreach (DriveInfo driveInfo in removables)
                {
                    string path = string.Format(@"{0}_AFA_KEY\Key", driveInfo.RootDirectory);
                    if (File.Exists(path)) return parseUserAndKey(path);
                }
                return new[] {"NoUser", "0000"};
            }
            catch
            {
                return new[] {"Error", "0000"};
            }
        }

        #endregion

        #region Private Methods

        private static string[] parseUserAndKey(string p_path)
        {
            return File.ReadAllLines(p_path);
        }

        #endregion
    }
}