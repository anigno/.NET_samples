using System.Net;

namespace HttpCommunicator
{

    public class Communicator
    {
        private HttpListener listener = new HttpListener();

        public Communicator(string uriPrefix = "http://localhost:8080/")
        {
            listener.Prefixes.Add(uriPrefix);
        }

        public void Start()
        {
            listener.Start();
        }

        public void Send(string message)
        {

        }

        public event Action<string> OnMessageReceived = delegate { };

    }
}