using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    public interface IPlayer
    {
        int Score { get; set; }
        MoveInfo GetMove(GameState currentState);
    }
}
