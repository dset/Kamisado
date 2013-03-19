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

        public MatchInfo()
        {
            PlayedRounds = new LinkedList<RoundInfo>();
        }
    }
}
