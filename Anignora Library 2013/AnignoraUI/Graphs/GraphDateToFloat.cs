using System;

namespace AnignoraUI.Graphs
{
    public class GraphDateToFloat : GraphLongToLong
    {
        public void AddPointDateFloat(DateTime date,float f)
        {
            AddPointLongLong(date.Date.ToFileTimeUtc(), (long) (f*10));
        }

        protected override string GetLabelStringX(long l)
        {
            return DateTime.FromFileTimeUtc(l).ToShortDateString();
        }

        protected override string GetLabelStringY(long l)
        {
            return string.Format("{0}.{1}", l/10, Math.Abs(l%10));
        }
    }
}
