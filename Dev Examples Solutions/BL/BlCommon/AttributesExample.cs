#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BlCommon
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DescriptionAttribute : Attribute
    {
        #region Constructors

        public DescriptionAttribute(string p_name)
        {
            Name = p_name;
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }
        public string Version { get; set; }
        public string InfoFile { get; set; }

        #endregion
    }

    //Usage
    [Description("Some Name", Version = "1.1", InfoFile = "Info.txt")]
    public class MyClass
    {
    }

    public class AttributeReading
    {
        #region Public Methods

        public void ReadAttributes()
        {
            Type type = this.GetType();
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute) type.GetCustomAttribute(typeof (DescriptionAttribute));
            if (descriptionAttribute != null)
            {
                string name = descriptionAttribute.Name;
                string version = descriptionAttribute.Version;
                string infoFile = descriptionAttribute.InfoFile;
            }
        }

        #endregion
    }
}