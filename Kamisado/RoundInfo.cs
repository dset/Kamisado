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
        public string Challenger { get; set; }
        public string Defender { get; set; }

        public RoundInfo(GameState startState)
        {
            StartState = startState;
            MadeMoves = new LinkedList<MoveInfo>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Challenger: " + Challenger);
            sb.AppendLine("Defender: " + Defender);
            sb.AppendLine("Winner: " + (PlayerTwoWon? Defender : Challenger));
            sb.AppendLine("Score: " + Score);
            sb.AppendLine("---------------------------");
            foreach (MoveInfo move in MadeMoves)
            {
                sb.AppendLine(move.ToString());
            }
            return sb.ToString();
        }
    }
}
