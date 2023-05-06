#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#endregion

namespace WindowEx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            HeaderHeight = 20;
            WindowStyle = WindowStyle.None;
        }

        #endregion

        #region Public Properties

        public int HeaderHeight { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnMouseDown(MouseButtonEventArgs p_e)
        {
            base.OnMouseDown(p_e);
            m_x = p_e.GetPosition(this).X;
            m_y = p_e.GetPosition(this).Y;
        }

        protected override void OnMouseMove(MouseEventArgs p_e)
        {
            base.OnMouseMove(p_e);
            if (p_e.LeftButton == MouseButtonState.Pressed)
            {
                double yFactor = p_e.MouseDevice.GetPosition(this).Y;
                if (yFactor < HeaderHeight)
                {
                    Left = Left + p_e.MouseDevice.GetPosition(this).X - m_x;
                    Top = Top + yFactor - m_y;
                }
            }
        }

        #endregion

        #region Fields

        private double m_x;
        private double m_y;

        #endregion
    }
}