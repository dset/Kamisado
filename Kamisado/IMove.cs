using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public interface IMove
    {
        bool IsTrivial
        {
            get;
        }

        Piece Piece
        {
            get;
        }

        Point End
        {
            get;
        }

        GameState Execute();
        GameState Reverse();
    }
}
