using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    public class RoundInfo
    {
        public GameState StartState { get; private set; }
        public LinkedList<MoveInfo> MadeMoves { get; private set; }
        public bool PlayerTwoWon { get; set; }
        public int Score { get; set; }

        public RoundInfo(GameState startState)
        {
            StartState = startState;
            MadeMoves = new LinkedList<MoveInfo>();
        }
    }
}
