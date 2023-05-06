#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlCommon;
using BlCommon.Configurators;
using BlCommon.SimplePipeCommunicator;
using WorkingWithAppDomains.SimplePipesOld;

#endregion

namespace WorkingWithAppDomains
{
    internal class Program
    {
        #region Constructors

        private Program(string[] p_args)
        {
            //testPipe();
            //return;
            AppDomain appDomain = createAppDomain("FirstAppDomain");
            Type appDomainProxyType = typeof (AppDomainProxy);
            AppDomainProxy appDomainProxy = (AppDomainProxy) appDomain.CreateInstanceAndUnwrap(appDomainProxyType.Assembly.FullName, appDomainProxyType.FullName);
            SimplePipeReader_old simplePipeReaderOld = new SimplePipeReader_old("MyPipe", 100);
            simplePipeReaderOld.OnDataReceived += p_bytes => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "received " + p_bytes.Length); };
            simplePipeReaderOld.BeginWaitForData();

            appDomainProxy.RunInAppDomain("Text from Application");
            string stringFromAppDomain = appDomainProxy.GetAppDomainString();
            TestUtils.ConsoleLog(ConsoleColor.Yellow, stringFromAppDomain);
            SomeDataSerializable serializableDataFromAppDomain = appDomainProxy.GetAppDomainSerializableData();
            TestUtils.ConsoleLog(ConsoleColor.Yellow, serializableDataFromAppDomain.Data);
            SomeDataMarshaled marshaledDataFromAppDomain = appDomainProxy.GetAppDomainMarshaledData();
            TestUtils.ConsoleLog(ConsoleColor.Yellow, marshaledDataFromAppDomain.Data);
        }

        #endregion

        #region Private Methods

        private AppDomain createAppDomain(string p_name)
        {
            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = Environment.CurrentDirectory;
            Evidence adevidence = AppDomain.CurrentDomain.Evidence;
            AppDomain appDomain = AppDomain.CreateDomain(p_name, adevidence, domaininfo);
            TestUtils.ConsoleLog(ConsoleColor.Yellow, "Created AppDomain: {0}", p_name);
            return appDomain;
        }


        private void testPipe()
        {
            Task.Factory.StartNew(() =>
            {
                NamedPipeServerStream s1 = new NamedPipeServerStream("A", PipeDirection.InOut, 1);
                s1.Close();
                NamedPipeServerStream s2 = new NamedPipeServerStream("A", PipeDirection.InOut, 1);

                SimplePipeReader_old reader = new SimplePipeReader_old("MyPipe", 100);
                byte[] buffer;
                buffer = reader.WaitForData();
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "Received " + buffer.Length);
                buffer = reader.WaitForData();
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "Received " + buffer.Length);
                buffer = reader.WaitForData();
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "Received " + buffer.Length);
            });
            Task.Factory.StartNew(async () =>
            {
                SimplePipeWriter_old writerOld = new SimplePipeWriter_old("MyPipe");
                byte[] buffer;
                buffer = new byte[] {1, 2, 3, 4, 5, 6};
                bool b = writerOld.WriteData(buffer);
                await Task.Delay(100);
                buffer = new byte[] {1, 2, 3, 4};
                b = writerOld.WriteData(buffer);
                await Task.Delay(100);
                buffer = new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
                b = writerOld.WriteData(buffer);
                TestUtils.ConsoleLog(ConsoleColor.Green, "WriteData result=" + b);
                await Task.Delay(100);
                b = writerOld.WriteData(buffer, 3000);
                TestUtils.ConsoleLog(ConsoleColor.Green, "WriteData result=" + b);
            });
        }


       

      

        private static void Main(string[] args)
        {
            new Program(args);
            TestUtils.WaitForEnter();
        }

        #endregion
    }
}