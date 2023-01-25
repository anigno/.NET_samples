using System.Drawing;
using System.ComponentModel;

namespace AnignoraUI.Labels
{
    public abstract class LabelGradientBase : LabelRoundedCorners
    {
        private const string CATEGORY_STRING = " LabelGradientBase";
        private Color m_gradientColorFirst = Color.Black;
        private Color m_gradientColorSecond = Color.White;

        [Category(CATEGORY_STRING)]
        public Color GradientColorFirst
        {
            get { return m_gradientColorFirst; }
            set
            {
                m_gradientColorFirst = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color GradientColorSecond
        {
            get { return m_gradientColorSecond; }
            set
            {
                m_gradientColorSecond = value;
                Refresh();
            }
        }
    }
}
