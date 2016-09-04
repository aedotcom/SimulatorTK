using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    //this interface is only necessary so the loop unrolled functions can tell the 2 classes apart
    public interface IMatrix3H4 : IMatrix
    {
        void SetScale(float SX, float SY, float SZ); //could have 2D or 1D scale, but they're not interesting
        void SetRotationZ(float radians); //at least 2x2 allows the possibility  of rotating around z
        void SetRotationY(float radians); //at leat 3x3 brings the possibility of rotation around any axis
        void SetRotationX(float radians); //technically these matricies don't have to be square, but in graphics
        void RotateAnyAxis(float radians, IVector3H1 axisUnitVector); //they usually are
        void SetTranslate(float TX, float TY, float TZ);
        void SetTranslate(float[] move);
        void SetTranslate(IVector3H1 V);
        void Spin(float radians, IVector3H1 axisUnitVector, IVector3H1 RelCenter);
        void Stretch(float SX, float SY, float SZ, IVector3H1 RelCenter);
    }
}
