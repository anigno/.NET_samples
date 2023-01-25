using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnignoraLibrary_2010_Tester
{
    public static class TestsHelper
    {
        public static void DebugWrite(string p_format,params object[] p_objects)
        {
            Debug.WriteLine("[{0}] {1}",DateTime.Now.ToString("hh:mm:ss.fff"),string.Format(p_format,p_objects));
        }
    }
}
