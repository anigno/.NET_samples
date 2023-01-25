using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using AnignoraCommonAndHelpers.Helpers;
using AnignoraCommunication.Agents;
using AnignoraCommunication.Agents.Icd;
using AnignoraCommunication.Communicators;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.CommunicationTests
{
    [TestFixture]
    public class TestCommunication
    {
        int m_messagesCounter;
        [Test]
        public void MsmQueueCommunicatorTests()
        {
            Interlocked.Exchange(ref m_messagesCounter, 0);

            string hostName = Dns.GetHostName();
            MsmqCommunicator msmqCommunicator1 = new MsmqCommunicator(1, 2, hostName);
            MsmqCommunicator msmqCommunicator2 = new MsmqCommunicator(2, 1, hostName);
            msmqCommunicator1.OnMessageReceived += msmqCommunicator1_OnMessageReceived;
            msmqCommunicator2.OnMessageReceived += msmqCommunicator2_OnMessageReceived;

            for (int a = 0; a < 5; a++)
            {
                MsmqCommunicatorMessage msmqCommunicatorMessage = new MsmqCommunicatorMessage();
                msmqCommunicator1.SendMessage(msmqCommunicatorMessage);
            } 
            for (int a = 0; a < 5; a++)
            {
                MsmqCommunicatorMessage msmqCommunicatorMessage = new MsmqCommunicatorMessage();
                msmqCommunicator2.SendMessage(msmqCommunicatorMessage);
            }
            Thread.Sleep(1000);
            //Assert received 10 messages
            Assert.AreEqual(10,m_messagesCounter);
        }

        void msmqCommunicator2_OnMessageReceived(MsmqCommunicatorMessage p_msmqCommunicatorMessage)
        {
            Debug.WriteLine("Received {0} SenderQ:{1} M:{2}", p_msmqCommunicatorMessage.SenderMachineName, p_msmqCommunicatorMessage.SenderIndex, p_msmqCommunicatorMessage.Id);
            Interlocked.Add(ref m_messagesCounter, 1);
        }

        void msmqCommunicator1_OnMessageReceived(MsmqCommunicatorMessage p_msmqCommunicatorMessage)
        {
            TestsHelper.DebugWrite("Received {0} SenderQ:{1} M:{2}", p_msmqCommunicatorMessage.SenderMachineName, p_msmqCommunicatorMessage.SenderIndex, p_msmqCommunicatorMessage.Id);
            Interlocked.Add(ref m_messagesCounter, 1);
        }

        [Test]
        public void MsmqAgentTest()
        {
            Interlocked.Exchange(ref m_messagesCounter,0);

            string hostName=Dns.GetHostName();
            MsmqAgent agentA=new MsmqAgent(100,100,hostName);
            agentA.OnRequestTimeout += agentA_OnRequestTimeout;

            agentA.RegisterCommandHandler<TestCommandMessage, TestCommandHandler>();
            agentA.SendCommand(new TestCommandMessage() { CommandString = "This is a command string" });
            Thread.Sleep(500);
            //Assert empty requests list
            Assert.AreEqual(0, agentA.ActiveRequestsIds.Length);

            agentA.RegisterRequestResponseHandler<RequestTestMessage, RequestTestHandler, ResponseTestMessage, ResponseTestHandler>();
            agentA.SendRequest(new RequestTestMessage(TimeSpan.FromSeconds(10)) { RequestString = "This is a request " });
            agentA.SendRequest(new RequestTestMessage(TimeSpan.FromSeconds(1)) { RequestString = "This is a request " });
            agentA.SendRequest(new RequestTestMessage(TimeSpan.FromSeconds(10)) { RequestString = "This is a request " });
            agentA.SendRequest(new RequestTestMessage(TimeSpan.FromSeconds(1)) { RequestString = "This is a request " });
            Thread.Sleep(18000);
            ////Assert 2 timeouts
            Assert.AreEqual(2, m_messagesCounter);
            //Assert empty requests list
            Assert.AreEqual(0, agentA.ActiveRequestsIds.Length, StringHelper.PrintArray(agentA.ActiveRequestsIds));

        }

        void agentA_OnRequestTimeout(IcdRequest p_msmqCommunicatorMessage)
        {
            TestsHelper.DebugWrite("Request timeout "+p_msmqCommunicatorMessage.Id);
            Interlocked.Add(ref m_messagesCounter, 1);

        }


    }
}
