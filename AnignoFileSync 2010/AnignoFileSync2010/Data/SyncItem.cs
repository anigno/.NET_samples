using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnignoFileSync.Data
{
    public class SyncItem
    {
        public string SourceDir;
        public string DestDir;
        public bool KeepHistory;
        public string Include;
        public string Exclude;

        public SyncItem()
        {
            //Include = "*.*";
            //Exclude = @"Bin\Debug";
        }
    }
}

//<SyncItem>
// <SourceDir>SourceDir</SourceDir> 
// <DestDir>DestDir</DestDir> 
// <KeepHistory>true</KeepHistory> 
//</SyncItem>
