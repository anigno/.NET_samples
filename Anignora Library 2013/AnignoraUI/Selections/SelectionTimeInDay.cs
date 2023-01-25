using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AnignoraUI.Selections
{
    public partial class SelectionTimeInDay : UserControl
    {
        private const string CATEGORY_STRING = " SelectionTimeInDay";

        private TimeSpan m_startTime ;
        private TimeSpan m_endTime ;
        private TimeSpan m_internal =new TimeSpan(1,0,0);

        private string m_stringFormat;

        [Category(CATEGORY_STRING)]
        public string StringFormat
        {
            get { return m_stringFormat; }
            set
            {
                m_stringFormat = value;
                updateControls();
            }
        }


        [Category(CATEGORY_STRING)]
        public TimeSpan StartTime
        {
            get { return m_startTime; }
            set
            {
                m_startTime = value;
                updateControls();
            }
        }

        [Category(CATEGORY_STRING)]
        public TimeSpan Interval
        {
            get { return m_internal; }
            set
            {
                if (value < TimeSpan.FromSeconds(1)) return;
                m_internal = value;
                updateControls();
            }
        }

        [Category(CATEGORY_STRING)]
        public TimeSpan EndTime
        {
            get { return m_endTime; }
            set
            {
                m_endTime = value;
                updateControls();
            }
        }

        private void updateControls()
        {
            checkedListBoxIntervals.Items.Clear();
            for (TimeSpan ts = StartTime; ts <= EndTime; ts = ts + Interval)
            {
                checkedListBoxIntervals.Items.Add(ts.ToString(StringFormat));
            }
        }


        public SelectionTimeInDay()
        {
            InitializeComponent();
        }
    }
}
