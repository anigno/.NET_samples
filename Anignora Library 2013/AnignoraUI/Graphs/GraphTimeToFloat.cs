using System;

namespace AnignoraUI.Graphs
{
    public class GraphTimeToFloat : GraphDateToFloat
    {
        public void AddPointTimeFloat(DateTime time,float f)
        {
            AddPointLongLong(time.ToFileTime(), (long)(f * 10));
        }

        protected override string GetLabelStringX(long l)
        {
            DateTime dt = DateTime.FromFileTime(l);
            return string.Format("{0:00}:{1:00}", dt.Hour, dt.Minute);
        }
    }
}
