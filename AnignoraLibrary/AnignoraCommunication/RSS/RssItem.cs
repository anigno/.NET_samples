using System;
using System.Xml;

namespace AnignoraCommunication.RSS
{
    public class RssItem
    {
		#region (------  Constructors  ------)

        public RssItem(XmlNode xmlNode)
        {
            Title = xmlNode.SelectSingleNode("title").InnerText;
            Link = xmlNode.SelectSingleNode("link").InnerText;
            Guid = xmlNode.SelectSingleNode("guid").InnerText;
            IsPermaLink = xmlNode.SelectSingleNode("guid").Attributes["isPermaLink"].Value.ToUpper() == "TRUE";
            string sDateTime= xmlNode.SelectSingleNode("pubDate").InnerText;
            DateTime dateTimeReadTemp;
            bool canParseDate = DateTime.TryParse(sDateTime, out dateTimeReadTemp);
            PubDate = canParseDate ? dateTimeReadTemp : DateTime.Now;
            Description = xmlNode.SelectSingleNode("description").InnerText;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public string Description { get; set; }

        public string Guid { get; set; }

        public bool IsPermaLink { get; set; }

        public string Link { get; set; }

        public DateTime PubDate { get; set; }

        public string Title { get; set; }

		#endregion (------  Properties  ------)
    }
}
