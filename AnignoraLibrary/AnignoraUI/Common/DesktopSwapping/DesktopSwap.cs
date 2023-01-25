using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnignoraUI.Common.DesktopSwapping
{
    public class DesktopSwap
    {
        #region (--- Public Static Methods ---)

        public static void SwapDesktops(string p_applicationProcessNameFirst, string p_applicationProcessNameSecond)
        {
            Screen[] screens = Screen.AllScreens;
            Rectangle boundsFirst = screens[0].Bounds;
            Rectangle boundsSecond = screens[0].Bounds;
            //Verify two screens
            if (Screen.AllScreens.Length > 1)
            {
                boundsSecond = screens[1].Bounds;
                //verify screens order, left shoyld be smaller values if (boundsFirst.Left > boundsSecond.Left)
                {
                    Rectangle temp = new Rectangle(boundsFirst.Left, boundsFirst.Top, boundsFirst.Width, boundsFirst.Height);
                    boundsFirst = new Rectangle(boundsSecond.Left, boundsSecond.Top, boundsSecond.Width, boundsSecond.Height);
                    boundsSecond = temp;
                }
            }

            //Iterate processes and swap requested applications 
            Process[] processes = Process.GetProcesses(".");
            foreach (Process process in processes)
            {
                IntPtr handle = process.MainWindowHandle;
                if (process.MainWindowTitle == "") continue;
                string processName = process.ProcessName.ToLower();
                if (processName == p_applicationProcessNameFirst || processName == p_applicationProcessNameSecond)
                {
                    WINDOWINFO winfo = new WINDOWINFO();
                    winfo.cbSize = (uint) Marshal.SizeOf(winfo);
                    ExternDlls.GetWindowInfo(handle, ref winfo);
                    ExternDlls.ShowWindow(handle, ExternDlls.CMD_SHOW_NORMAL);
                    if (winfo.rcClient.Left < boundsSecond.Left)
                    {
                        ExternDlls.SetWindowPos(handle, 0, boundsSecond.Left, boundsSecond.Top, boundsSecond.Width, boundsSecond.Height, ExternDlls.SWP_NOZORDER);
                    }
                    else
                    {
                        ExternDlls.SetWindowPos(handle, 0, boundsFirst.Left, boundsFirst.Top, boundsFirst.Width, boundsFirst.Height, ExternDlls.SWP_NOZORDER);
                    }
                    ExternDlls.ShowWindow(handle, ExternDlls.CMD_SHOW_MAXIMIZE);
                }
            }
        }

        #endregion (--- Public Static Methods ---)

        #region (--- Fields ---)

        #endregion (--- Fields ---)
    }
}