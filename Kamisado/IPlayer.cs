using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    interface IPlayer
    {
        public Move GetMove(GameState currentState);
    }
}
