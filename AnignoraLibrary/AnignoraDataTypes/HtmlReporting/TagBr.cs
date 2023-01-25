using System;

namespace AnignoraDataTypes.HtmlReporting
{
    public class TagBr : Tag
    {
        #region Constructors

        public TagBr()
            : base("BR")
        {
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "<BR>" + Environment.NewLine;
        }

        #endregion
    }
}