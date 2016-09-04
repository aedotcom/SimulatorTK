using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using Geometry //not necessary 

namespace Simulator.Core.Geometry.Meshes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ColorVBODataPoint
    {
        public Vector3 Position;
        public uint Color;

        public ColorVBODataPoint(float x, float y, float z, System.Drawing.Color color)
        {
            Position = new Vector3(x, y, z);
            Color = Helpers.SystemColorToRGBA(color);
        }

    }
}
