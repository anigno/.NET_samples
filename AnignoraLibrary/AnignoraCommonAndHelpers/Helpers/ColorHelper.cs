using System.Drawing;

namespace AnignoraCommonAndHelpers.Helpers
{
    public static class ColorHelper
    {
        #region Public Methods

        public static Color ShiftedColor(this Color p_color, int p_rgbChange)
        {
            Color newColor = Color.FromArgb((p_color.R + p_rgbChange)%256, (p_color.G + p_rgbChange)%256, (p_color.B + p_rgbChange)%256);
            return newColor;
        }

        #endregion

        #region Constants

        public const int ARGB_RED = -65536;
        public const int ARGB_BLUE = -16777056;
        public const int ARGB_GREEN = -16744448;
        public const int ARGB_WHITE = -1;
        public const int ARGB_BLACK = -16777216;
        public const int ARGB_SILVER = -4144960;
        public const int ARGB_LIGHTSKYBLUE = -7876870;

        #endregion
    }
}