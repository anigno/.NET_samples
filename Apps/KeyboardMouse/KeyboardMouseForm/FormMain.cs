using AnignoraUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardMouseForm
{
    public partial class FormMain : Form
    {

        private KeyboardMouseProvider m_provider = new KeyboardMouseProvider();
        private Thread m_periodicKeyCheck;
        private const int A = 3;
        private int ax=A;
        private int ay=A;


        public FormMain()
        {
            InitializeComponent();
            m_provider.KeyDown += M_provider_KeyDown;
            m_provider.KeyUp += M_provider_KeyUp;
            m_periodicKeyCheck = new Thread(periodicKeyCheckMethod)
            {
                Priority = ThreadPriority.Highest
            };
            m_periodicKeyCheck.Start();
        }

        private void M_provider_KeyUp(object sender, KeyEventArgs e)
        {
            bool capsState = m_provider.GetKetState(0x14);
            if (capsState)
            {
                if (e.KeyCode == Keys.A) m_provider.MouseButton(AnignoraUI.HooksAndImmulate.MouseEventFlags.LEFTUP);
            }
        }

        private void M_provider_KeyDown(object sender, KeyEventArgs e)
        {
            bool capsState = m_provider.GetKetState(0x14);
            if (capsState)
            {
                if (e.KeyCode == Keys.A) m_provider.MouseButton(AnignoraUI.HooksAndImmulate.MouseEventFlags.LEFTDOWN);
            }
        }

        private void periodicKeyCheckMethod()
        {
            while (true)
            {
                bool capsState = m_provider.GetKetState(0x14);
                CrossThreadActivities.Do(this,a => { Text = "" + capsState; });
                if (capsState)
                {
                    if (m_provider.IsKeyDown(Keys.Left))
                    {
                        m_provider.MoveMouse(-ax, 0);
                        ax += A;
                    }
                    else if (m_provider.IsKeyDown(Keys.Right))
                    {
                        m_provider.MoveMouse(+ax, 0);
                        ax += A;
                    }
                    else
                    {
                        ax = A;
                    }
                    if (m_provider.IsKeyDown(Keys.Up))
                    {
                        m_provider.MoveMouse(0, -ay);
                        ay += A;
                    }
                    else if (m_provider.IsKeyDown(Keys.Down))
                    {
                        m_provider.MoveMouse(0, +ay);
                        ay += A;
                    }
                    else
                    {
                        ay = A;
                    }
                }
                else
                {
                    ax = ay = 0;
                    m_provider.MouseButton(AnignoraUI.HooksAndImmulate.MouseEventFlags.LEFTUP);
                }
                Thread.Sleep(100);
            }
        }
    }
}
