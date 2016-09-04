using System;
namespace Interfaces
{
    public interface IVector3H1
    {
        object Clone();
        bool Equals(object obj);
        int GetHashCode();
        IVector3H1 GetUnitVector();
        double Magnitude();
        void SetUnitVector();
        void SetXYZ(float x, float y, float z);
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
    }
}
