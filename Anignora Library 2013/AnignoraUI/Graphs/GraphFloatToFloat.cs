using System;

namespace AnignoraUI.Graphs
{
    public class GraphFloatToFloat : GraphLongToLong
    {
        public void AddPointFloatFloat(float x,float y)
        {
            AddPointLongLong((long)(x*100),(long)(y*100));
        }

        protected override string GetLabelStringX(long l)
        {
            return string.Format("{0}.{1}", l / 100, Math.Abs(l % 100));
        }

        protected override string GetLabelStringY(long l)
        {
            return string.Format("{0}.{1}", l / 100, Math.Abs(l % 100));
        }
    }
}
