using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry
{
    public interface IMatrix : ICloneable
    {
        float[,] Data { get; }
        int Rows { get; }
        int Columns { get; }
        float DiagonalProduct();
        void SetZeros();
        void SetIdentity();
        void SetRandom();
        void CheckForZeros(float tolerance = 1e-5F);
    }
}
