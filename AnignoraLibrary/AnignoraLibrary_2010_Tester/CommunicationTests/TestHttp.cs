using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AnignoraCommunication.Http;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.CommunicationTests
{
    [TestFixture]
    public class TestHttp
    {
        [Test]
        public void TestHttpListenerStart()
        {
            HttpAgent agent=new HttpAgent("127.0.0.1",51111,"dir1");
            agent.StartListening();
            Console.ReadLine();
        }
    }
}
