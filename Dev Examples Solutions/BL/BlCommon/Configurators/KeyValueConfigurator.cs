#region

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#endregion

namespace BlCommon.Configurators
{
    /// <summary>
    /// Key, Value and description xml configurator using XmlPath to search for data
    /// </summary>
    public class KeyValueConfigurator
    {
        #region Constructors

        public KeyValueConfigurator(string p_configurationFile)
        {
            m_configurationFile = p_configurationFile;
            init();
        }

        #endregion

        #region Public Methods

        public IEnumerable<ConfigurationElement> GetValues(string p_nodeName, string p_key)
        {
            string xpath = @"//" + p_nodeName + "[@key='" + p_key + @"']";
            XmlNodeList xmlNodeList = m_document.SelectNodes(xpath);
            return FromXmlNodeList(xmlNodeList);
        }

        public IEnumerable<ConfigurationElement> GetValues(string p_nodeName)
        {
            string xpath = @"//" + p_nodeName;
            XmlNodeList xmlNodeList = m_document.SelectNodes(xpath);
            return FromXmlNodeList(xmlNodeList);
        }

        #endregion

        #region Private Methods

        private void init()
        {
            m_document.Load(m_configurationFile);
        }

        private IEnumerable<ConfigurationElement> FromXmlNodeList(IEnumerable p_xmlNodeList)
        {
            List<ConfigurationElement> configurationElements = new List<ConfigurationElement>();
            foreach (XmlNode xmlNode in p_xmlNodeList)
            {
                ConfigurationElement configurationElement = fromXmlNode(xmlNode);
                configurationElements.Add(configurationElement);
            }
            return configurationElements;
        }

        private static ConfigurationElement fromXmlNode(XmlNode p_xmlNode)
        {
            ConfigurationElement configurationElement = new ConfigurationElement();
            if (p_xmlNode.Attributes == null) return configurationElement;
            configurationElement.Key = p_xmlNode.Attributes["key"] == null ? "" : p_xmlNode.Attributes["key"].Value;
            configurationElement.Value = p_xmlNode.Attributes["value"] == null ? "" : p_xmlNode.Attributes["value"].Value;
            configurationElement.Description = p_xmlNode.Attributes["description"] == null ? "" : p_xmlNode.Attributes["description"].Value;
            return configurationElement;
        }

        #endregion

        #region Fields

        private readonly string m_configurationFile;
        private readonly XmlDocument m_document = new XmlDocument();

        #endregion
    }
}