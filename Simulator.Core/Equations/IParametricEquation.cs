using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Core.Equations
{
    public interface IParametricEquation
    {
        OpenTK.Vector3 Evaluate(float t);
    }
}
