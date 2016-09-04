using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Geometry
{
    //This vector is assumed to have a homogenous coordinate that is always 1
    public class Vector3H1 : IVector3H1, ICloneable, IEquatable<Vector3H1>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        public Vector3H1()
        {
            X = 0; Y = 0; Z = 0;
        }

        public Vector3H1(float x, float y, float z)
        {
            SetXYZ(x, y, z);
        }

        public Vector3H1(Vector3H1 oldVector)
        {
            X = oldVector.X;
            Y = oldVector.Y;
            Z = oldVector.Z;
        }

        public Vector3H1(float[] array)
        {
            X = array[0];
            Y = array[1];
            Z = array[2];
        }

        public void SetXYZ(float[] array)
        {
            X = array[0];
            Y = array[1];
            Z = array[2];
        }

        public void SetXYZ(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void SetRandom()
        {
            X = Geometry.Helper.RandomFloat();
            Y = Geometry.Helper.RandomFloat();
            Z = Geometry.Helper.RandomFloat();
        }

        public void SetZeros()
        {
            X = 0; Y = 0; Z = 0;
        }

        public void CheckForZeros(float tolerance = 1e-5F)
        {
            if (Math.Abs(X) < tolerance)
                X = 0;
            if (Math.Abs(Y) < tolerance)
                Y = 0;
            if (Math.Abs(Z) < tolerance)
                Z = 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //IClonable gives you a warning if you don't have this function signaure
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Vector3H1 return false.
            Vector3H1 v2 = obj as Vector3H1;
            if ((System.Object)v2 == null)
            {
                return false;
            }

            return this.Equals(v2);
        }

        //but IEquatable<T> wants this function signature
        public bool Equals(Vector3H1 other)
        {
            //gotta test for NaN
            if (float.IsNaN(other.X))
                return false;

            if (float.IsNaN(other.Y))
                return false;

            if (float.IsNaN(other.Z))
                return false;

            //else have to check values
            if (Math.Abs(this.X - other.X) > 1e-5)
                return false;

            if (Math.Abs(this.Y - other.Y) > 1e-5)
                return false;

            if (Math.Abs(this.Z - other.Z) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not need comparison
            //else
            return true;
        }


        #region math operators
        public static bool operator ==(Vector3H1 v1, Vector3H1 v2)
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
            //else have to check values
            return v1.Equals(v2);
        }

        public static bool operator ==(Vector3H1 v1, float[] v2)
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

            if (float.IsNaN(v1.Z))
                return false;

            if (float.IsNaN(v2[0]))
                return false;

            if (float.IsNaN(v2[1]))
                return false;

            if (float.IsNaN(v2[2]))
                return false;

            //else have to check values
            if (Math.Abs(v1.X - v2[0]) > 1e-5)
                return false;

            if (Math.Abs(v1.Y - v2[1]) > 1e-5)
                return false;

            if (Math.Abs(v1.Z - v2[2]) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not neec comparison
            //else
            return true;
        }

        public static bool operator !=(Vector3H1 v1, float[] v2)
        {
            return !(v1 == v2);
        }


        public static bool operator ==(float[] v1, Vector3H1 v2)
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

            if (float.IsNaN(v1[2]))
                return false;

            if (float.IsNaN(v2.X))
                return false;

            if (float.IsNaN(v2.Y))
                return false;

            if (float.IsNaN(v2.Z))
                return false;

            //else have to check values
            if (Math.Abs(v2.X - v1[0]) > 1e-5)
                return false;

            if (Math.Abs(v2.Y - v1[1]) > 1e-5)
                return false;

            if (Math.Abs(v2.Z - v1[2]) > 1e-5)
                return false;
            //of course the implied homogenous coordinate of 1 does not neec comparison
            //else
            return true;
        }

        public static bool operator !=(float[] v1, Vector3H1 v2)
        {
            return !(v1 == v2);
        }


        public static bool operator !=(Vector3H1 v1, Vector3H1 v2)
        {
            return !(v1 == v2);
        }

        public static Vector3H1 operator +(Vector3H1 v1, Vector3H1 v2)
        {
            return new Vector3H1 { X = v1.X + v2.X, Y = v1.Y + v2.Y, Z = v1.Z + v2.Z };
        }

        public static Vector3H1 operator -(Vector3H1 v1, Vector3H1 v2)
        {
            return new Vector3H1 { X = v1.X - v2.X, Y = v1.Y - v2.Y, Z = v1.Z - v2.Z };
        }

        public static Vector3H1 operator -(Vector3H1 v2)
        {
            return new Vector3H1 { X = - v2.X, Y = - v2.Y, Z = - v2.Z };
        }

        public static float operator *(Vector3H1 U, Vector3H1 V)
        {
            return DotProduct(U, V);
        }

        public static Vector3H1 operator *(float coeff, Vector3H1 V)
        {
            return new Vector3H1 { X = coeff * V.X, Y = coeff * V.Y, Z =coeff * V.Z };
        }

        public static Vector3H1 operator /(Vector3H1 V, float coeff)
        {
            return new Vector3H1 { X = V.X / coeff, Y = V.Y / coeff, Z = V.Z / coeff };
        }

        public static Vector3H1 operator *(IMatrix4D M, Vector3H1 V)
        {
            Vector3H1 NewVect = new Vector3H1();
            NewVect.X = M.Data[0, 0] * V.X + M.Data[0, 1] * V.Y + M.Data[0, 2] * V.Z + M.Data[0, 3];  // * V.H //H is always assumed to be 1 
            NewVect.Y = M.Data[1, 0] * V.X + M.Data[1, 1] * V.Y + M.Data[1, 2] * V.Z + M.Data[1, 3];  // * V.H
            NewVect.Z = M.Data[2, 0] * V.X + M.Data[2, 1] * V.Y + M.Data[2, 2] * V.Z + M.Data[2, 3];  // * V.H
            //NewVect.H= M.Data[3,0]*V.X + M.Data[3,1]*V.Y + M.Data[3,2]*V.Z + M.Data[3,3] * V.H
            return NewVect;
        }

        public static Vector3H1 operator *(IMatrix3H4 M, Vector3H1 V)
        {
            Vector3H1 NewVect = new Vector3H1();
            NewVect.X = M.Data[0, 0] * V.X + M.Data[0, 1] * V.Y + M.Data[0, 2] * V.Z + M.Data[0, 3];  // * V.H //H is always assumed to be 1 
            NewVect.Y = M.Data[1, 0] * V.X + M.Data[1, 1] * V.Y + M.Data[1, 2] * V.Z + M.Data[1, 3];  // * V.H
            NewVect.Z = M.Data[2, 0] * V.X + M.Data[2, 1] * V.Y + M.Data[2, 2] * V.Z + M.Data[2, 3];  // * V.H
            //NewVect.H= M.Data[3,0]*V.X + M.Data[3,1]*V.Y + M.Data[3,2]*V.Z + M.Data[3,3] * V.H
            return NewVect;
        }

        public static Vector3H1 operator *(IMatrix3D M, Vector3H1 V)
        {
            Vector3H1 NewVect = new Vector3H1();
            NewVect.X = M.Data[0, 0] * V.X + M.Data[0, 1] * V.Y + M.Data[0, 2] * V.Z;  // * V.H //H is always assumed to be 1 
            NewVect.Y = M.Data[1, 0] * V.X + M.Data[1, 1] * V.Y + M.Data[1, 2] * V.Z;  // * V.H
            NewVect.Z = M.Data[2, 0] * V.X + M.Data[2, 1] * V.Y + M.Data[2, 2] * V.Z;  // * V.H
            return NewVect;
        }
        #endregion


        #region static math functions for the whole class
        public static double Distance(Vector3H1 v1, Vector3H1 v2)  //EX: Vector3H1.Distance(Vect1, Vect2);  
        {
            return Math.Sqrt((v2.X - v1.X) * (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y) + (v2.Z - v1.Z) * (v2.Z - v1.Z));
        }

        public static Vector3H1 CrossProduct(Vector3H1 U, Vector3H1 V)  //need 3 unit tests
        {
            return new Vector3H1 { X = U.Y * V.Z - U.Z * V.Y,
                                   Y = U.Z * V.X - U.X * V.Z,
                                   Z = U.X * V.Y - U.Y * V.X    };
        }

        public static float DotProduct(Vector3H1 U, Vector3H1 V)
        {
            return (U.X * V.X + U.Y * V.Y + U.Z * V.Z);
        }
        
        #endregion




        #region instance methods
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }


        public IVector3H1 GetUnitVector()
        {
            float magnitude = (float)this.Magnitude();
            return new Vector3H1 { X = this.X / magnitude, Y = this.Y / magnitude, Z = this.Z / magnitude };
        }

        public void SetUnitVector()
        {
            //this = this.GetUnitVector() //sadly C# won't allow this
            double magnitude = this.Magnitude();
            X = (float)(X / magnitude);
            Y = (float)(Y / magnitude);
            Z = (float)(Z / magnitude);
        }

        public static IMatrix3D OutterProduct(IVector3H1 A, IVector3H1 B)
        {
            Matrix3X3 NewMatrix = new Matrix3X3();
            NewMatrix.Data[0, 0] = A.X * B.X;
            NewMatrix.Data[0, 1] = A.X * B.Y;
            NewMatrix.Data[0, 2] = A.X * B.Z;
            NewMatrix.Data[1, 0] = A.Y * B.X;
            NewMatrix.Data[1, 1] = A.Y * B.Y;
            NewMatrix.Data[1, 2] = A.Y * B.Z;
            NewMatrix.Data[2, 0] = A.Z * B.X;
            NewMatrix.Data[2, 1] = A.Z * B.Y;
            NewMatrix.Data[2, 2] = A.Z * B.Z;
	        return NewMatrix;
        }


        public object Clone()
        {
            return this.MemberwiseClone(); //should be ok b/c this is a "shallow object" that contains no sub objects
        }
        #endregion

    }
}
