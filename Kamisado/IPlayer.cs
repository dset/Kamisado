using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    interface IPlayer
    {
        Move GetMove(GameState currentState);
    }
}
