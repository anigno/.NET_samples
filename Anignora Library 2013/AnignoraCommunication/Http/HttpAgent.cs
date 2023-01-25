using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using log4net;

namespace AnignoraCommunication.Http
{
    public class HttpAgent 
    {
		#region (------  Fields  ------)
        private readonly HttpListener  m_listener=new HttpListener();
        private Thread m_listenerThread;
        private long m_listenerThreadContinueFlag;
        private readonly string m_uriPrefix;
        private static readonly ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        public event EventHandler<HttpAgentRequestReceivedEventArgs> HttpAgentRequestReceived = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public HttpAgent(string p_ipOrHost, int p_port, string p_subDir)
        {
            m_uriPrefix = string.Format("http://{0}:{1}/{2}/", p_ipOrHost, p_port, p_subDir);
            s_log.DebugFormat("{0}",m_uriPrefix);
        }
		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)
        public void Send(string p_text,int p_timeoutMs=1000)
        {
            s_log.DebugFormat("{0} {1}",p_text,p_timeoutMs);
            try
            {
                WebRequest request = WebRequest.Create(m_uriPrefix + p_text);
                request.Timeout = p_timeoutMs;
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(p_text);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                //WebResponse response = request.GetResponse();
                //dataStream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(dataStream);
                //string responseFromServer = reader.ReadToEnd();

                //reader.Close();
                //dataStream.Close();
                //response.Close();
            }
            catch (Exception ex)
            {
                s_log.Error("",ex);
                throw;
            }
        }

        public void StartListening()
        {
            s_log.DebugFormat("{0}", m_uriPrefix);
            if(m_listener.IsListening)m_listener.Stop();
            m_listener.Prefixes.Add(m_uriPrefix);
            m_listener.Start();
            m_listenerThread=new Thread(listenerThreadStart);
            m_listenerThread.IsBackground = true;
            Interlocked.Exchange(ref m_listenerThreadContinueFlag, 1);
            m_listenerThread.Start();
        }

        public void StopListening()
        {
            s_log.DebugFormat("{0}", m_uriPrefix);
            m_listener.Stop();
            Interlocked.Exchange(ref m_listenerThreadContinueFlag, 0);
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
         private void listenerThreadStart()
        {
            while (Interlocked.Read(ref m_listenerThreadContinueFlag) == 1)
            {
                s_log.DebugFormat("Listening at: {0}", m_uriPrefix);
                HttpListenerContext context = m_listener.GetContext();
                HttpListenerRequest request = context.Request;
                string rawUrl = request.RawUrl;
                IPEndPoint localEndPoint = request.LocalEndPoint;
                IPEndPoint remoteEndPoint = request.RemoteEndPoint;
                
                //Send response
                HttpListenerResponse response = context.Response;
                string responseString = string.Format("[{0}] [{1}] L[{2}] R[{3}]", DateTime.Now, rawUrl, localEndPoint, remoteEndPoint);
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                s_log.DebugFormat("Raising HttpAgentRequestReceived for: {0}", m_uriPrefix);
                HttpAgentRequestReceived(this, new HttpAgentRequestReceivedEventArgs(rawUrl, localEndPoint, remoteEndPoint));
            }
        }
		#endregion (------  Private Methods  ------)
    }
}
