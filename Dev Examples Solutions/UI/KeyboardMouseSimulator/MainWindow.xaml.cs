#region

using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace KeyboardMouseSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            m_hook.HookedKeys.Add(Keys.Up);
            m_hook.HookedKeys.Add(Keys.Down);
            m_hook.HookedKeys.Add(Keys.Left);
            m_hook.HookedKeys.Add(Keys.Right);
            m_hook.HookedKeys.Add(Keys.CapsLock);

           

            m_hook.KeyUp += (p_sender, p_keyEventArgs) =>
            {
                p_keyEventArgs.SuppressKeyPress = true;
                switch (p_keyEventArgs.KeyCode)
                {
                    case Keys.Up:
                        m_ay = 0;
                        m_dy = 0;
                        break;
                    case Keys.Down:
                        m_ay = 0;
                        m_dy = 0;
                        break;
                    case Keys.Left:
                        m_ax = 0;
                        m_dx = 0;
                        break;
                    case Keys.Right:
                        m_ax = 0;
                        m_dx = 0;
                        break;
                    case Keys.CapsLock:
                        m_isCaps = !m_isCaps;
                        break;
                }
            };
            m_hook.KeyDown += (p_sender, p_keyEventArgs) =>
            {
                p_keyEventArgs.SuppressKeyPress = true;
                switch (p_keyEventArgs.KeyCode)
                {
                    case Keys.Up:
                        m_ay = -1;
                        break;
                    case Keys.Down:
                        m_ay = +1;
                        break;
                    case Keys.Left:
                        m_ax = -1;
                        break;
                    case Keys.Right:
                        m_ax = +1;
                        break;
                }
            };
            m_hook.hook();
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (m_isCaps)
                    {
                        m_dy += m_ay;
                        m_dx += m_ax;
                        m_dy = m_dy*m_ay*m_ay;
                        m_dx = m_dx*m_ax*m_ax;
                        if (m_dy > 20) m_dy = 20;
                        if (m_dx > 20) m_dx = 20;
                        MouseSimulator.MousePoint cursorPosition = MouseSimulator.GetCursorPosition();
                        MouseSimulator.SetCursorPosition(cursorPosition.X + m_dx, cursorPosition.Y + m_dy);
                    }
                    await Task.Delay(25);
                }
            }, TaskCreationOptions.LongRunning);
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private readonly GlobalKeyboardHook m_hook = new GlobalKeyboardHook();
        private int m_ax = 0;
        private int m_ay = 0;
        private int m_dx = 0;
        private int m_dy = 0;

        private bool m_isCaps;

        #endregion
    }
}