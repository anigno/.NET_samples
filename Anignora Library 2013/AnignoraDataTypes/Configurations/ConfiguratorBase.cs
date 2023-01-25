using System.IO;
using log4net;

namespace AnignoraDataTypes.Configurations
{
    /// <summary>
    /// File based abstract generic configurator
    /// </summary>
    /// <typeparam name="T">IConfiguration ,new()</typeparam>
    public abstract class ConfiguratorBase<T> where T : IConfiguration, new()
    {
        #region Constructors

        protected ConfiguratorBase(string p_filename, string p_configurationSubFolder = "Configuration")
        {
            Filename = Path.GetDirectoryName(p_filename) + p_configurationSubFolder + @"\" + Path.GetFileName(p_filename);
        }

        #endregion

        #region Public Methods

        public virtual T Load()
        {
            if (!File.Exists(Filename))
            {
                Configuration = new T();
                Configuration.SetDefaults();
                Save();
                s_logger.DebugFormat("Created new configuration file: {0}", Filename);
            }
            Configuration = LoadSpecific();

            return Configuration;
        }

        public void Save()
        {
            string folder = Path.GetDirectoryName(Filename);
            if (folder.Trim() != "" && !Directory.Exists(folder)) Directory.CreateDirectory(folder);
            SaveSpecific();
        }

        #endregion

        #region Public Properties

        public T Configuration { get; protected set; }

        public string Filename { get; private set; }

        #endregion

        #region Protected Methods

        protected abstract T LoadSpecific();

        protected abstract void SaveSpecific();

        #endregion

        #region Fields

        private static readonly ILog s_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}