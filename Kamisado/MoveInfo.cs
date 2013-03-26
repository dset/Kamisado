using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    public class MoveInfo
    {
        public IMove Move { get; private set; }
        public double Value { get; set; }
        public int Depth { get; private set; }

        public MoveInfo(IMove move, double value, int depth)
        {
            Move = move;
            Value = value;
            Depth = depth;
        }

        public override string ToString()
        {
            return Move + " with value " + Value + " at depth " + Depth;
        }
    }
}
