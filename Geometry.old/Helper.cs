using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace Geometry
{
    public static class Helper
    {
        private static float degreesToRadiansRatio;
        private static float radiansToDegreesRatio;
        private static Random rand;

        static Helper() // this static constructor initializes these values just once long before the helper is called at run time
        {
            degreesToRadiansRatio = ((float) Math.PI) / 180;
            radiansToDegreesRatio = 180 / (float)Math.PI;
            rand = new Random(); //field initialization would also work
        }

        public static float RandomFloat(bool allowNegative = true)
        {
            if (allowNegative)
            {
                double d = rand.NextDouble();
                if (d > 0.5d)
                {
                    return (float)rand.NextDouble();
                }
                else
                {
                    return -(float)rand.NextDouble();
                }
            }
            else
            {
                return (float)rand.NextDouble();
            }
        }

        public static byte RandomByte() //this allows me to store an individual random byte to a variable so it can be compared later
        {
            byte[] byteHolder = new byte[1];
            rand.NextBytes(byteHolder);
            return byteHolder[0];
        }

        public static float DegreesToRadians(float d)
        {
            return degreesToRadiansRatio * d;
        }

        public static float RadiansToDegrees(float r)
        {
            return radiansToDegreesRatio * r;
        }

        public static bool AreSame(float x, float y, float tolerance = 1e-5F)
        {
            if (float.IsNaN(x))
                return false;

            if (float.IsNaN(y))
                return false;

            if (Math.Abs(x - y) > 1e-5)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Determinate2D quickly calculates the determinate of a 2X2 matrix
        ///  --- a larger matrix can be used, but the extra data will be ignored
        ///      and you only get the determinate of the upper left corner 2X2 elements
        /// </summary>
        /// <param name="A">IMatrix</param>
        /// <returns>the determinate</returns>
        public static float Determinate2D(IMatrix A)
        {
            return A.Data[0, 0] * A.Data[1, 1] - A.Data[0, 1] * A.Data[1, 0];
        }

        /// <summary>
        /// Determinate3D quickly calculates the determinate of a 3X3 matrix
        ///  --- a larger matrix can be used, but the extra data will be ignored
        ///      and you only get the determinate of the upper left corner 3X3 elements
        /// </summary>
        /// <param name="A">IMatrix</param>
        /// <returns>the determinate</returns>
        public static float Determinate3D(IMatrix A)
        {
            return A.Data[0, 0] * A.Data[1, 1] * A.Data[2, 2]
                 + A.Data[1, 2] * A.Data[0, 1] * A.Data[2, 0]
                 + A.Data[1, 0] * A.Data[0, 2] * A.Data[2, 1]
                 - A.Data[2, 2] * A.Data[0, 1] * A.Data[1, 0]
                 - A.Data[2, 0] * A.Data[0, 2] * A.Data[1, 1]
                 - A.Data[0, 0] * A.Data[1, 2] * A.Data[2, 1];
        }




        /// <summary>
        /// QuickSolve3D 
        /// Solves matrix equation [A] * x = N for x based on Cramer's Rule
        /// where A is a Matrix, N is a vector (usually a normal)
        /// -----------------------------------------------------------------
        /// Marix A is typed as IMatrix so that Matrix3X3, Matrix3H4 or Matrix4X4 can be used
        /// extra data is ignored, but Matrix2X2 would cause an error (but it doesn't exist yet)
        /// </summary>
        /// <param name="A">IMatrix</param>
        /// <param name="N">IVector3H1</param>
        /// <param name="bUnitize">bool</param>
        /// <returns></returns>
        public static Vector3H1 QuickSolve3D(IMatrix A, IVector3H1 N, bool bUnitize = true)
        {
            float determinate = Determinate3D(A);

            //if the determinate comes out to zero, it's probably b/c this is actually a small
            if (determinate == 0) //2X2 matrix in a larger object w/ the rest zeros, so just 
            {
                determinate = Determinate2D(A); //go ahead & take the 2X2 determinate
                A.Data[2, 2] = 1; //but you aldo have to set the Z-identity
            }                     //this works, but it it good to change the matrix????????


            Vector3H1 V = new Vector3H1();

            V.X = ( ( A.Data[2, 2] * A.Data[1, 1] - A.Data[1, 2] * A.Data[2, 1]) * N.X
                  + ( A.Data[2, 1] * A.Data[0, 2] - A.Data[0, 1] * A.Data[2, 2]) * N.Y
                  + ( A.Data[0, 1] * A.Data[1, 2] - A.Data[1, 1] * A.Data[0, 2]) * N.Z )
                         / determinate;

            V.Y = (  (  A.Data[1, 2] * A.Data[2, 0] - A.Data[2, 2] * A.Data[1, 0]) * N.X
                    +(  A.Data[0, 0] * A.Data[2, 2] - A.Data[2, 0] * A.Data[0, 2]) * N.Y
                    + ( A.Data[1, 0] * A.Data[0, 2] - A.Data[0, 0] * A.Data[1, 2]) * N.Z )
                          / determinate;

            V.Z = ( ( A.Data[2, 1] * A.Data[1, 0] - A.Data[1, 1] * A.Data[2, 0]) * N.X 
                   +( A.Data[0, 1] * A.Data[2, 0] - A.Data[0, 0] * A.Data[2, 1]) * N.Y
                   +( A.Data[0, 0] * A.Data[1, 1] - A.Data[0, 1] * A.Data[1, 0]) * N.Z )
                         / determinate;


            if (bUnitize)
            {
                V.SetUnitVector();
            }

            return V;
        }


    }
}
