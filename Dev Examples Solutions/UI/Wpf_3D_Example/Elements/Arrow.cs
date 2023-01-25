#region

using System.Windows.Media.Media3D;
using Wpf_3D.A3dModeling;

#endregion

namespace Wpf_3D_Example.Elements
{
    public class Arrow
    {
        #region Constructors

        public Arrow()
        {
            Geometry = new AGeometryModel3D();
            Geometry.AddSquere(new[] {new Point3D(-10, -10, -10), new Point3D(10, -10, -10), new Point3D(10, 10, -10), new Point3D(-10, 10, -10)});
            Geometry.AddTriangle(new[] {new Point3D(-10, -10, -10), new Point3D(10, -10, -10), new Point3D(0, 0, 20)});
            Geometry.AddTriangle(new[] {new Point3D(10, -10, -10), new Point3D(10, 10, -10), new Point3D(0, 0, 20)});
            Geometry.AddTriangle(new[] {new Point3D(10, 10, -10), new Point3D(-10, 10, -10), new Point3D(0, 0, 20)});
            Geometry.AddTriangle(new[] {new Point3D(-10, 10, -10), new Point3D(-10, -10, -10), new Point3D(0, 0, 20)});

            m_translateTransform3D = Geometry.AddTranslateTransform3D();
            m_scaleTransform3D = Geometry.AddScaleTransform3D();
            Point3D centerRotationPoint = new Point3D(0, 0, 0);
            m_axisAngleRotation3Dx = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
            m_rotX = Geometry.AddRotateTransform3D(m_axisAngleRotation3Dx, centerRotationPoint);
            m_axisAngleRotation3Dy = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            m_rotY = Geometry.AddRotateTransform3D(m_axisAngleRotation3Dy, centerRotationPoint);
            m_axisAngleRotation3Dz = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            m_rotZ = Geometry.AddRotateTransform3D(m_axisAngleRotation3Dz, centerRotationPoint);
        }

        #endregion

        #region Public Methods

        public void TranslateTo(double p_x, double p_y, double p_z)
        {
            m_translateTransform3D.OffsetX = p_x;
            m_translateTransform3D.OffsetY = p_y;
            m_translateTransform3D.OffsetZ = p_z;

            m_rotX.CenterX = m_rotY.CenterX = m_rotZ.CenterX = m_translateTransform3D.OffsetX;
            m_rotX.CenterY = m_rotY.CenterY = m_rotZ.CenterY = m_translateTransform3D.OffsetY;
            m_rotX.CenterZ = m_rotY.CenterZ = m_rotZ.CenterZ = m_translateTransform3D.OffsetZ;
        }

        public void TranslateBy(double p_x, double p_y, double p_z)
        {
            m_translateTransform3D.OffsetX += p_x;
            m_translateTransform3D.OffsetY += p_y;
            m_translateTransform3D.OffsetZ += p_z;

            m_rotX.CenterX = m_rotY.CenterX = m_rotZ.CenterX = m_translateTransform3D.OffsetX;
            m_rotX.CenterY = m_rotY.CenterY = m_rotZ.CenterY = m_translateTransform3D.OffsetY;
            m_rotX.CenterZ = m_rotY.CenterZ = m_rotZ.CenterZ = m_translateTransform3D.OffsetZ;
        }

        public void RotateTo(double p_x, double p_y, double p_z)
        {
            m_axisAngleRotation3Dx.Angle = p_x;
            m_axisAngleRotation3Dy.Angle = p_y;
            m_axisAngleRotation3Dz.Angle = p_z;
        }

        public void ScaleTo(double p_x, double p_y, double p_z)
        {
            m_scaleTransform3D.ScaleX = p_x;
            m_scaleTransform3D.ScaleY = p_y;
            m_scaleTransform3D.ScaleZ = p_z;
        }

        public GeometryModel3D CreateGeometryModel3D()
        {
            return Geometry.CreateGeometryModel3D();
        }

        #endregion

        #region Public Properties

        public double RotationX
        {
            get { return m_axisAngleRotation3Dx.Angle; }
            set { m_axisAngleRotation3Dx.Angle = value; }
        }

        public double RotationY
        {
            get { return m_axisAngleRotation3Dy.Angle; }
            set { m_axisAngleRotation3Dy.Angle = value; }
        }

        public double RotationZ
        {
            get { return m_axisAngleRotation3Dz.Angle; }
            set { m_axisAngleRotation3Dz.Angle = value; }
        }

        public AGeometryModel3D Geometry { get; private set; }

        #endregion

        #region Fields

        private readonly TranslateTransform3D m_translateTransform3D;
        private readonly ScaleTransform3D m_scaleTransform3D;
        private readonly AxisAngleRotation3D m_axisAngleRotation3Dx;
        private readonly AxisAngleRotation3D m_axisAngleRotation3Dy;
        private readonly AxisAngleRotation3D m_axisAngleRotation3Dz;

        private readonly RotateTransform3D m_rotX;
        private readonly RotateTransform3D m_rotY;
        private readonly RotateTransform3D m_rotZ;

        #endregion
    }
}