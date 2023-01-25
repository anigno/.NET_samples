using System.IO;
using System.Xml.Serialization;

namespace AnignoraDataTypes.Configurations
{
    public class ConfiguratorXml<T> : ConfiguratorBase<T> where T : IConfiguration, new()
    {
        #region Constructors

        public ConfiguratorXml(string p_filename, string p_defaultFolder = "Configuration")
            : base(p_filename, p_defaultFolder)
        {
            m_xmlSerializer = new XmlSerializer(typeof (T));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Load configuration from file, if file not exist, Base class creates new instance and call SetDefaults;
        /// </summary>
        protected override T LoadSpecific()
        {
            FileStream fs = new FileStream(Filename, FileMode.Open);
            object o = m_xmlSerializer.Deserialize(fs);
            Configuration = (T) o;
            fs.Close();
            return Configuration;
        }

        protected override void SaveSpecific()
        {
            FileStream fs = new FileStream(Filename, FileMode.Create);
            m_xmlSerializer.Serialize(fs, Configuration);
            fs.Close();
        }

        #endregion

        #region Fields

        private readonly XmlSerializer m_xmlSerializer;

        #endregion
    }
}