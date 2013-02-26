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
        public double Value { get; private set; }

        public MoveInfo(IMove move, double value)
        {
            Move = move;
            Value = value;
        }
    }
}
