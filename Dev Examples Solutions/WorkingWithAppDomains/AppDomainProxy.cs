using System;
using System.Threading.Tasks;
using BlCommon;
using WorkingWithAppDomains.SimplePipesOld;

namespace WorkingWithAppDomains
{
    public class AppDomainProxy : MarshalByRefObject
    {

        public void RunInAppDomain(string p_someTest)
        {
            Task.Factory.StartNew(async () =>
            {
                TestUtils.ConsoleLog(ConsoleColor.Green, p_someTest);
                await Task.Delay(250);
                SimplePipeWriter_old simplePipeWriterOld = new SimplePipeWriter_old("MyPipe");
                simplePipeWriterOld.WriteData(new byte[] { 1, 2, 3, 4, 5, 6 });
            });
        }

        public string GetAppDomainString()
        {
            return "String from AppDomain";
        }

        public SomeDataSerializable GetAppDomainSerializableData()
        {
            return new SomeDataSerializable() { Data = "AppDomainSerializableData" };
        }
        public SomeDataMarshaled GetAppDomainMarshaledData()
        {
            return new SomeDataMarshaled() { Data = "AppDomainMarshaledData" };
        }

    }
}