using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AnignoraUI.HooksAndImmulate;

namespace AnignoraCommonAndHelpers.HooksAndImmulate
{
    public partial class FormHookTester : Form
    {
		#region (------  Fields  ------)

        int closeCnt = 40;
        private readonly KeyboardHookController keyHook = new KeyboardHookController();
        private readonly MouseHookController mouseHook = new MouseHookController();

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public FormHookTester()
        {
            InitializeComponent();
            buttonMouseClickTest.Click += buttonMouseClickTest_Click;
            keyHook.KeyDown += keyHook_KeyDown;
        }

		#endregion (------  Constructors  ------)

		#region (------  Private Methods  ------)

        void buttonMouseClickTest_Click(object sender, EventArgs e)
        {
            buttonMouseClickTest.Text = "Clicked " + DateTime.Now.ToLongTimeString();
        }

        private void buttonMoveMouseAndClick_Click(object sender, EventArgs e)
        {
            const int headerHeight = 40;
            int absoluteX = Left + buttonMouseClickTest.Left + 20;
            int absoluteY = Top + buttonMouseClickTest.Top + headerHeight;
            mouseHook.SetMousePosition(absoluteX,absoluteY );
            mouseHook.EmulateMouseButton(MouseEventFlags.LEFTDOWN);
            mouseHook.EmulateMouseButton(MouseEventFlags.LEFTUP);
        }

        private void buttonScrollDown_Click(object sender, EventArgs e)
        {
            listBoxScrollTest.Focus();
            mouseHook.EmulateMouseWheel(MouseWheelDirectionEnum.Down);
        }

        private void buttonScrollUp_Click(object sender, EventArgs e)
        {
            listBoxScrollTest.Focus();
            mouseHook.EmulateMouseWheel(MouseWheelDirectionEnum.Up);
        }

        void keyHook_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKeyTest2.Text = e.KeyCode + " " + e.KeyValue;
            e.SuppressKeyPress = true;
        }

        private void textBoxTestKeyEmulate_Click(object sender, EventArgs e)
        {
            keyHook.EmulateKey(65, KeyboardHookController.KeyEventEnum.KeyDown);
            keyHook.EmulateKey(65, KeyboardHookController.KeyEventEnum.KeyUp);
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            Point p=mouseHook.GetMousePosition();
            textBoxMouseLocation.Text = p.X + "," + p.Y;
            if(closeCnt--<=0)Close();
            Text = closeCnt.ToString();
        }

		#endregion (------  Private Methods  ------)
    }
}
