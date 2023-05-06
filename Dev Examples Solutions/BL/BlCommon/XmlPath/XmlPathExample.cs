#region

using System.Xml;
using NUnit.Framework;

#endregion

namespace BlCommon.XmlPath
{
    public class XmlPathExample
    {
        #region Public Methods

        [Test]
        public void TestXpath()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(TEST_FILE);

            XmlNodeList vehicles = xmlDoc.SelectNodes(@"//Vehicle"); //All vehicles
            TestUtils.DebugLog("Found {0} Vehicles", vehicles.Count);

            XmlNodeList cars1 = xmlDoc.SelectNodes(@"//Cars/Vehicle"); //All vehicles in cars
            XmlNodeList cars2 = xmlDoc.SelectNodes(@"Garage/Cars/Vehicle"); //Absolute
            TestUtils.DebugLog("Found {0} Vehicles in cars", cars1.Count);
            TestUtils.DebugLog("Found {0} Vehicles in cars", cars2.Count);

            XmlNodeList volvos = xmlDoc.SelectNodes(@"//Vehicle[@Type='Volvo']");
            TestUtils.DebugLog("Found {0} volvos", volvos.Count);

            XmlNodeList highNumbers = xmlDoc.SelectNodes(@"//Vehicle[@Number>'40']");
            TestUtils.DebugLog("Found {0} highNumbers", highNumbers.Count);

            XmlNode aTruck = xmlDoc.SelectSingleNode(@"//Trucks/Vehicle");
            TestUtils.DebugLog("Found {0} truck owners", aTruck.ChildNodes.Count);
            XmlNodeList owners = aTruck.SelectNodes(@"Owner"); //Relative to node
            TestUtils.DebugLog("Found {0} truck owners", owners.Count);

            TestUtils.DebugLog("Owner: {0}", owners[0].Attributes["Name"].InnerText);
            TestUtils.DebugLog("Parent number: {0}", owners[0].ParentNode.Attributes["Number"].InnerText);
        }

        #endregion

        #region Constants

        private const string TEST_FILE = @"XmlPath\Test.xml";

        #endregion
    }
}