#region

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#endregion

namespace Wpf_3D.A3dModeling
{
    public class AViewport3D
    {
        #region Constructors

        public AViewport3D()
        {
            GeometryModels = new List<GeometryModel3D>();
            DirectionalLightsList = new List<DirectionalLight>();
            DirectionalLightsList.Add(new DirectionalLight(Colors.Red, new Vector3D(-100, 0, 0)));
            DirectionalLightsList.Add(new DirectionalLight(Colors.Green, new Vector3D(0, -100, 0)));
            DirectionalLightsList.Add(new DirectionalLight(Colors.Blue, new Vector3D(0, 0, 100)));
            AmbientLight = new AmbientLight(Colors.LightGray);
            Camera = m_cameraOrthographicCamera;
        }

        #endregion

        #region Public Methods

        public Viewport3D CreateViewPort3D()
        {
            Model3DGroup myModel3DGroup = new Model3DGroup();
            myModel3DGroup.Children.Add(AmbientLight);
            foreach (DirectionalLight light in DirectionalLightsList)
            {
                myModel3DGroup.Children.Add(light);
            }
            foreach (GeometryModel3D geometryModel3D in GeometryModels)
            {
                myModel3DGroup.Children.Add(geometryModel3D);
            }

            ModelVisual3D modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = myModel3DGroup;
            Viewport3D viewPort3D = new Viewport3D();
            viewPort3D.Children.Add(modelVisual3D);
            viewPort3D.Camera = Camera;
            return viewPort3D;
        }

        #endregion

        #region Public Properties

        public ProjectionCamera Camera { get; set; }

        public List<DirectionalLight> DirectionalLightsList { get; private set; }

        public List<GeometryModel3D> GeometryModels { get; private set; }

        public AmbientLight AmbientLight { get; set; }

        #endregion

        #region Fields

        private readonly PerspectiveCamera m_cameraPrespective = new PerspectiveCamera(new Point3D(30, 30, -10), new Vector3D(-1, -1, 1), new Vector3D(0, 0, 1), 120);
        private readonly OrthographicCamera m_cameraOrthographicCamera = new OrthographicCamera(new Point3D(30, 30, -10), new Vector3D(-1, -1, 1), new Vector3D(0, 0, 1), 120);

        #endregion
    }
}