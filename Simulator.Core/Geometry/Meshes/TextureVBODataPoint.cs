using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenTK;

namespace Simulator.Core.Geometry.Meshes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureVBODataPoint
    {
        public Vector3 Position; 
        public Vector3 Normal;   
        public Vector2 TexCoord; 

        //constructor
        public TextureVBODataPoint(float x, float y, float z, float nx, float ny, float nz, float texX, float texY)
        {
            Position = new Vector3(x, y, z);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(texX, texY);
        }


    }
}
