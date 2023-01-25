using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections;

namespace AnignoraUI.Common
{
    [ProvideProperty("LanguageText", typeof(System.Windows.Forms.Control))]
    public class LanguageExtension : Component, IExtenderProvider
    {
		#region (------------------  Fields  ------------------)
        private System.Windows.Forms.Control containerControl;
        private CultureInfo culture = new CultureInfo("en");
        readonly Hashtable hashTable = new Hashtable();
		#endregion (------------------  Fields  ------------------)


		#region (------------------  Properties  ------------------)
        public System.Windows.Forms.Control ContainerControl
        {
            get { return containerControl; }
            set { containerControl = value; }
        }

        public CultureInfo Culture
        {
            get { return culture; }
            set
            {
                culture = value;
                if (ContainerControl != null)
                {
                    ContainerControl.ControlAdded += ContainerControl_ControlAdded;
                    foreach (System.Windows.Forms.Control c in ContainerControl.Controls)
                    {
                        UpdateText(c);
                    }
                }
            }
        }

        public string CultureName
        {
            get
            {
                return Culture.Name;
            }
        }
		#endregion (------------------  Properties  ------------------)


		#region (------------------  Event Handlers  ------------------)
        void ContainerControl_ControlAdded(object sender, ControlEventArgs e)
        {
            UpdateText(e.Control);
        }
		#endregion (------------------  Event Handlers  ------------------)


		#region (------------------  Private Methods  ------------------)
        void UpdateText(System.Windows.Forms.Control control)
        {
            if (Culture.Name == Thread.CurrentThread.CurrentCulture.Name)
            {
                control.Text = GetLanguageText(control);
            }
        }
		#endregion (------------------  Private Methods  ------------------)


		#region (------------------  Public Methods  ------------------)
        public bool CanExtend(object extendee)
        {
            if (extendee is System.Windows.Forms.Control) return true; return false;
        }

        public string GetLanguageText(System.Windows.Forms.Control control)
        {
            if (hashTable.Contains(control)) return hashTable[control].ToString();
            return control.Text;
        }

        public void SetLanguageText(System.Windows.Forms.Control control, string value)
        {
            if (hashTable.Contains(control))
            {
                hashTable[control] = value;
            }
            else
            {
                hashTable.Add(control, value);
            }
            UpdateText(control);
        }
		#endregion (------------------  Public Methods  ------------------)
    }
}