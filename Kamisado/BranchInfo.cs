using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class BranchInfo
    {
        public double Value { get; private set; }
        public int Depth { get; private set; }

        public BranchInfo(double value, int depth)
        {
            Value = value;
            Depth = depth;
        }

        public override string ToString()
        {
            return "Value: " + Value + " Depth: " + Depth;
        }
    }
}
