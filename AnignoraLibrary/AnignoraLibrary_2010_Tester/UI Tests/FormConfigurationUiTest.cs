using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using AnignoraDataTypes.Configurations;

namespace AnignoraLibrary_2010_Tester.UI_Tests
{
    public partial class FormConfigurationUiTest : Form
    {
        public class TestClass : IConfiguration
        {
            
            [Description("Some Integer"),Category("Numbers")]
            public int IntValue { get; set; }
            [Description("Any string you like"), Category("Strings")]
            public string StringValue { get; set; }
            [Description("An array of floats"), Category("Numbers")]
            public float[] ArrayOfFloats { get; set; }
            [Description("An array of strings"), Category("Strings")]
            public string[] ArrayOfStrings { get; set; }


            void IConfiguration.SetDefaults()
            {
                IntValue = 17;
                StringValue = "Hello";
                ArrayOfFloats=new float[]{1,2,3};
            }
        }

        private const string TEST_FILE = "TestFile.xml";
        readonly ConfiguratorXml<TestClass> m_configuratorXml = new ConfiguratorXml<TestClass>(TEST_FILE);
        TestClass m_testvClass=new TestClass();

        public FormConfigurationUiTest()
        {
            InitializeComponent();
        }

        private void formConfigurationUiTestLoad(object p_sender, EventArgs p_e)
        {
            m_testvClass = m_configuratorXml.Load();
            propertyGrid1.SelectedObject = m_testvClass;
        }

        protected override void OnClosing(CancelEventArgs p_e)
        {
            base.OnClosing(p_e);
            m_configuratorXml.Save();
        }
    }
}
