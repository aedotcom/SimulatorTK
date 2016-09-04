using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Geometry
{
    public class Matrix4X4 : IMatrix4D
    {
        #region Constructors
        public Matrix4X4() 
        {
            Data = new float[4, 4];  //in C# the values are 0 by default
        }

        public Matrix4X4(Matrix4X4 OtherMatrix) //copy constructor
        {
            Data = new float[4, 4];
            this.Copy(OtherMatrix);
        }

        public Matrix4X4(float[,] array) //array copy constructor //assumes exactly 4x4 array,, hmm....
        {
            Data = new float[4, 4];
            this.Copy(array);
        }

        public Matrix4X4(IMatrix3D OtherMatrix) //copy constructor
        {
            Data = new float[4, 4];
            this.Copy(OtherMatrix);
            Data[3, 3] = 1; //homogenous
        }
        #endregion

        #region matrix properties
        public float[,] Data { get; set; }
        public int Rows
        {
            get
            {
                return Data.GetLength(0);
            }
        }
        public int Columns
        {
            get
            {
                return Data.GetLength(1);
            }
        }
        #endregion

        #region general object methods
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            Matrix4X4 MyClone = new Matrix4X4();
            MyClone.Data[0, 0] = Data[0, 0];  // copy the elements
            MyClone.Data[0, 1] = Data[0, 1];
            MyClone.Data[0, 2] = Data[0, 2];
            MyClone.Data[0, 3] = Data[0, 3];
            MyClone.Data[1, 0] = Data[1, 0];
            MyClone.Data[1, 1] = Data[1, 1];
            MyClone.Data[1, 2] = Data[1, 2];
            MyClone.Data[1, 3] = Data[1, 3];
            MyClone.Data[2, 0] = Data[2, 0];
            MyClone.Data[2, 1] = Data[2, 1];
            MyClone.Data[2, 2] = Data[2, 2];
            MyClone.Data[2, 3] = Data[2, 3];
            MyClone.Data[3, 0] = Data[3, 0];
            MyClone.Data[3, 1] = Data[3, 1];
            MyClone.Data[3, 2] = Data[3, 2];
            MyClone.Data[3, 3] = Data[3, 3];
            return (object)MyClone;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            // If parameter cannot be cast to MatrixBaseClass return false.
            IMatrix m2 = obj as IMatrix;
            if ((System.Object)m2 == null)
            {
                return false;
            }
            //else if (this.Rows != m2.Rows) //for testing perposes that may not m atter
            //{
            //    return false;
            //}
            //else if (this.Columns != m2.Columns)
            //{
            //    return false;
            //}
            //else have to check values
            for (int row = 0; row < Math.Min(this.Rows, m2.Rows); row++)
            {
                for (int col = 0; col < Math.Min(this.Columns, m2.Columns); col++)
                {
                    if (Math.Abs(this.Data[row, col] - m2.Data[row, col]) > 1e-5)
                        return false;
                }
            }
            //else
            return true;
        }
        #endregion


        #region Math Related Methods
        public void SetIdentity()
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = 0;
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0; Data[1, 3] = 0;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1; Data[2, 3] = 0;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        public void SetZeros()
        {
            Data[0, 0] = 0; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = 0;
            Data[1, 0] = 0; Data[1, 1] = 0; Data[1, 2] = 0; Data[1, 3] = 0;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 0; Data[2, 3] = 0;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 0;
        }

        public void SetRandom()
        {
            for (int row = 0; row < this.Rows; row++)
            {
                for (int col = 0; col < this.Columns; col++)
                {
                    Data[row, col] = Geometry.Helper.RandomFloat();
                }
            }
        }

        public void CheckForZeros(float tolerance = 1e-5F)
        {
            if (Math.Abs(Data[0, 0]) < tolerance)
                Data[0, 0] = 0;
            if (Math.Abs(Data[0, 1]) < tolerance)
                Data[0, 1] = 0;
            if (Math.Abs(Data[0, 2]) < tolerance)
                Data[0, 2] = 0;
            if (Math.Abs(Data[0, 3]) < tolerance)
                Data[0, 3] = 0;

            if (Math.Abs(Data[1, 0]) < tolerance)
                Data[1, 0] = 0;
            if (Math.Abs(Data[1, 1]) < tolerance)
                Data[1, 1] = 0;
            if (Math.Abs(Data[1, 2]) < tolerance)
                Data[1, 2] = 0;
            if (Math.Abs(Data[1, 3]) < tolerance)
                Data[1, 3] = 0;

            if (Math.Abs(Data[2, 0]) < tolerance)
                Data[2, 0] = 0;
            if (Math.Abs(Data[2, 1]) < tolerance)
                Data[2, 1] = 0;
            if (Math.Abs(Data[2, 2]) < tolerance)
                Data[2, 2] = 0;
            if (Math.Abs(Data[2, 3]) < tolerance)
                Data[2, 3] = 0;

            if (Math.Abs(Data[3, 0]) < tolerance)
                Data[3, 0] = 0;
            if (Math.Abs(Data[3, 1]) < tolerance)
                Data[3, 1] = 0;
            if (Math.Abs(Data[3, 2]) < tolerance)
                Data[3, 2] = 0;
            if (Math.Abs(Data[3, 3]) < tolerance)
                Data[3, 3] = 0;
        }


        //This index property lets you get/set an entire row as a float[4] array
        //Because ths is managed code, if you try to access a row out of bounds
        //(less than 0 or greater than 3), it automatically throws an exception 
        //w/out having to test the row varaible & manually throw an exception
        public float[] this[int row]
        {
            get
            {
                float[] rowvector = new float[4];
                rowvector[0] = Data[row, 0];
                rowvector[1] = Data[row, 1];
                rowvector[2] = Data[row, 2];
                rowvector[3] = Data[row, 3];
                return rowvector;
            }
            set
            {
                Data[row, 0] = value[0];
                Data[row, 1] = value[1];
                Data[row, 2] = value[2];
                Data[row, 3] = value[3];
            }
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //may not work in .NET 4.0
        public void SetRow(int row, Vector3H1 V, float W = 0)
        {
            Data[row, 0] = V.X;
            Data[row, 1] = V.Y;
            Data[row, 2] = V.Z;
            Data[row, 3] = W;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //may not work in .NET 4.0
        public void SetColumn(int col, Vector3H1 V, float W = 0)
        {
            Data[0, col] = V.X;
            Data[1, col] = V.Y;
            Data[2, col] = V.Z;
            Data[3, col] = W;
        }

        //Can only be done in Square Matricies unless you allow for generic sizes which are not yet supported
        public void SetTranspose()
        {
            IMatrix4D Transpose = this.GetTranspose();
            this.Copy(Transpose);
        }
        public IMatrix4D GetTranspose()
        {
            IMatrix4D NewGuy = new Matrix4X4();
            int row, col; //STORE THIS 'MAT' TO NewGuy MATRIX w/ rows and columns reversed
            for (row = 0; row < this.Rows; row++)
                for (col = 0; col < this.Columns; col++)
                    NewGuy.Data[row, col] = Data[col, row];

            return NewGuy;
        }


        //maybe copy lower & uper should be the helper file using the base clase imlicit cast...
        public void CopyLower(Matrix4X4 M)
        {
            Data[0, 0] = M.Data[0, 0];
            Data[1, 0] = M.Data[1, 0]; Data[1, 1] = M.Data[1, 1];
            Data[2, 0] = M.Data[2, 0]; Data[2, 1] = M.Data[2, 1]; Data[2, 2] = M.Data[2, 2];
            Data[3, 0] = M.Data[3, 0]; Data[3, 1] = M.Data[3, 1]; Data[3, 2] = M.Data[3, 2]; Data[3, 3] = M.Data[3, 3];
        }

        public void CopyUpper(Matrix4X4 M)
        {
            Data[0, 0] = M.Data[0, 0]; Data[0, 1] = M.Data[0, 1]; Data[0, 2] = M.Data[0, 2]; Data[0, 3] = M.Data[0, 3];
            Data[1, 1] = M.Data[1, 1]; Data[1, 2] = M.Data[1, 2]; Data[1, 3] = M.Data[1, 3];
            Data[2, 2] = M.Data[2, 2]; Data[2, 3] = M.Data[2, 3];
            Data[3, 3] = M.Data[3, 3];
        }

        public void setDiagonal(float d0, float d1, float d2, float d3)
        {
            Data[0, 0] = d0;
            Data[1, 1] = d1;
            Data[2, 2] = d2;
            Data[3, 3] = d3;
        }

        public float DiagonalProduct()
        {
            return Data[0, 0] * Data[1, 1] * Data[2, 2] * Data[3, 3];
        }

        #endregion


        #region Math Operators
        public static bool operator ==(Matrix4X4 m1, Matrix4X4 m2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(m1, m2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)m1 == null) ^ ((object)m2 == null))
            {
                return false;
            }

            //else have to check values
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix4X4 m1, Matrix4X4 m2)
        {
            return !(m1 == m2);
        }


        public static bool operator ==(Matrix4X4 m1, Matrix3H4 m2)
        {
            // If one is null, but not both, return false.
            if (((object)m1 == null) ^ ((object)m2 == null))
            {
                return false;
            }

            //else have to check values
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix4X4 m1, Matrix3H4 m2)
        {
            return !(m1 == m2);
        }

        public static bool operator ==(Matrix3H4 m1,  Matrix4X4 m2)
        {
            // If one is null, but not both, return false.
            if (((object)m1 == null) ^ ((object)m2 == null))
            {
                return false;
            }

            //else have to check values
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix3H4 m1, Matrix4X4 m2)
        {
            return !(m1 == m2);
        }

        public static bool operator ==(Matrix3X3 m1, Matrix4X4 m2)
        {
            // If one is null, but not both, return false.
            if (((object)m1 == null) ^ ((object)m2 == null))
            {
                return false;
            }

            //else have to check values
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix3X3 m1, Matrix4X4 m2)
        {
            return !(m1 == m2);
        }


        public static bool operator ==(Matrix4X4 m1, Matrix3X3 m2)
        {
            // If one is null, but not both, return false.
            if (((object)m1 == null) ^ ((object)m2 == null))
            {
                return false;
            }

            //else have to check values
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix4X4 m1, Matrix3X3 m2)
        {
            return !(m1 == m2);
        }


        public static Matrix4X4 operator *(Matrix4X4 A, Matrix4X4 B)
        {
            Matrix4X4 C = new Matrix4X4(); //ACCELERATED W/ OUT LOOPS
	        C.Data[0,0] = A.Data[0,0]*B.Data[0,0]+A.Data[0,1]*B.Data[1,0]+A.Data[0,2]*B.Data[2,0]+A.Data[0,3]*B.Data[3,0];
	        C.Data[0,1] = A.Data[0,0]*B.Data[0,1]+A.Data[0,1]*B.Data[1,1]+A.Data[0,2]*B.Data[2,1]+A.Data[0,3]*B.Data[3,1];
	        C.Data[0,2] = A.Data[0,0]*B.Data[0,2]+A.Data[0,1]*B.Data[1,2]+A.Data[0,2]*B.Data[2,2]+A.Data[0,3]*B.Data[3,2];
	        C.Data[0,3] = A.Data[0,0]*B.Data[0,3]+A.Data[0,1]*B.Data[1,3]+A.Data[0,2]*B.Data[2,3]+A.Data[0,3]*B.Data[3,3];

	        C.Data[1,0] = A.Data[1,0]*B.Data[0,0]+A.Data[1,1]*B.Data[1,0]+A.Data[1,2]*B.Data[2,0]+A.Data[1,3]*B.Data[3,0];
	        C.Data[1,1] = A.Data[1,0]*B.Data[0,1]+A.Data[1,1]*B.Data[1,1]+A.Data[1,2]*B.Data[2,1]+A.Data[1,3]*B.Data[3,1];
	        C.Data[1,2] = A.Data[1,0]*B.Data[0,2]+A.Data[1,1]*B.Data[1,2]+A.Data[1,2]*B.Data[2,2]+A.Data[1,3]*B.Data[3,2];
 	        C.Data[1,3] = A.Data[1,0]*B.Data[0,3]+A.Data[1,1]*B.Data[1,3]+A.Data[1,2]*B.Data[2,3]+A.Data[1,3]*B.Data[3,3];

	        C.Data[2,0] = A.Data[2,0]*B.Data[0,0]+A.Data[2,1]*B.Data[1,0]+A.Data[2,2]*B.Data[2,0]+A.Data[2,3]*B.Data[3,0];
	        C.Data[2,1] = A.Data[2,0]*B.Data[0,1]+A.Data[2,1]*B.Data[1,1]+A.Data[2,2]*B.Data[2,1]+A.Data[2,3]*B.Data[3,1];
	        C.Data[2,2] = A.Data[2,0]*B.Data[0,2]+A.Data[2,1]*B.Data[1,2]+A.Data[2,2]*B.Data[2,2]+A.Data[2,3]*B.Data[3,2];
	        C.Data[2,3] = A.Data[2,0]*B.Data[0,3]+A.Data[2,1]*B.Data[1,3]+A.Data[2,2]*B.Data[2,3]+A.Data[2,3]*B.Data[3,3];

	        C.Data[3,0] = A.Data[3,0]*B.Data[0,0]+A.Data[3,1]*B.Data[1,0]+A.Data[3,2]*B.Data[2,0]+A.Data[3,3]*B.Data[3,0];
	        C.Data[3,1] = A.Data[3,0]*B.Data[0,1]+A.Data[3,1]*B.Data[1,1]+A.Data[3,2]*B.Data[2,1]+A.Data[3,3]*B.Data[3,1];
	        C.Data[3,2] = A.Data[3,0]*B.Data[0,2]+A.Data[3,1]*B.Data[1,2]+A.Data[3,2]*B.Data[2,2]+A.Data[3,3]*B.Data[3,2];
	        C.Data[3,3] = A.Data[3,0]*B.Data[0,3]+A.Data[3,1]*B.Data[1,3]+A.Data[3,2]*B.Data[2,3]+A.Data[3,3]*B.Data[3,3];
	        return C;
        }


        public static Matrix4X4 operator *(float coeff, Matrix4X4 A)
        {
            Matrix4X4 NewMat = new Matrix4X4();
	        NewMat.Data[0,0] = coeff * A.Data[0,0] ;
	        NewMat.Data[0,1] = coeff * A.Data[0,1] ;
	        NewMat.Data[0,2] = coeff * A.Data[0,2] ;
	        NewMat.Data[0,3] = coeff * A.Data[0,3] ;
	        NewMat.Data[1,0] = coeff * A.Data[1,0] ;
	        NewMat.Data[1,1] = coeff * A.Data[1,1] ;
	        NewMat.Data[1,2] = coeff * A.Data[1,2] ;
	        NewMat.Data[1,3] = coeff * A.Data[1,3] ;
	        NewMat.Data[2,0] = coeff * A.Data[2,0] ;
	        NewMat.Data[2,1] = coeff * A.Data[2,1] ;
	        NewMat.Data[2,2] = coeff * A.Data[2,2] ;
	        NewMat.Data[2,3] = coeff * A.Data[2,3] ;
	        NewMat.Data[3,0] = coeff * A.Data[3,0] ;
	        NewMat.Data[3,1] = coeff * A.Data[3,1] ;
	        NewMat.Data[3,2] = coeff * A.Data[3,2] ;
	        NewMat.Data[3,3] = coeff * A.Data[3,3] ;
	        return NewMat;
        }

        public static Matrix4X4 operator *(Matrix4X4 A, float coeff) //allow reverse order
        {
            Matrix4X4 NewMat = new Matrix4X4();
	        NewMat.Data[0,0] = coeff * A.Data[0,0] ;
	        NewMat.Data[0,1] = coeff * A.Data[0,1] ;
	        NewMat.Data[0,2] = coeff * A.Data[0,2] ;
	        NewMat.Data[0,3] = coeff * A.Data[0,3] ;
	        NewMat.Data[1,0] = coeff * A.Data[1,0] ;
	        NewMat.Data[1,1] = coeff * A.Data[1,1] ;
	        NewMat.Data[1,2] = coeff * A.Data[1,2] ;
	        NewMat.Data[1,3] = coeff * A.Data[1,3] ;
	        NewMat.Data[2,0] = coeff * A.Data[2,0] ;
	        NewMat.Data[2,1] = coeff * A.Data[2,1] ;
	        NewMat.Data[2,2] = coeff * A.Data[2,2] ;
	        NewMat.Data[2,3] = coeff * A.Data[2,3] ;
	        NewMat.Data[3,0] = coeff * A.Data[3,0] ;
	        NewMat.Data[3,1] = coeff * A.Data[3,1] ;
	        NewMat.Data[3,2] = coeff * A.Data[3,2] ;
	        NewMat.Data[3,3] = coeff * A.Data[3,3] ;
	        return NewMat;
        }

        public static Matrix4X4 operator +(Matrix4X4 A, Matrix4X4 B)
        {
            Matrix4X4 NewMat = new Matrix4X4();
	        NewMat.Data[0,0] = A.Data[0,0] + B.Data[0,0];
	        NewMat.Data[0,1] = A.Data[0,1] + B.Data[0,1];
	        NewMat.Data[0,2] = A.Data[0,2] + B.Data[0,2];
	        NewMat.Data[0,3] = A.Data[0,3] + B.Data[0,3];
	        NewMat.Data[1,0] = A.Data[1,0] + B.Data[1,0];
	        NewMat.Data[1,1] = A.Data[1,1] + B.Data[1,1];
	        NewMat.Data[1,2] = A.Data[1,2] + B.Data[1,2];
	        NewMat.Data[1,3] = A.Data[1,3] + B.Data[1,3];
	        NewMat.Data[2,0] = A.Data[2,0] + B.Data[2,0];
	        NewMat.Data[2,1] = A.Data[2,1] + B.Data[2,1];
	        NewMat.Data[2,2] = A.Data[2,2] + B.Data[2,2];
	        NewMat.Data[2,3] = A.Data[2,3] + B.Data[2,3];
	        NewMat.Data[3,0] = A.Data[3,0] + B.Data[3,0];
	        NewMat.Data[3,1] = A.Data[3,1] + B.Data[3,1];
	        NewMat.Data[3,2] = A.Data[3,2] + B.Data[3,2];
	        NewMat.Data[3,3] = A.Data[3,3] + B.Data[3,3];
	        return NewMat;
        }

        public static Matrix4X4 operator -(Matrix4X4 A, Matrix4X4 B)
        {
            Matrix4X4 NewMat = new Matrix4X4();
	        NewMat.Data[0,0] = A.Data[0,0] - B.Data[0,0];
	        NewMat.Data[0,1] = A.Data[0,1] - B.Data[0,1];
	        NewMat.Data[0,2] = A.Data[0,2] - B.Data[0,2];
	        NewMat.Data[0,3] = A.Data[0,3] - B.Data[0,3];
	        NewMat.Data[1,0] = A.Data[1,0] - B.Data[1,0];
	        NewMat.Data[1,1] = A.Data[1,1] - B.Data[1,1];
	        NewMat.Data[1,2] = A.Data[1,2] - B.Data[1,2];
	        NewMat.Data[1,3] = A.Data[1,3] - B.Data[1,3];
	        NewMat.Data[2,0] = A.Data[2,0] - B.Data[2,0];
	        NewMat.Data[2,1] = A.Data[2,1] - B.Data[2,1];
	        NewMat.Data[2,2] = A.Data[2,2] - B.Data[2,2];
	        NewMat.Data[2,3] = A.Data[2,3] - B.Data[2,3];
	        NewMat.Data[3,0] = A.Data[3,0] - B.Data[3,0];
	        NewMat.Data[3,1] = A.Data[3,1] - B.Data[3,1];
	        NewMat.Data[3,2] = A.Data[3,2] - B.Data[3,2];
	        NewMat.Data[3,3] = A.Data[3,3] - B.Data[3,3];
	        return NewMat;
        }

        public static Matrix4X4 operator -(Matrix4X4 B)
        {
            Matrix4X4 NewMat = new Matrix4X4();
            NewMat.Data[0, 0] = - B.Data[0, 0];
            NewMat.Data[0, 1] = - B.Data[0, 1];
            NewMat.Data[0, 2] = - B.Data[0, 2];
            NewMat.Data[0, 3] = - B.Data[0, 3];
            NewMat.Data[1, 0] = - B.Data[1, 0];
            NewMat.Data[1, 1] = - B.Data[1, 1];
            NewMat.Data[1, 2] = - B.Data[1, 2];
            NewMat.Data[1, 3] = - B.Data[1, 3];
            NewMat.Data[2, 0] = - B.Data[2, 0];
            NewMat.Data[2, 1] = - B.Data[2, 1];
            NewMat.Data[2, 2] = - B.Data[2, 2];
            NewMat.Data[2, 3] = - B.Data[2, 3];
            NewMat.Data[3, 0] = - B.Data[3, 0];
            NewMat.Data[3, 1] = - B.Data[3, 1];
            NewMat.Data[3, 2] = - B.Data[3, 2];
            NewMat.Data[3, 3] = - B.Data[3, 3];
            return NewMat;
        }
        #endregion




        #region Graphics Specific Methods
        /// <summary> //Can only be done in Matrix4X4 b/c it uses the 3rd row
        /// Build a world space to camera space matrix - Based on OpenTK.Matrix4.LookAt
        /// </summary>
        /// <param name="eye">Eye (camera) position in world space</param>
        /// <param name="target">Target position in world space</param>
        /// <param name="unitUpVector">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        public void SetLookAt(Vector3H1 eye, Vector3H1 target, Vector3H1 unitUpVector)
        {
            //unitUpVector.SetUnitVector()
            Vector3H1 z = eye - target;
            z.SetUnitVector();
            Vector3H1 x = Vector3H1.CrossProduct(unitUpVector, z);
            x.SetUnitVector();
            Vector3H1 y = Vector3H1.CrossProduct(z, x);
            y.SetUnitVector();

            Data[0, 0] = x.X;
            Data[0, 1] = y.X;
            Data[0, 2] = z.X;
            Data[0, 3] = 0;

            Data[1, 0] = x.Y;
            Data[1, 1] = y.Y;
            Data[1, 2] = z.Y;
            Data[1, 3] = 0;

            Data[2, 0] = x.Z;
            Data[2, 1] = y.Z;
            Data[2, 2] = z.Z;
            Data[2, 3] = 0;

            Data[3, 0] = -((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z));
            Data[3, 1] = -((y.X * eye.X) + (y.Y * eye.Y) + (y.Z * eye.Z));
            Data[3, 2] = -((z.X * eye.X) + (z.Y * eye.Y) + (z.Z * eye.Z));
            Data[3, 3] = 1;
        }

        public void SetScale(float SX, float SY, float SZ)
        {
            Data[0, 0] = SX; Data[0, 1] = 0;  Data[0, 2] = 0;  Data[0, 3] = 0;
            Data[1, 0] = 0;  Data[1, 1] = SY; Data[1, 2] = 0;  Data[1, 3] = 0;
            Data[2, 0] = 0;  Data[2, 1] = 0;  Data[2, 2] = SZ; Data[2, 3] = 0;
            Data[3, 0] = 0;  Data[3, 1] = 0;  Data[3, 2] = 0;  Data[3, 3] = 1;
        }
            
        public void SetRotationX(float radians) //Rotate AROUND x-Axis
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = 0;  //X identity
            Data[1, 0] = 0; Data[1, 1] = (float)Math.Cos(radians); Data[1, 2] = (float) -Math.Sin(radians); Data[1, 3] = 0;
            Data[2, 0] = 0; Data[2, 1] = (float)Math.Sin(radians); Data[2, 2] = (float)Math.Cos(radians); Data[2, 3] = 0;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        public void SetRotationY(float radians) //Rotate around Y-Axis
        {
            Data[0, 0] = (float)Math.Cos(radians);  Data[0, 1] = 0; Data[0, 2] = (float)Math.Sin(radians); Data[0, 3] = 0;
            Data[1, 0] = 0;                         Data[1, 1] = 1; Data[1, 2] = 0; Data[1, 3] = 0;  //Y identity
            Data[2, 0] = (float)-Math.Sin(radians); Data[2, 1] = 0; Data[2, 2] = (float)Math.Cos(radians); Data[2, 3] = 0;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        public void SetRotationZ(float radians) // Rotate around Z-Axis
        {
            Data[0, 0] = (float)Math.Cos(radians); Data[0, 1] = (float)-Math.Sin(radians); Data[0, 2] = 0; Data[0, 3] = 0;
            Data[1, 0] = (float)Math.Sin(radians); Data[1, 1] = (float)Math.Cos(radians);  Data[1, 2] = 0; Data[1, 3] = 0;
            Data[2, 0] = 0;                        Data[2, 1] = 0; Data[2, 2] = 1;          Data[2, 3] = 0; //Z identity
            Data[3, 0] = 0;                        Data[3, 1] = 0; Data[3, 2] = 0;          Data[3, 3] = 1;
        }



        //I got this method for rotating around an arbitary axis from the 
        //GLspec15.pdf.  (page number 42 in the image, page 55 of the pdf file)
        //IMPORTANT!!!  The angle 'radians' is in the direction of the 
        //"finger curl of the right hand rule" around the vector {x,y,z}
        //------------------------------------------------------------------------------------------
        public void RotateAnyAxis(float radians, IVector3H1 axisUnitVector) //unitize Axis first!!!!!
        {

            //but remember the cross product of 2 unit vectors is only a unit vector if the original 2 vectors are @ 90degrees .....
            ///................... should I go ahead & always run the unit calculation here for safety?
            ///maybe not b/c in FlightSimulator I pretty much always unit-ize the vectors before haand
            //double myAxis[3] = {Axis.vect[0], Axis.vect[1], Axis.vect[2]};
            //int mySize = 3;
	        //UnitVOfArray(myAxis, mySize);

	        Matrix3X3 S = new Matrix3X3();
	        S.Data[0,0] = 0;
	        S.Data[0,1] = - axisUnitVector.Z;
	        S.Data[0,2] = axisUnitVector.Y;
	        S.Data[1,0] = axisUnitVector.Z;
	        S.Data[1,1] = 0;
	        S.Data[1,2] = - axisUnitVector.X;
	        S.Data[2,0] = - axisUnitVector.Y;
	        S.Data[2,1] = axisUnitVector.X;
	        S.Data[2,2] = 0;

	        Matrix3X3 outter = (Matrix3X3)Vector3H1.OutterProduct(axisUnitVector, axisUnitVector); 
	        Matrix3X3 Id = new Matrix3X3();
	        Id.SetIdentity();
	        Matrix3X3 Result= outter + ((float)Math.Cos(radians))*(Id - outter) + ((float)Math.Sin(radians))*S;

            //Manually copy for maximum speed
            this.Data[0, 0] = Result.Data[0, 0]; this.Data[0, 1] = Result.Data[0, 1]; this.Data[0, 2] = Result.Data[0, 2]; this.Data[0, 3] = 0;
            this.Data[1, 0] = Result.Data[1, 0]; this.Data[1, 1] = Result.Data[1, 1]; this.Data[1, 2] = Result.Data[1, 2]; this.Data[1, 3] = 0;
            this.Data[2, 0] = Result.Data[2, 0]; this.Data[2, 1] = Result.Data[2, 1]; this.Data[2, 2] = Result.Data[2, 2]; this.Data[2, 3] = 0;
            this.Data[3, 0] = 0;                 this.Data[3, 1] = 0;                 this.Data[3, 2] = 0;                 this.Data[3, 3] = 1;
        }


        public void SetTranslate(float TX, float TY, float TZ)
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = TX;
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0; Data[1, 3] = TY;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1; Data[2, 3] = TZ;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        public void SetTranslate(float[] move)
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = move[0];
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0; Data[1, 3] = move[1];
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1; Data[2, 3] = move[2];
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        public void SetTranslate(IVector3H1 V)
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0; Data[0, 3] = V.X;
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0; Data[1, 3] = V.Y;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1; Data[2, 3] = V.Z;
            Data[3, 0] = 0; Data[3, 1] = 0; Data[3, 2] = 0; Data[3, 3] = 1;
        }

        //"Spin" = rotate around a relative center of motion :o)
        //----------------------------------------------------------------
        public void Spin(float radians, IVector3H1 axisUnitVector, IVector3H1 RelCenter) 
        {
	        Matrix4X4 Trans1 = new Matrix4X4();
            //translate relative center to oragine
	        Trans1.SetTranslate( -(Vector3H1)RelCenter);  //have to cast to object not interface type to apply operator
            Matrix4X4 RotateMat = new Matrix4X4();
	        RotateMat.RotateAnyAxis(radians, axisUnitVector); //Do 3D rotation
            Matrix4X4 Trans2 = new Matrix4X4();
	        Trans2.SetTranslate(RelCenter); // put it back
	        IMatrix4D Answer = Trans2 * RotateMat * Trans1; //multiply out answer
            this.Copy(Answer);
        }

        //"stretch" = scale around a relative (quasi-center) point
        //basically you move to the origin, do a normal scale, then move the object back
        public void Stretch(float SX, float SY, float SZ, IVector3H1 RelCenter) //probably cannot be done in 3x3... 3H4???
        {
            //translate relative center to oragine
            Matrix4X4 Trans1 = new Matrix4X4();
            Trans1.SetTranslate(-RelCenter.X, -RelCenter.Y, -RelCenter.Z);

            //do the scaling
            Matrix4X4 ScaleMat = new Matrix4X4();
            ScaleMat.SetScale(SX, SY, SZ);

            // put it back
            Matrix4X4 Trans2 = new Matrix4X4();
            Trans2.SetTranslate(RelCenter);

            IMatrix4D Answer = Trans2 * ScaleMat * Trans1;
            this.Copy(Answer);
        }

#endregion



    }
}
