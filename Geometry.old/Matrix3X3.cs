using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace Geometry
{
    public class Matrix3X3 : IMatrix3D
    {
        #region Constructors
        public Matrix3X3()
        {
            Data = new float[3, 3];
        }

        public Matrix3X3(Matrix3X3 OtherMatrix) //copy constructor
        {
            Data = new float[3, 3];
            this.Copy(OtherMatrix);
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


        #region General object methods
        public object Clone()
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = Data[0, 0];
            NewMat.Data[0, 1] = Data[0, 1];
            NewMat.Data[0, 2] = Data[0, 2];
            NewMat.Data[1, 0] = Data[1, 0];
            NewMat.Data[1, 1] = Data[1, 1];
            NewMat.Data[1, 2] = Data[1, 2];
            NewMat.Data[2, 0] = Data[2, 0];
            NewMat.Data[2, 1] = Data[2, 1];
            NewMat.Data[2, 2] = Data[2, 2];
            return (object)NewMat;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
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
            else if (this.Rows != m2.Rows)
            {
                return false;
            }
            else if (this.Columns != m2.Columns)
            {
                return false;
            }
            //else have to check values
            for (int row = 0; row < this.Rows; row++)
            {
                for (int col = 0; col < this.Columns; col++)
                {
                    if (Math.Abs(this.Data[row, col] - m2.Data[row, col]) > 1e-5)
                        return false;
                }
            }
            //else
            return true;
        }
        #endregion

        #region Math related methods
        public void SetIdentity()
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0;
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1;
        }

        public void SetZeros()
        {
            Data[0, 0] = 0; Data[0, 1] = 0; Data[0, 2] = 0;
            Data[1, 0] = 0; Data[1, 1] = 0; Data[1, 2] = 0;
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 0;
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

            if (Math.Abs(Data[1, 0]) < tolerance)
                Data[1, 0] = 0;
            if (Math.Abs(Data[1, 1]) < tolerance)
                Data[1, 1] = 0;
            if (Math.Abs(Data[1, 2]) < tolerance)
                Data[1, 2] = 0;

            if (Math.Abs(Data[2, 0]) < tolerance)
                Data[2, 0] = 0;
            if (Math.Abs(Data[2, 1]) < tolerance)
                Data[2, 1] = 0;
            if (Math.Abs(Data[2, 2]) < tolerance)
                Data[2, 2] = 0;
        }

        public float DiagonalProduct()
        {
            return Data[0, 0] * Data[1, 1] * Data[2, 2];
        }

        //Can only be done in Square Matricies unless you allow for arbitary rows and columns which is not yet supported
        public void SetTranspose()
        {
            IMatrix3D Transpose = this.GetTranspose();
            this.Copy(Transpose);
        }
        public IMatrix3D GetTranspose()
        {
            IMatrix3D NewGuy = new Matrix3X3();
            int row, col; //STORE THIS 'MAT' TO NewGuy MATRIX w/ rows and columns reversed
            for (row = 0; row < this.Rows; row++)
                for (col = 0; col < this.Columns; col++)
                    NewGuy.Data[row, col] = Data[col, row];

            return NewGuy;
        }
        #endregion

        //Sadly static methods, including operators cannot be inherited or specified in Interfaces
        #region Math operators
        public static bool operator ==(Matrix3X3 m1, Matrix3X3 m2)
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

        public static bool operator !=(Matrix3X3 m1, Matrix3X3 m2)
        {
            return !(m1 == m2);
        }


        public static Matrix3X3 operator *(Matrix3X3 A, Matrix3X3 B)
        {
            Matrix3X3 C = new Matrix3X3(); //ACCELERATED W/ OUT LOOPS
            C.Data[0, 0] = A.Data[0, 0] * B.Data[0, 0] + A.Data[0, 1] * B.Data[1, 0] + A.Data[0, 2] * B.Data[2, 0] ;
            C.Data[0, 1] = A.Data[0, 0] * B.Data[0, 1] + A.Data[0, 1] * B.Data[1, 1] + A.Data[0, 2] * B.Data[2, 1] ;
            C.Data[0, 2] = A.Data[0, 0] * B.Data[0, 2] + A.Data[0, 1] * B.Data[1, 2] + A.Data[0, 2] * B.Data[2, 2] ;

            C.Data[1, 0] = A.Data[1, 0] * B.Data[0, 0] + A.Data[1, 1] * B.Data[1, 0] + A.Data[1, 2] * B.Data[2, 0] ;
            C.Data[1, 1] = A.Data[1, 0] * B.Data[0, 1] + A.Data[1, 1] * B.Data[1, 1] + A.Data[1, 2] * B.Data[2, 1] ;
            C.Data[1, 2] = A.Data[1, 0] * B.Data[0, 2] + A.Data[1, 1] * B.Data[1, 2] + A.Data[1, 2] * B.Data[2, 2] ;

            C.Data[2, 0] = A.Data[2, 0] * B.Data[0, 0] + A.Data[2, 1] * B.Data[1, 0] + A.Data[2, 2] * B.Data[2, 0] ;
            C.Data[2, 1] = A.Data[2, 0] * B.Data[0, 1] + A.Data[2, 1] * B.Data[1, 1] + A.Data[2, 2] * B.Data[2, 1] ;
            C.Data[2, 2] = A.Data[2, 0] * B.Data[0, 2] + A.Data[2, 1] * B.Data[1, 2] + A.Data[2, 2] * B.Data[2, 2] ;
            return C;
        }


        public static Matrix3X3 operator *(float coeff, Matrix3X3 A)
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = coeff * A.Data[0, 0];
            NewMat.Data[0, 1] = coeff * A.Data[0, 1];
            NewMat.Data[0, 2] = coeff * A.Data[0, 2];
            NewMat.Data[1, 0] = coeff * A.Data[1, 0];
            NewMat.Data[1, 1] = coeff * A.Data[1, 1];
            NewMat.Data[1, 2] = coeff * A.Data[1, 2];
            NewMat.Data[2, 0] = coeff * A.Data[2, 0];
            NewMat.Data[2, 1] = coeff * A.Data[2, 1];
            NewMat.Data[2, 2] = coeff * A.Data[2, 2];
            return NewMat;
        }

        public static Matrix3X3 operator *(Matrix3X3 A, float coeff) //allow reverse order
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = coeff * A.Data[0, 0];
            NewMat.Data[0, 1] = coeff * A.Data[0, 1];
            NewMat.Data[0, 2] = coeff * A.Data[0, 2];
            NewMat.Data[1, 0] = coeff * A.Data[1, 0];
            NewMat.Data[1, 1] = coeff * A.Data[1, 1];
            NewMat.Data[1, 2] = coeff * A.Data[1, 2];
            NewMat.Data[2, 0] = coeff * A.Data[2, 0];
            NewMat.Data[2, 1] = coeff * A.Data[2, 1];
            NewMat.Data[2, 2] = coeff * A.Data[2, 2];
            return NewMat;
        }

        public static Matrix3X3 operator +(Matrix3X3 A, Matrix3X3 B)
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = A.Data[0, 0] + B.Data[0, 0];
            NewMat.Data[0, 1] = A.Data[0, 1] + B.Data[0, 1];
            NewMat.Data[0, 2] = A.Data[0, 2] + B.Data[0, 2];
            NewMat.Data[1, 0] = A.Data[1, 0] + B.Data[1, 0];
            NewMat.Data[1, 1] = A.Data[1, 1] + B.Data[1, 1];
            NewMat.Data[1, 2] = A.Data[1, 2] + B.Data[1, 2];
            NewMat.Data[2, 0] = A.Data[2, 0] + B.Data[2, 0];
            NewMat.Data[2, 1] = A.Data[2, 1] + B.Data[2, 1];
            NewMat.Data[2, 2] = A.Data[2, 2] + B.Data[2, 2];
            return NewMat;
        }

        public static Matrix3X3 operator -(Matrix3X3 A, Matrix3X3 B)
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = A.Data[0, 0] - B.Data[0, 0];
            NewMat.Data[0, 1] = A.Data[0, 1] - B.Data[0, 1];
            NewMat.Data[0, 2] = A.Data[0, 2] - B.Data[0, 2];
            NewMat.Data[1, 0] = A.Data[1, 0] - B.Data[1, 0];
            NewMat.Data[1, 1] = A.Data[1, 1] - B.Data[1, 1];
            NewMat.Data[1, 2] = A.Data[1, 2] - B.Data[1, 2];
            NewMat.Data[2, 0] = A.Data[2, 0] - B.Data[2, 0];
            NewMat.Data[2, 1] = A.Data[2, 1] - B.Data[2, 1];
            NewMat.Data[2, 2] = A.Data[2, 2] - B.Data[2, 2];
            return NewMat;
        }

        public static Matrix3X3 operator -(Matrix3X3 B)
        {
            Matrix3X3 NewMat = new Matrix3X3();
            NewMat.Data[0, 0] = -B.Data[0, 0];
            NewMat.Data[0, 1] = -B.Data[0, 1];
            NewMat.Data[0, 2] = -B.Data[0, 2];
            NewMat.Data[1, 0] = -B.Data[1, 0];
            NewMat.Data[1, 1] = -B.Data[1, 1];
            NewMat.Data[1, 2] = -B.Data[1, 2];
            NewMat.Data[2, 0] = -B.Data[2, 0];
            NewMat.Data[2, 1] = -B.Data[2, 1];
            NewMat.Data[2, 2] = -B.Data[2, 2];
            return NewMat;
        }
        #endregion

        #region Graphics methods
        public void SetScale(float SX, float SY, float SZ)
        {
            Data[0, 0] = SX; Data[0, 1] = 0; Data[0, 2] = 0; 
            Data[1, 0] = 0; Data[1, 1] = SY; Data[1, 2] = 0; 
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = SZ; 
        }


        public void SetRotationX(float radians) //Rotate AROUND x-Axis
        {
            Data[0, 0] = 1; Data[0, 1] = 0; Data[0, 2] = 0;  //X identity
            Data[1, 0] = 0; Data[1, 1] = (float)Math.Cos(radians); Data[1, 2] = (float)-Math.Sin(radians);
            Data[2, 0] = 0; Data[2, 1] = (float)Math.Sin(radians); Data[2, 2] = (float)Math.Cos(radians); 
        }

        public void SetRotationY(float radians) //Rotate around Y-Axis
        {
            Data[0, 0] = (float)Math.Cos(radians); Data[0, 1] = 0; Data[0, 2] = (float)Math.Sin(radians); 
            Data[1, 0] = 0; Data[1, 1] = 1; Data[1, 2] = 0;  //Y identity
            Data[2, 0] = (float)-Math.Sin(radians); Data[2, 1] = 0; Data[2, 2] = (float)Math.Cos(radians); 
        }

        public void SetRotationZ(float radians) // Rotate around Z-Axis
        {
            Data[0, 0] = (float)Math.Cos(radians); Data[0, 1] = (float)-Math.Sin(radians); Data[0, 2] = 0; 
            Data[1, 0] = (float)Math.Sin(radians); Data[1, 1] = (float)Math.Cos(radians); Data[1, 2] = 0; 
            Data[2, 0] = 0; Data[2, 1] = 0; Data[2, 2] = 1; //Z identity
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
            S.Data[0, 0] = 0;
            S.Data[0, 1] = -axisUnitVector.Z;
            S.Data[0, 2] = axisUnitVector.Y;
            S.Data[1, 0] = axisUnitVector.Z;
            S.Data[1, 1] = 0;
            S.Data[1, 2] = -axisUnitVector.X;
            S.Data[2, 0] = -axisUnitVector.Y;
            S.Data[2, 1] = axisUnitVector.X;
            S.Data[2, 2] = 0;

            Matrix3X3 outter = (Matrix3X3)Vector3H1.OutterProduct(axisUnitVector, axisUnitVector);
            Matrix3X3 Id = new Matrix3X3();
            Id.SetIdentity();
            Matrix3X3 Result = outter + ((float)Math.Cos(radians)) * (Id - outter) + ((float)Math.Sin(radians)) * S;

            //Manually copy for maximum speed
            this.Data[0, 0] = Result.Data[0, 0]; this.Data[0, 1] = Result.Data[0, 1]; this.Data[0, 2] = Result.Data[0, 2];
            this.Data[1, 0] = Result.Data[1, 0]; this.Data[1, 1] = Result.Data[1, 1]; this.Data[1, 2] = Result.Data[1, 2];
            this.Data[2, 0] = Result.Data[2, 0]; this.Data[2, 1] = Result.Data[2, 1]; this.Data[2, 2] = Result.Data[2, 2];
        }
        #endregion

    }
}
