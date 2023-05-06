#region

using System;
using System.Windows.Markup;

#endregion

namespace MarkupExamples
{
    public class EnumMarkupExtension : MarkupExtension
    {
        #region Constructors

        public EnumMarkupExtension(Type p_enumType)
        {
            m_enumType = p_enumType;
        }

        #endregion

        #region Public Methods

        public override object ProvideValue(IServiceProvider p_serviceProvider)
        {
            string[] names = Enum.GetNames(m_enumType);
            for (int index = 0; index < names.Length; index++)
            {
                names[index] = PrefixString + names[index];
            }
            return names;
        }

        #endregion

        #region Public Properties

        public string PrefixString { get; set; }

        #endregion

        #region Fields

        private readonly Type m_enumType;

        #endregion
    }

    public enum MyEnum
    {
        One,
        Two,
        Three
    }
}