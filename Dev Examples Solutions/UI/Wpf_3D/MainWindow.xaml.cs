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

#endregion

namespace Wpf_3D
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

            AGeometryModel3D aGeometryModel3D1 = new AGeometryModel3D();
            aGeometryModel3D1.AddSquere(new[] {new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0), new Point3D(1, 0, 0)});
            aGeometryModel3D1.AddSquere(new[] {new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(0, 1, 1), new Point3D(0, 0, 1)});
            aGeometryModel3D1.AddSquere(new[] {new Point3D(0, 0, 0), new Point3D(1, 0, 0), new Point3D(1, 0, 1), new Point3D(0, 0, 1)});
            aGeometryModel3D1.AddSquere(new[] {new Point3D(0, 0, 1), new Point3D(0, 1, 1), new Point3D(1, 1, 1), new Point3D(1, 0, 1)});

            AGeometryModel3D aGeometryModel3D2 = new AGeometryModel3D();
            aGeometryModel3D2.AddSquere(new[] {new Point3D(2, 0, 0), new Point3D(2, 1, 0), new Point3D(3, 1, 0), new Point3D(3, 0, 0)});
            aGeometryModel3D2.AddSquere(new[] {new Point3D(2, 0, 0), new Point3D(2, 1, 0), new Point3D(2, 1, 1), new Point3D(2, 0, 1)});
            aGeometryModel3D2.AddSquere(new[] {new Point3D(2, 0, 0), new Point3D(3, 0, 0), new Point3D(3, 0, 1), new Point3D(2, 0, 1)});
            aGeometryModel3D2.AddSquere(new[] {new Point3D(2, 0, 1), new Point3D(2, 1, 1), new Point3D(3, 1, 1), new Point3D(3, 0, 1)});

            ImageBrush imageBrush1 = new ImageBrush(new BitmapImage(new Uri("sampleImages\\CA-wp1.jpg", UriKind.Relative)));
            aGeometryModel3D1.Material = new DiffuseMaterial(imageBrush1);

            aGeometryModel3D2.Material = new DiffuseMaterial(new LinearGradientBrush(Colors.Yellow, Colors.Blue, new Point(0, 0), new Point(1, 1)));

            GeometryModel3D geometryModel3D1 = aGeometryModel3D1.CreateGeometryModel3D();
            GeometryModel3D geometryModel3D2 = aGeometryModel3D2.CreateGeometryModel3D();

            AViewport3D aViewport3D = new AViewport3D();
            aViewport3D.GeometryModels.Add(geometryModel3D1);
            aViewport3D.GeometryModels.Add(geometryModel3D2);

            aViewport3D.DirectionalLightsList.Clear();
            aViewport3D.DirectionalLightsList.Add(new DirectionalLight(Color.FromRgb(255, 100, 100), new Vector3D(-100, -100, 100)));
            aViewport3D.DirectionalLightsList.Add(new DirectionalLight(Color.FromRgb(100, 255, 100), new Vector3D(50, -100, 50)));
            aViewport3D.DirectionalLightsList.Add(new DirectionalLight(Color.FromRgb(100, 100, 255), new Vector3D(-50, 0, 0)));

            aViewport3D.Camera = new PerspectiveCamera(new Point3D(3, 3, -3), new Vector3D(-1, -1, 1), new Vector3D(0, 0, 1), 120);

            Viewport3D viewPort3D = aViewport3D.CreateViewPort3D();
            Content = viewPort3D;

            AxisAngleRotation3D axisAngleRotation3D2 = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            Point3D centerPoint3D2 = new Point3D(2.5, .5, 0);
            aGeometryModel3D2.AddRotateTransform3D(axisAngleRotation3D2, centerPoint3D2);

            ScaleTransform3D scaleTransform3D = aGeometryModel3D1.AddScaleTransform3D();
            TranslateTransform3D translateTransform3D = aGeometryModel3D1.AddTranslateTransform3D();

            AxisAngleRotation3D axisAngleRotation3D1 = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            Point3D centerPoint3D1 = new Point3D(0, 0, 0);
            aGeometryModel3D1.AddRotateTransform3D(axisAngleRotation3D1, centerPoint3D1);

            Task.Factory.StartNew(() =>
            {
                Action<int> action = (p_a) =>
                {
                    scaleTransform3D.ScaleX = scaleTransform3D.ScaleY = scaleTransform3D.ScaleZ = p_a/500.0 + 0.5;
                    translateTransform3D.OffsetY = p_a/500.0;
                    centerPoint3D1.Y = translateTransform3D.OffsetY;
                    centerPoint3D1.X = translateTransform3D.OffsetX;
                    centerPoint3D1.Z = translateTransform3D.OffsetZ;
                    axisAngleRotation3D1.Angle = p_a;

                    axisAngleRotation3D2.Angle = p_a;
                };
                for (int a = 0; a < 10000; a++)
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(action, a);
                    }
                    catch (Exception ex)
                    {
                    }
                    Task.Delay(5).Wait();
                }
            });
        }

        #endregion
    }
}