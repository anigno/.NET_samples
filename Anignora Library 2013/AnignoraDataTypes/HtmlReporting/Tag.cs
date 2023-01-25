using System.Collections.Generic;
using System.Text;

namespace AnignoraDataTypes.HtmlReporting
{
    public class Tag
    {
        #region Constructors

        public Tag(string p_tagName)
        {
            TagName = p_tagName;
        }

        #endregion

        #region Public Methods

        public Tag AddTag(Tag p_tag)
        {
            Tags.Add(p_tag);
            return p_tag;
        }

        public void AddAttribute(TagAttribute p_attribute)
        {
            TagAttributes.Add(p_attribute);
        }

        public override string ToString()
        {
            StringBuilder stringBuilderTags = new StringBuilder();
            foreach (Tag tag in Tags)
            {
                stringBuilderTags.Append(tag);
            }
            return GetOpenTagString(TagName) + InnerText + stringBuilderTags + GetCloseagString(TagName);
        }

        public string GetOpenTagString(string p_tagname)
        {
            StringBuilder stringBuilderAttributes = new StringBuilder();
            foreach (TagAttribute attribute in TagAttributes)
            {
                stringBuilderAttributes.Append(string.Format(" {0}=\"{1}\" ", attribute.Key, attribute.Value));
            }
            return string.Format("<{0}{1}>", p_tagname, stringBuilderAttributes);
        }

        public string GetCloseagString(string p_tagname)
        {
            return string.Format("</{0}>", p_tagname);
        }

        #endregion

        #region Public Properties

        public string TagName { get; private set; }
        public string InnerText { get; set; }

        #endregion

        #region Fields

        protected readonly List<Tag> Tags = new List<Tag>();
        protected readonly List<TagAttribute> TagAttributes = new List<TagAttribute>();

        #endregion
    }
}