using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnignoraUI.Common
{
    struct ScrollInfoStruct
    {
        public int cbSize;
        public int fMask;
        public int nMin;
        public int nMax;
        public int nPage;
        public int nPos;
        public int nTrackPos;
    }

    /// <summary>
    /// Enable scrolling abbilities to control, use WndProc override in control to ask if control was scrolled or get scroll position
    /// </summary>
    public static class ScrollHelper
    {

        #region Const Fields (10) 

        private const int SB_ENDSCROLL = 8;
        private const int SB_HSCROLL = 0;
        private const int SB_VSCROLL = 1;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
        private const int SIF_PAGE = 0x2;
        private const int SIF_POS = 0x4;
        private const int SIF_RANGE = 0x1;
        private const int SIF_TRACKPOS = 0x10;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        #endregion Const Fields 

        #region Static Methods (5) 

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

        public static int GetScrollPosition(IntPtr controlHandle, ScrollOrientation orientation)
        {
            if (orientation == ScrollOrientation.HorizontalScroll)
            {
                ScrollInfoStruct si = new ScrollInfoStruct();
                si.fMask = SIF_ALL;
                si.cbSize = Marshal.SizeOf(si);
                GetScrollInfo(controlHandle, 0, ref si);
                return si.nPos;
            }
            else
            {
                ScrollInfoStruct si = new ScrollInfoStruct();
                si.fMask = SIF_ALL;
                si.cbSize = Marshal.SizeOf(si);
                GetScrollInfo(controlHandle, 1, ref si);
                return si.nPos;
            }
        }

        /// <summary>
        /// Returns ScrollEventArgs if WndProc message is a scrolling message, else return null
        /// </summary>
        public static ScrollEventArgs IsScrolled(ref Message msg,ScrollOrientation orientation)
        {
            if (orientation == ScrollOrientation.HorizontalScroll) return IsScrolledHorizontal(ref msg);
            return IsScrolledVertical(ref msg);
        }

        public static ScrollEventArgs IsScrolledHorizontal(ref Message msg)
        {
            if (msg.Msg == WM_HSCROLL)
            {
                ScrollInfoStruct si = new ScrollInfoStruct();
                si.fMask = SIF_ALL;
                si.cbSize = Marshal.SizeOf(si);
                GetScrollInfo(msg.HWnd, SB_HSCROLL, ref si);

                if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                {
                    ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
                    return sargs;
                }
            }
            return null;
        }

        public static ScrollEventArgs IsScrolledVertical(ref Message msg)
        {
            if (msg.Msg == WM_VSCROLL || msg.Msg==15)
            {
                ScrollInfoStruct si = new ScrollInfoStruct();
                si.fMask = SIF_ALL;
                si.cbSize = Marshal.SizeOf(si);
                GetScrollInfo(msg.HWnd, SB_VSCROLL, ref si);

                //if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                {
                    ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
                    return sargs;
                }
            }
            return null;
        }

        #endregion Static Methods 

    }
}