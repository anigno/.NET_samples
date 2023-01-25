#region

using System;
using System.Reactive.Subjects;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#endregion

namespace Wpf_3D_Example.Elements
{
    public class Cube : Arrow
    {
        #region Constructors

        public Cube()
        {
            Geometry.Clear();
            Geometry.AddSquere(new[] {new Point3D(-20, -10, -10), new Point3D(10, -10, -10), new Point3D(10, 10, -10), new Point3D(-20, 10, -10)});
            Geometry.AddSquere(new[] {new Point3D(-20, -10, 10), new Point3D(10, -10, 10), new Point3D(10, 10, 10), new Point3D(-20, 10, 10)});
            Geometry.AddSquere(new[] {new Point3D(-20, -10, -10), new Point3D(10, -10, -10), new Point3D(10, -10, 10), new Point3D(-20, -10, 10)});
            Geometry.AddSquere(new[] {new Point3D(10, -10, -10), new Point3D(10, 10, -10), new Point3D(10, 10, 10), new Point3D(10, -10, 10)});
            Geometry.AddSquere(new[] {new Point3D(10, 10, -10), new Point3D(-20, 10, -10), new Point3D(-20, 10, 10), new Point3D(10, 10, 10)});
            Geometry.AddSquere(new[] {new Point3D(-20, 10, -10), new Point3D(-20, -10, -10), new Point3D(-20, -10, 10), new Point3D(-20, 10, 10)});
            Geometry.Material = new DiffuseMaterial(new RadialGradientBrush(Colors.Yellow, Colors.Blue));
        }

        #endregion

        #region Public Methods

        public void PitchInc(double p_val)
        {
            double rz = RotationZ;
            double ry = RotationY;
            RotationZ = 0;
            RotationY = 0;
            RotationX += p_val;
            Pitch += p_val;
            //RotationZ = rz;
            //RotationY = ry;
        }

        public void PitchDec(double p_val)
        {
        }

        public void RollInc(double p_val)
        {
            double rz = RotationZ;
            double rx = RotationX;
            RotationZ = 0;
            //RotationX = 0;
            RotationY += p_val;
            Roll += p_val;
            RotationZ = rz;
            //RotationX = rx;
        }

        public void RollDec(double p_val)
        {
        }

        public void YawInc(double p_val)
        {
            double ry = RotationY;
            double rx = RotationX;
            RotationY = 0;
            RotationX = 0;
            RotationZ += p_val;
            Yaw += p_val;
            //RotationY = ry;
            //RotationX = rx;
        }

        public void YawDec(double p_val)
        {
        }


        public static double CosA(double p_angle)
        {
            p_angle = Math.Abs(p_angle) < 0.001 ? 0.001 : p_angle;
            return Math.Cos(p_angle/57.3);
        }

        public static double SinA(double p_angle)
        {
            p_angle = Math.Abs(p_angle) < 0.001 ? 0.001 : p_angle;
            return Math.Sin(p_angle/57.3);
        }

        #endregion

        #region Public Properties

        public double Pitch { get; private set; }

        public double Yaw { get; private set; }

        public double Roll { get; private set; }

        #endregion

        //private void setRotation2()
        //{
        //    double cq = CosA(Yaw);
        //    double sq = SinA(Yaw);
        //    double cy = CosA(Pitch);
        //    double sy = SinA(Pitch);
        //    double co = CosA(Roll);
        //    double so = SinA(Roll);
        //    Matrix3D m = new Matrix3D(
        //        cq * cy - sq * co * sy, -cq * sy - sq * co * cy, sq * so,
        //        sq * cy + cq * co * sy, -sq * sy + cq * co * cy, -cq * so,
        //        so * sy, so * cy, co
        //        );
        //    Point3D ypr = new Point3D(Yaw, Roll, Pitch);
        //    Point3D xyz = m * ypr;
        //    OnRotatedPry.OnNext(new Vector3D(xyz.X, xyz.Y, xyz.Z));
        //    //RotationX = xyz.X;
        //    //RotationY = xyz.Y;
        //    //RotationZ = xyz.Z;
        //}

        #region Private Methods

        private void setXyzToRpy()
        {
            double cy = CosA(Roll);
            double sy = SinA(Roll);
            double co = CosA(Pitch);
            double so = SinA(Pitch);
            double cq = CosA(Yaw);
            double sq = SinA(Yaw);
            Matrix3D m = new Matrix3D(
                cq*co, cq*so*sy - sq*cy, cq*so*cy + sq*sy,
                sq*co, sq*so*sy + cq*cy, sq*so*cy - cq*sy,
                -so, co*sy, co*cq
                );
            Point3D xyz = new Point3D(RotationX, RotationY, RotationZ);
            Point3D rpy = m*xyz;

            double p = Math.Asin(-m[2, 1]);
            double r = Math.Atan2(m[0, 1], m[1, 1]);
            double y = Math.Atan2(m[2, 0], m[2, 2]);

            OnRotatedPry.OnNext(new[] {new Vector3D(rpy.X, rpy.Y, rpy.Z), new Vector3D(p, r, y)});
        }

        #endregion

        #region Fields

        public Subject<Vector3D[]> OnRotatedPry = new Subject<Vector3D[]>();

        #endregion
    }
}