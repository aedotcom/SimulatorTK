using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Core.Geometry.TwoDimensional
{
    public abstract class ClickableArea
    {

        //TODO: ADD CheckPoint(int x, int y)


        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


    }
}
