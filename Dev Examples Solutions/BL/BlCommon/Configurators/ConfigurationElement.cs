namespace BlCommon.Configurators
{
    public class ConfigurationElement
    {
        #region Public Methods

        public override string ToString()
        {
            return string.Format("[key={0} value={1} description={2}]", Key, Value, Description);
        }

        #endregion

        #region Public Properties

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        #endregion
    }
}