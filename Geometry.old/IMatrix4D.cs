using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry
{
    //4x4 or at least 3x4 allows homogenous translations 
    public interface IMatrix4D : IMatrix3D
    {
        void SetTranslate(float TX, float TY, float TZ);
        void SetTranslate(float[] move);
        void SetTranslate(Vector3H1 V);
        void Spin(float radians, Vector3H1 axisUnitVector, Vector3H1 RelCenter);
        void Stretch(float SX, float SY, float SZ, Vector3H1 RelCenter);
    }
}
