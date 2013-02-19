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

        public IMove GetMove(GameState currentState)
        {
            while (!GotMove) ;
            GotMove = false;

            return ChosenMove;
        }
    }
}
