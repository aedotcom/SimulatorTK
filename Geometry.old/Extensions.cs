using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace Geometry
{
    public static class Extensions
    {
        public static void Copy(this IMatrix A, IMatrix B)
        {
            A.SetZeros();
            for (int row = 0; row < Math.Min(A.Rows, B.Rows); row++)
            {
                for (int col = 0; col < Math.Min(A.Columns, B.Columns); col++)
                {
                    A.Data[row, col] = B.Data[row, col];
                }
            }
        }

        public static void Copy(this IMatrix A, float[,] B)
        {
            A.SetZeros();
            for (int row = 0; row < Math.Min(A.Rows, B.GetLength(0)); row++)
            {
                for (int col = 0; col < Math.Min(A.Columns, B.GetLength(1)); col++)
                {
                    A.Data[row, col] = B[row, col];
                }
            }
        }





        public static bool ContainsSameDataAs(this float[] a, float[] b, float tolerance = 1e-5F)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.  // ^ is the C# XOR
            if (((object)a == null) ^ ((object)b == null))
            {
                return false;
            }

            //if the lengths don't match, it's definitel not equal
            if (a.Length != b.Length)
            {
                return false;
            }

            //test the data
            for (int i = 0; i < a.Length; i++)
            {
                if (Math.Abs(a[i] - b[i]) > tolerance)
                    return false;
            }

            //else
            return true;
        }




    }
}
