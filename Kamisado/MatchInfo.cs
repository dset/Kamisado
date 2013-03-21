using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    public class MatchInfo
    {
        public LinkedList<RoundInfo> PlayedRounds { get; private set; }
        public bool Player2Won { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public string Player1Description { get; set; }
        public string Player2Description { get; set; }

        public MatchInfo()
        {
            PlayedRounds = new LinkedList<RoundInfo>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Player 1: " + Player1Description);
            sb.AppendLine("Player 2: " + Player2Description);
            sb.AppendLine("Player 1 Score: " + Player1Score);
            sb.AppendLine("Player 2 Score: " + Player2Score);
            sb.AppendLine("Winner: " + (Player2Won? "Player 2" : "Player 1"));

            int player1Score = 0;
            int player2Score = 0;
            int i = 0;
            foreach (RoundInfo round in PlayedRounds)
            {
                i++;
                if ((round.PlayerTwoWon && round.Challenger == "Player 1") || (!round.PlayerTwoWon && round.Defender == "Player 1"))
                {
                    player2Score += round.Score;
                }
                else
                {
                    player1Score += round.Score;
                }

                sb.AppendLine("Round " + i + ", Player 1: " + player1Score + ", Player 2: " + player2Score);
            }

            int j = 0;
            foreach (RoundInfo round in PlayedRounds)
            {
                j++;
                sb.AppendLine("############### Round " + j + " ###############");
                sb.AppendLine(round.ToString());
                sb.AppendLine("#############################################");
            }

            return sb.ToString();
        }
    }
}
