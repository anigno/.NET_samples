namespace AnignoraDataTypes.HtmlReporting
{
    public class HtmlReporter
    {
        #region Constructors

        public HtmlReporter()
        {
            HtmlTag = new TagHtml();
        }

        #endregion

        #region Public Properties

        public TagHtml HtmlTag { get; private set; }

        public string Report
        {
            get { return HtmlTag.ToString(); }
        }

        #endregion
    }
}