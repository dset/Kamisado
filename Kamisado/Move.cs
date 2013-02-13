using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public struct Move
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }

        public Move(Point start, Point end) : this()
        {
            Start = start;
            End = end;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            Move m = (Move)obj;

            return Start.Equals(m.Start) && End.Equals(m.End);
        }
    }
}
