using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK;

namespace Simulator.Core.Geometry.Meshes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexDataPoint
    {
        public Vector3 Position; 
        public Vector3 Normal;   
        public Vector2 TexCoord;
        public uint Color;

        //constructor
        public VertexDataPoint(float x, float y, float z, float nx, float ny, float nz, float texX, float texY, System.Drawing.Color color)
        {
            Position = new Vector3(x, y, z);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(texX, texY);
            Color = Helpers.SystemColorToRGBA(color);
        }
    }
}
