using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Core
{
    public interface ITrinaryEquatable
    {
        Nullable<bool> TrinaryEqual(object other);
    }
}
