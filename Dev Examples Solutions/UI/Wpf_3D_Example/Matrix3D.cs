#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#endregion

namespace Wpf_3D_Example
{
    /// <summary>
    /// Matrix of 3x3 (rows,columns)
    /// </summary>
    public class Matrix3D
    {
        #region Constructors

        public Matrix3D()
        {
            m_data = new double[3][];
            m_data[0] = new double[3];
            m_data[1] = new double[3];
            m_data[2] = new double[3];
        }

        public Matrix3D(double p_11, double p_12, double p_13, double p_21, double p_22, double p_23, double p_31, double p_32, double p_33)
            : this()
        {
            m_data[0][0] = p_11;
            m_data[0][1] = p_12;
            m_data[0][2] = p_13;
            m_data[1][0] = p_21;
            m_data[1][1] = p_22;
            m_data[1][2] = p_23;
            m_data[2][0] = p_31;
            m_data[2][1] = p_32;
            m_data[2][2] = p_33;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("|{0},{1},{2}|{3},{4},{5}|{6},{7},{8}|",
                m_data[0][0],
                m_data[0][1],
                m_data[0][2],
                m_data[1][0],
                m_data[1][1],
                m_data[1][2],
                m_data[2][0],
                m_data[2][1],
                m_data[2][2]
                );
        }

        #endregion

        #region Public Properties

        public double this[int p_row, int p_column]
        {
            get { return m_data[p_row][p_column]; }
            set { m_data[p_row][p_column] = value; }
        }

        #endregion

        #region Fields

        private readonly double[][] m_data;

        #endregion

        public static Matrix3D operator +(Matrix3D p_matrixA, Matrix3D p_matrixB)
        {
            Matrix3D m = new Matrix3D(
                p_matrixA.m_data[0][0] + p_matrixB.m_data[0][0],
                p_matrixA.m_data[0][1] + p_matrixB.m_data[0][1],
                p_matrixA.m_data[0][2] + p_matrixB.m_data[0][2],
                p_matrixA.m_data[1][0] + p_matrixB.m_data[1][0],
                p_matrixA.m_data[1][1] + p_matrixB.m_data[1][1],
                p_matrixA.m_data[1][2] + p_matrixB.m_data[1][2],
                p_matrixA.m_data[2][0] + p_matrixB.m_data[2][0],
                p_matrixA.m_data[2][1] + p_matrixB.m_data[2][1],
                p_matrixA.m_data[2][2] + p_matrixB.m_data[2][2]
                );
            return m;
        }

        public static Point3D operator *(Matrix3D p_matrix, Point3D p_vector3D)
        {
            Matrix3D matrixFromPoint = new Matrix3D(p_vector3D.X, 0, 0, p_vector3D.Y, 0, 0, p_vector3D.Z, 0, 0);
            Matrix3D matrixForResult = p_matrix*matrixFromPoint;
            Point3D p = new Point3D(matrixForResult[0, 0], matrixForResult[1, 0], matrixForResult[2, 0]);
            return p;
        }

        public static Matrix3D operator *(Matrix3D p_matrix, Matrix3D p_otherMatrix)
        {
            Matrix3D m = new Matrix3D();
            m.m_data[0][0] = p_matrix.m_data[0][0]*p_otherMatrix.m_data[0][0] + p_matrix.m_data[0][1]*p_otherMatrix.m_data[1][0] + p_matrix.m_data[0][2]*p_otherMatrix.m_data[2][0];
            m.m_data[1][0] = p_matrix.m_data[1][0]*p_otherMatrix.m_data[0][0] + p_matrix.m_data[1][1]*p_otherMatrix.m_data[1][0] + p_matrix.m_data[1][2]*p_otherMatrix.m_data[2][0];
            m.m_data[2][0] = p_matrix.m_data[2][0]*p_otherMatrix.m_data[0][0] + p_matrix.m_data[2][1]*p_otherMatrix.m_data[1][0] + p_matrix.m_data[2][2]*p_otherMatrix.m_data[2][0];

            m.m_data[0][1] = p_matrix.m_data[0][0]*p_otherMatrix.m_data[0][1] + p_matrix.m_data[0][1]*p_otherMatrix.m_data[1][1] + p_matrix.m_data[0][2]*p_otherMatrix.m_data[2][1];
            m.m_data[1][1] = p_matrix.m_data[1][0]*p_otherMatrix.m_data[0][1] + p_matrix.m_data[1][1]*p_otherMatrix.m_data[1][1] + p_matrix.m_data[1][2]*p_otherMatrix.m_data[2][1];
            m.m_data[2][1] = p_matrix.m_data[2][0]*p_otherMatrix.m_data[0][1] + p_matrix.m_data[2][1]*p_otherMatrix.m_data[1][1] + p_matrix.m_data[2][2]*p_otherMatrix.m_data[2][1];

            m.m_data[0][2] = p_matrix.m_data[0][0]*p_otherMatrix.m_data[0][2] + p_matrix.m_data[0][1]*p_otherMatrix.m_data[1][2] + p_matrix.m_data[0][2]*p_otherMatrix.m_data[2][2];
            m.m_data[1][2] = p_matrix.m_data[1][0]*p_otherMatrix.m_data[0][2] + p_matrix.m_data[1][1]*p_otherMatrix.m_data[1][2] + p_matrix.m_data[1][2]*p_otherMatrix.m_data[2][2];
            m.m_data[2][2] = p_matrix.m_data[2][0]*p_otherMatrix.m_data[0][2] + p_matrix.m_data[2][1]*p_otherMatrix.m_data[1][2] + p_matrix.m_data[2][2]*p_otherMatrix.m_data[2][2];
            return m;
        }
    }
}