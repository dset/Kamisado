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

        public int Score { get; set; }

        public Human()
        {
            GotMove = false;
            Score = 0;
        }

        public MoveInfo GetMove(GameState currentState)
        {
            while (!GotMove) ;
            GotMove = false;

            return new MoveInfo(ChosenMove, 0, 0);
        }
    }
}
