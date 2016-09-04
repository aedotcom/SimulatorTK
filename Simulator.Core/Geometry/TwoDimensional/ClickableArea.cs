using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Core.Geometry.TwoDimensional
{
    public abstract class ClickableArea
    {
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool InsideArea(int x, int y)
        {
            if ((x >= X0 && x <= (X0 + Width))
            && (y >= Y0 && y <= (Y0 + Height)))
            { return true; }
            else
            { return false; }
        }
    }
}
