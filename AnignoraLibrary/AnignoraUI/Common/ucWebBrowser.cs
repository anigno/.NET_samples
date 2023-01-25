using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AnignoraUI.Common
{
    public partial class ucWebBrowser : UserControl
    {
		#region (------  Const Fields  ------)

        public const string CATEGORY_STRING = " AWebBrowser";

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        #endregion (------  Fields  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public string HomePageUrl { get; set; }

        #endregion (------  Properties  ------)

		#region (------  Event Handlers  ------)

        private void buttonBack_Click(object sender, EventArgs e)
        {
            webBrowserMain.GoBack();
        }

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            webBrowserMain.GoForward();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            webBrowserMain.Navigate(textBoxUrl.Text);
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            webBrowserMain.Navigate(HomePageUrl);
        }

        private void textBoxUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Return)
            {
                webBrowserMain.Navigate(textBoxUrl.Text);
            }
        }

        void webBrowserMain_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if(e.Url.ToString()!="about:blank")textBoxUrl.Text = e.Url.ToString();
        }

		#endregion (------  Event Handlers  ------)

		#region (------  Constructors  ------)

        public ucWebBrowser()
        {
            InitializeComponent();
        }

		#endregion (------  Constructors  ------)

		#region (------  Overridden Methods  ------)

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            webBrowserMain.ScriptErrorsSuppressed = true; 
            webBrowserMain.Navigating += webBrowserMain_Navigating;
            webBrowserMain.Navigate(HomePageUrl);
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Public Methods  ------)

        public void Navigate(string url)
        {
            webBrowserMain.Navigate(url);
        }

		#endregion (------  Public Methods  ------)
    }
}
