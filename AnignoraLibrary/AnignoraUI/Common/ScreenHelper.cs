using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AnignoraUI.Common
{
    public static class ScreenHelper
    {
        public static int GetScreenIndex(Screen p_screen)
        {
            IEnumerable<string> screensSimpleNames = Screen.AllScreens.Select(GetScreenSimpleName);
            string screenSimpleName = GetScreenSimpleName(p_screen);
            for (int a = 0; a < screensSimpleNames.Count(); a++)
            {
                if (screenSimpleName == screensSimpleNames.ElementAt(a)) return a;
            }
            return -1;
        }

        public static string GetScreenSimpleName(Screen p_screen)
        {
            int a = p_screen.DeviceName.IndexOf("DISPLAY");
            string b = p_screen.DeviceName.Substring(a, 8);
            return b;
        }

        
    }
}
