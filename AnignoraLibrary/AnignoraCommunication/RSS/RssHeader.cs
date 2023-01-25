using System.Collections.Generic;
using System.Xml;

namespace AnignoraCommunication.RSS
{
    public class RssHeader
    {
		#region (------  Fields  ------)

        private readonly List<RssItem> itemsList = new List<RssItem>();
        public string Descriptor { get; set; }
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public RssHeader(XmlNode xmlNode,string descriptor)
        {
            Descriptor = descriptor;
            Title = xmlNode.SelectSingleNode("channel/title").InnerText;
            Description = xmlNode.SelectSingleNode("channel/description").InnerText;
            Link = xmlNode.SelectSingleNode("channel/link").InnerText;
            XmlNodeList items = xmlNode.SelectNodes("channel/item");
            itemsList.Clear();
            if (items != null)
            {
                foreach (XmlNode item in items)
                {
                    RssItem rItem = new RssItem(item);
                    itemsList.Add(rItem);
                }
            }
            RssItems = itemsList.ToArray();
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public string Description { get; set; }

        public string Link { get; set; }

        public RssItem[] RssItems { get; set; }

        public string Title { get; set; }

		#endregion (------  Properties  ------)
    }
}