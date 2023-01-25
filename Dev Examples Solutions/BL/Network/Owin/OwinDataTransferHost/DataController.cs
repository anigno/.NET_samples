#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BlCommon;
using OwinHost;

#endregion

namespace OwinDataTransferHost
{
    
    public class DataController : ApiController
    {
        #region Public Methods

        public IHttpActionResult Post()
        {
            Task<byte[]> t = this.Request.Content.ReadAsByteArrayAsync();
            t.Wait();

            HttpRequestMessage httprequest = this.Request;
            string descriptor = httprequest.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "Descriptor").Value;

            string head = httprequest.Headers.GetValues("X-Security-Label").FirstOrDefault();
            string label = Encoding.UTF8.GetString(Convert.FromBase64String(head));

            byte[] bytes = t.Result;
            Console.WriteLine("Received [{0} {1} {2}]", descriptor, label, bytes.Length);
            return Ok();
        }

        #endregion
    }
}