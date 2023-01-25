using System.Net;
using System.Text;
using System.Xml;
using System.Timers;

namespace AnignoraCommunication.RSS
{
    public class RssReader
    {
		#region (------  Fields  ------)

        private string[] feeds;
        private int firstUpdateCounter;
        private int lastFeedRead;
        private double timerInterval;
        private readonly Timer timerRead=new Timer();
        private WebClient WClient;
        private readonly XmlDocument XmlDoc = new XmlDocument();

		#endregion (------  Fields  ------)

		#region (------  Delegates  ------)

        public delegate void OnRssFeedReadDelegate(string sUrl,RssHeader rssHeader);

		#endregion (------  Delegates  ------)

		#region (------  Events  ------)

        public event OnRssFeedReadDelegate OnRssFeedRead;

		#endregion (------  Events  ------)

		#region (------  Public Methods  ------)

        public RssHeader ReadRssFeed(string sUrl,string descriptor)
        {
            WClient=new WebClient();
            byte[] bytes = WClient.DownloadData(sUrl);
            UTF8Encoding enc=new UTF8Encoding();
            string strUtf8=enc.GetString(bytes);
            XmlDoc.LoadXml(strUtf8);
            RssHeader rhRet = new RssHeader(XmlDoc.DocumentElement,descriptor);
            WClient.Dispose();
            return rhRet;
        }

        public void StartAsyncReading(double intervalMs,params string[] feedsToRead)
        {
            feeds = feedsToRead;
            timerInterval = intervalMs;
            timerRead.Stop();
            timerRead.Interval = timerInterval/10;  //Initial read until read all once
            timerRead.Elapsed += timerRead_Elapsed;
            timerRead.Start();
        }

        public void StopAsyncReading()
        {
            timerRead.Elapsed -= timerRead_Elapsed;
            timerRead.Stop();    
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        void timerRead_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Read all feeds once faster
            if(firstUpdateCounter++>feeds.Length)
            {
                timerRead.Interval = timerInterval;
            }
            string sUrl = feeds[lastFeedRead++];
            if (lastFeedRead == feeds.Length ) lastFeedRead = 0;
            RssHeader rh = ReadRssFeed(sUrl,sUrl);
            if (OnRssFeedRead != null) OnRssFeedRead(sUrl, rh);
        }

		#endregion (------  Private Methods  ------)
    }
}
