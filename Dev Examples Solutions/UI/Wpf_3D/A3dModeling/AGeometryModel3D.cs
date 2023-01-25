#region

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#endregion

namespace Wpf_3D.A3dModeling
{
    public class AGeometryModel3D
    {
        #region Constructors

        public AGeometryModel3D()
        {
            Material = new DiffuseMaterial(new SolidColorBrush(Colors.White));
        }

        #endregion

        #region Public Methods

        public GeometryModel3D CreateGeometryModel3D()
        {
            //CreateViewPort3D MeshGeometry3D
            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
            meshGeometry3D.Positions = m_point3DCollection;
            meshGeometry3D.TriangleIndices = m_triangleIndicesCollection;
            // Apply the mesh to the geometry model.
            GeometryModel3D geometryModel3D = new GeometryModel3D();
            geometryModel3D.Geometry = meshGeometry3D;
            // Define the material for the geometry
            geometryModel3D.Material = Material;
            //Add texture points for each vertex
            for (int a = 0; a < m_point3DCollection.Count/3; a++)
            {
                meshGeometry3D.TextureCoordinates.Add(new Point(0, 0));
                meshGeometry3D.TextureCoordinates.Add(new Point(0, 1));
                meshGeometry3D.TextureCoordinates.Add(new Point(1, 1));
                meshGeometry3D.TextureCoordinates.Add(new Point(1, 0));
            }
            geometryModel3D.Transform = m_transform3DGroup;
            return geometryModel3D;
        }

        public void Clear()
        {
            m_point3DCollection.Clear();
            m_triangleIndicesCollection.Clear();
        }

        public void AddSquere(Point3D[] p_points)
        {
            AddTriangle(p_points);
            Point3D[] points = {p_points[2], p_points[3], p_points[0]};
            AddTriangle(points);
        }

        public void AddTriangle(Point3D[] p_points)
        {
            for (int a = 0; a < 3; a++)
            {
                m_point3DCollection.Add(p_points[a]);
                m_triangleIndicesCollection.Add(m_point3DCollection.Count - 1);
            }
            for (int a = 2; a >= 0; a--)
            {
                m_point3DCollection.Add(p_points[a]);
                m_triangleIndicesCollection.Add(m_point3DCollection.Count - 1);
            }
        }

        public RotateTransform3D AddRotateTransform3D(AxisAngleRotation3D p_axisAngleRotation3D, Point3D p_center)
        {
            RotateTransform3D rotateTransform3D = new RotateTransform3D(p_axisAngleRotation3D, p_center);
            m_transform3DGroup.Children.Add(rotateTransform3D);
            return rotateTransform3D;
        }

        public TranslateTransform3D AddTranslateTransform3D()
        {
            TranslateTransform3D translateTransform3D = new TranslateTransform3D(new Vector3D(0, 0, 0));
            m_transform3DGroup.Children.Add(translateTransform3D);
            return translateTransform3D;
        }

        public ScaleTransform3D AddScaleTransform3D()
        {
            ScaleTransform3D scaleTransform3D = new ScaleTransform3D(new Vector3D(1, 1, 1));
            m_transform3DGroup.Children.Add(scaleTransform3D);
            return scaleTransform3D;
        }

        #endregion

        #region Public Properties

        public DiffuseMaterial Material { get; set; }

        #endregion

        #region Fields

        private readonly Transform3DGroup m_transform3DGroup = new Transform3DGroup();

        private readonly Point3DCollection m_point3DCollection = new Point3DCollection();
        private readonly Int32Collection m_triangleIndicesCollection = new Int32Collection();

        #endregion
    }
}