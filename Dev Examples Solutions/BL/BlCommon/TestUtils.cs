#region

using System;
using System.Diagnostics;

#endregion

namespace BlCommon
{
    public class TestUtils
    {
        #region Public Methods

        public static void ConsoleLog(ConsoleColor p_color, string p_patern, params object[] p_objects)
        {
            lock (SyncRoot)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = p_color;
                ConsoleLog(p_patern, p_objects);
                Console.ForegroundColor = color;
            }
        }

        public static void ConsoleLog(Exception p_ex, string p_text = "")
        {
            ConsoleLog(ConsoleColor.Red, "{0} [{1}] [{2}]", p_text, p_ex.Message, p_ex);
        }

        public static void ConsoleLog(string p_patern, params object[] p_objects)
        {
            //p_patern = p_patern.Replace("{", "[{");
            //p_patern = p_patern.Replace("}", "}]");
            string text = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss.fff"), string.Format(p_patern, p_objects));
            Console.WriteLine(text);
            //Debug.WriteLine(text);
        }

        public static void DebugLog(string p_patern, params object[] p_objects)
        {
            Debug.WriteLine("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss.fff"), string.Format(p_patern, p_objects));
            //ConsoleLog("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss.fff"), string.Format(p_patern, p_objects));
        }

        public static string WaitForEnter()
        {
            return WaitForEnter("Press Enter to continue");
        }

        public static string WaitForEnter(string p_message)
        {
            ConsoleLog(ConsoleColor.Gray, p_message);
            string s = Console.ReadLine();
            return s;
        }

        #endregion

        #region Fields

        public static object SyncRoot = new object();

        #endregion
    }
}