#region

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using OwinDataTransferHost;

#endregion

namespace OwinDataTransferClient
{
    public class DataTransferClient
    {
        #region Constructors

        public DataTransferClient(string p_baseAddress = "http://localhost:8080")
        {
            m_baseAddress = p_baseAddress + @"/api/Data/";
        }

        #endregion

        #region Public Methods

        public WebClient CreateClient()
        {
            WebClient client = new WebClient();
            client.BaseAddress = m_baseAddress;
            return client;
        }

        public void SendDataData(DataItem p_data)
        {
            using (WebClient client = CreateClient())
            {
                //Add to query key/value collection
                client.QueryString.Add("Descriptor", p_data.Descriptor);

                //Add to headers collection
                string labelBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(p_data.Label));
                client.Headers.Add("Content-Type: text/plain");
                client.Headers.Add("X-Security-Label:" + labelBase64String);

                byte[] response = client.UploadData(client.BaseAddress, "POST", p_data.Data);
            }
        }

        #endregion

        #region Fields

        private readonly string m_baseAddress;

        #endregion
    }
}