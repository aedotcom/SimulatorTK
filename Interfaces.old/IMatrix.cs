using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IMatrix : ICloneable
    {
        float[,] Data { get; set; }
        int Rows { get; }
        int Columns { get; }
        float DiagonalProduct();
        void SetZeros();
        void SetIdentity();
        void SetRandom();
        void CheckForZeros(float tolerance = 1e-5F);
    }
}
