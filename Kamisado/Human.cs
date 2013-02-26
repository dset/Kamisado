using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class Human : IPlayer
    {
        public volatile bool GotMove;
        public IMove ChosenMove;

        public Human()
        {
            GotMove = false;
        }

        public MoveInfo GetMove(GameState currentState)
        {
            while (!GotMove) ;
            GotMove = false;

            return new MoveInfo(ChosenMove, 0);
        }
    }
}
