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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_3D.A3dModeling;
using Wpf_3D_Example.Elements;

#endregion

namespace Wpf_3D_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly Arrow m_arrow = new Arrow();

        #region Constructors

        public MainWindow()
        {
            //Matrix3D m1 = new Matrix3D(1, 2, 3, 4, 5, 6, 7, 8, 9);
            //Matrix3D m2 = new Matrix3D(1, 0, 0, 1, 0, 0, 1, 0, 0);
            //Point3D p1=new Point3D(1,1,1);

            //Point3D p2= m1*p1;
            //var v1 = m1 + m2;
            //var v2 = m1 * m2;

            InitializeComponent();
            DataContext = this;

            AViewport3D aViewport3D = new AViewport3D();
            OrthographicCamera cameraOrthographicCamera = new OrthographicCamera(new Point3D(60, 30, 0), new Vector3D(-1, -1, 0), new Vector3D(0, 0, 1), 120);
            aViewport3D.Camera = cameraOrthographicCamera;

            //aViewport3D.DirectionalLightsList.Clear();
            aViewport3D.GeometryModels.Add(m_cube.CreateGeometryModel3D());
            //aViewport3D.GeometryModels.Add(m_arrow.CreateGeometryModel3D());

            Viewport3D viewPort3D = aViewport3D.CreateViewPort3D();
            Grid.Children.Add(viewPort3D);
            m_cube.OnRotatedPry.Subscribe(p_d => { LabelX.Content = string.Format("RX:{0:0.00} RY:{1:0.00} RZ:{2:0.00} P:{3:0.00} Y:{4:0.00} R:{5:0.00}", p_d[0].X, p_d[0].Y, p_d[0].Z, p_d[1].X, p_d[1].Y, p_d[1].Z); });
        }

        #endregion

        #region Private Methods

        private void Translate_OnValueChanged(object p_sender, RoutedPropertyChangedEventArgs<double> p_e)
        {
            m_cube.TranslateTo(TranslateX.Value/1.0, TranslateY.Value/1.0, TranslateZ.Value/1.0);
        }

        private void Rotate_OnValueChanged(object p_sender, RoutedPropertyChangedEventArgs<double> p_e)
        {
            m_cube.RotateTo(RotateX.Value, RotateY.Value, RotateZ.Value);
        }

        private void Scale_OnValueChanged(object p_sender, RoutedPropertyChangedEventArgs<double> p_e)
        {
            m_cube.ScaleTo(Scale.Value/10.0, Scale.Value/10.0, Scale.Value/10.0);
        }


        private void ButtonResetAll_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            RotateX.Value = RotateY.Value = RotateZ.Value = 0;
        }

        private void PitchInc_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.PitchInc(15);
        }

        private void PitchDec_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.PitchDec(15);
        }

        private void YawInc_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.YawInc(15);
        }

        private void YawDec_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.YawDec(15);
        }

        private void RollInc_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.RollInc(15);
        }

        private void RollDec_OnClick(object p_sender, RoutedEventArgs p_e)
        {
            m_cube.RollDec(15);
        }

        #endregion

        #region Fields

        private readonly Cube m_cube = new Cube();

        #endregion
    }
}