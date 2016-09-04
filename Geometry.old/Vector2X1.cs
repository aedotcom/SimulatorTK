using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Geometry
{
    //This vector could be used for TextureCoordinates, but the math operators might be useful for those
    //It could also be used for 2D math graphing, although speed is not necessary for that, so a 
    //generic matrix & generic vector class could be used for that...
    public class Vector2X1 : ICloneable
    {
        public float X;
        public float Y;

        public Vector2X1()
        {
            X = 0; Y = 0;
        }

        public Vector2X1(float x, float y)
        {
            SetXYZ(x, y);
        }

        public Vector2X1(Vector2X1 oldVector)
        {
            X = oldVector.X;
            Y = oldVector.Y;
        }

        public Vector2X1(float[] array)
        {
            X = array[0];
            Y = array[1];
        }

        public void SetXYZ(float x, float y)
        {
            X = x;
            Y = y;
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

            // If parameter cannot be cast to Vector2X1 return false.
            Vector2X1 v2 = obj as Vector2X1;
            if ((System.Object)v2 == null)
            {
                return false;
            }

            //gotta test for NaN
            if (float.IsNaN(v2.X))
                return false;

            if (float.IsNaN(v2.Y))
                return false;

            //else have to check values
            if (Math.Abs(this.X - v2.X) > 1e-5)
                return false;

            if (Math.Abs(this.Y - v2.Y) > 1e-5)
                return false;
            //else
            return true;
        }


        #region math operators
        public static bool operator ==(Vector2X1 v1, Vector2X1 v2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(v1, v2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)v1 == null) ^ ((object)v2 == null))
            {
                return false;
            }

            //gotta test for NaN
            if (float.IsNaN(v1.X))
                return false;

            if (float.IsNaN(v1.Y))
                return false;

            if (float.IsNaN(v2.X))
                return false;

            if (float.IsNaN(v2.Y))
                return false;


            //else have to check values
            if (Math.Abs(v1.X - v2.X) > 1e-5)
                return false;

            if (Math.Abs(v1.Y - v2.Y) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not need comparison
            //else
            return true;
        }

        public static bool operator ==(Vector2X1 v1, float[] v2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(v1, v2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)v1 == null) ^ ((object)v2 == null))
            {
                return false;
            }

            //gotta test for NaN
            if (float.IsNaN(v1.X))
                return false;

            if (float.IsNaN(v1.Y))
                return false;

            if (float.IsNaN(v2[0]))
                return false;

            if (float.IsNaN(v2[1]))
                return false;

            //else have to check values
            if (Math.Abs(v1.X - v2[0]) > 1e-5)
                return false;

            if (Math.Abs(v1.Y - v2[1]) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not neec comparison
            //else
            return true;
        }

        public static bool operator !=(Vector2X1 v1, float[] v2)
        {
            return !(v1 == v2);
        }


        public static bool operator ==(float[] v1, Vector2X1 v2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(v1, v2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)v1 == null) ^ ((object)v2 == null))
            {
                return false;
            }

            //gotta test for NaN
            if (float.IsNaN(v1[0]))
                return false;

            if (float.IsNaN(v1[1]))
                return false;

            if (float.IsNaN(v2.X))
                return false;

            if (float.IsNaN(v2.Y))
                return false;

            //else have to check values
            if (Math.Abs(v2.X - v1[0]) > 1e-5)
                return false;

            if (Math.Abs(v2.Y - v1[1]) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not neec comparison
            //else
            return true;
        }

        public static bool operator !=(float[] v1, Vector2X1 v2)
        {
            return !(v1 == v2);
        }


        public static bool operator !=(Vector2X1 v1, Vector2X1 v2)
        {
            return !(v1 == v2);
        }

        public static Vector2X1 operator +(Vector2X1 v1, Vector2X1 v2)
        {
            return new Vector2X1 { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static Vector2X1 operator -(Vector2X1 v1, Vector2X1 v2)
        {
            return new Vector2X1 { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }

        public static Vector2X1 operator -(Vector2X1 v2)
        {
            return new Vector2X1 { X = -v2.X, Y = -v2.Y };
        }

        public static float operator *(Vector2X1 U, Vector2X1 V)
        {
            return DotProduct(U, V);
        }

        public static Vector2X1 operator *(float coeff, Vector2X1 V)
        {
            return new Vector2X1 { X = coeff * V.X, Y = coeff * V.Y };
        }

        public static Vector2X1 operator /(Vector2X1 V, float coeff)
        {
            return new Vector2X1 { X = V.X / coeff, Y = V.Y / coeff };
        }


        //Let's j ust say if this vector is multiplied by a matrix it just uses the upper left 4 elements regardless of how big the matrix is
        public static Vector2X1 operator *(IMatrix M, Vector2X1 V)
        {
            Vector2X1 NewVect = new Vector2X1();
            NewVect.X = M.Data[0, 0] * V.X + M.Data[0, 1] * V.Y;
            NewVect.Y = M.Data[1, 0] * V.X + M.Data[1, 1] * V.Y;
            return NewVect;
        }
        #endregion


        #region static math functions for the whole class
        public static double Distance(Vector2X1 v1, Vector2X1 v2)  //EX: Vector2X1.Distance(Vect1, Vect2); 
        {
            return Math.Sqrt((v2.X - v1.X) * (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y));
        }


        public static float DotProduct(Vector2X1 U, Vector2X1 V)
        {
            return (U.X * V.X + U.Y * V.Y);
        }

        #endregion




        #region instance methods
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }


        public Vector2X1 GetUnitVector()
        {
            float magnitude = (float)this.Magnitude();
            return new Vector2X1 { X = this.X / magnitude, Y = this.Y / magnitude };
        }

        public void SetUnitVector()
        {
            //this = this.GetUnitVector() //sadly C# won't allow this
            double magnitude = this.Magnitude();
            X = (float)(X / magnitude);
            Y = (float)(Y / magnitude);
        }

        public static Matrix3X3 OutterProduct(Vector2X1 A, Vector2X1 B)
        {
            Matrix3X3 NewMatrix = new Matrix3X3();
            NewMatrix.SetZeros();
            NewMatrix.Data[0, 0] = A.X * B.X;
            NewMatrix.Data[0, 1] = A.X * B.Y;
            NewMatrix.Data[1, 0] = A.Y * B.X;
            NewMatrix.Data[1, 1] = A.Y * B.Y;
            return NewMatrix;
        }


        public object Clone()
        {
            return this.MemberwiseClone(); //should be ok b/c this is a "shallow object" that contains no sub objects
        }
        #endregion

    }
}
