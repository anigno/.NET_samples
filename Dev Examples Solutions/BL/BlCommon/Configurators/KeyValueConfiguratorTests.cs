#region

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#endregion

namespace BlCommon.Configurators
{
    [TestFixture]
    public class KeyValueConfiguratorTests
    {
        #region Public Methods

        [Test]
        public void Test01()
        {
            const string CONFIG_FILE = @"Configuration\ConfigurationTest.xml";
            KeyValueConfigurator configurator = new KeyValueConfigurator(CONFIG_FILE);
            IEnumerable<ConfigurationElement> configurationElements;
            configurationElements = configurator.GetValues("Person", "1001");
            Assert.AreEqual(3, configurationElements.Count());
            configurationElements = configurator.GetValues("Person");
            Assert.AreEqual(8, configurationElements.Count());
            configurationElements = configurator.GetValues("Persons");
            Assert.AreEqual(2, configurationElements.Count());
            configurationElements = configurator.GetValues(@"MorePersons/Persons");
            Assert.AreEqual(1, configurationElements.Count());
            configurationElements = configurator.GetValues(@"MorePersons/Persons/Person");
            Assert.AreEqual(2, configurationElements.Count());
            configurationElements = configurator.GetValues(@"Persons/Person");
            Assert.AreEqual(5, configurationElements.Count());
        }

        #endregion
    }
}