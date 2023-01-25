using System;
using System.IO;
using AnignoraDataTypes.Configurations;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class TestConfigurators
    {
        [Test]
        public void ConfiguratorXmlTest()
        {
            const string FILENAME = "ConfiguratorXmlTest.xml";
            string path = Path.Combine("Configuration",FILENAME);
            if(File.Exists(path))File.Delete(path);
            ConfiguratorXml<TestClass> configuratorXml = new ConfiguratorXml<TestClass>(FILENAME);
            //Delete temp test file leftover
            File.Delete(FILENAME);
            //Load and check defaults
            configuratorXml.Load();
            Assert.AreEqual(3, configuratorXml.Configuration.IntValue);
            Assert.AreEqual(2,configuratorXml.Configuration.ArrayOfFloats.Length);
            //Make some changes
            configuratorXml.Configuration.IntValue = 4;
            configuratorXml.Configuration.ArrayOfFloats = new[] { 11.3f,2f,3f};
            //Save changes, load and verifies changes
            configuratorXml.Save();
            configuratorXml.Load();
            Assert.AreEqual(4, configuratorXml.Configuration.IntValue);
            Assert.AreEqual(3, configuratorXml.Configuration.ArrayOfFloats.Length);
            //Delete temp test file
            File.Delete(FILENAME);
        }

        public class TestClass : IConfiguration
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
            public float[] ArrayOfFloats { get; set; }


            public void SetDefaults()
            {
                IntValue = 3;
                StringValue = "Hello";
                ArrayOfFloats = new[] { 1.2f, 3.4f };
            }
        }
    }
}
