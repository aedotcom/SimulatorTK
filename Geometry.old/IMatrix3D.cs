using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry
{
    public interface IMatrix3D : IMatrix
    {
        void SetScale(float SX, float SY, float SZ); //could have 2D or 1D scale, but they're not interesting
        void SetRotationZ(float radians); //at least 2x2 allows the possibility  of rotating around z
        void SetRotationY(float radians); //at leat 3x3 brings the possibility of rotation around any axis
        void SetRotationX(float radians); //technically these matricies don't have to be square, but in graphics
        void RotateAnyAxis(float radians, Vector3H1 axisUnitVector); //they usually are
    }
}
