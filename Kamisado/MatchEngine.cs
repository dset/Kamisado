using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kamisado
{
    public class MatchEngine
    {
        private IPlayer _player1;
        private IPlayer _player2;
        private GameEngine _engine;

        private int _winScore;
        private MatchInfo _matchInfo;

        private IPlayer _startingPlayer;
        private IPlayer _secondPlayer;

        public MatchEngine(IPlayer player1, IPlayer player2, int winScore)
        {
            _player1 = player1;
            _player2 = player2;
            
            _winScore = winScore;

            _startingPlayer = player1;
            _secondPlayer = player2;

            _engine = new GameEngine(_startingPlayer, _secondPlayer, new GameState());
            _matchInfo = new MatchInfo();
        }

        public MatchInfo Run()
        {
            while (_player1.Score < _winScore && _player2.Score < _winScore)
            {
                RoundInfo roundInfo =_engine.Run();
                if (_startingPlayer == _player1)
                {
                    roundInfo.Challenger = "Player 1";
                    roundInfo.Defender = "Player 2";
                }
                else
                {
                    roundInfo.Challenger = "Player 2";
                    roundInfo.Defender = "Player 1";
                }

                _matchInfo.PlayedRounds.AddLast(roundInfo);
                if (roundInfo.PlayerTwoWon)
                {
                    _secondPlayer.Score += roundInfo.Score;
                }
                else
                {
                    _startingPlayer.Score += roundInfo.Score;
                    IPlayer tmp = _startingPlayer;
                    _startingPlayer = _secondPlayer;
                    _secondPlayer = tmp;
                }

                _engine = new GameEngine(_startingPlayer, _secondPlayer, GameState.GenerateNextRound(_engine.CurrentState, false));
            }

            _matchInfo.Player1Score = _player1.Score;
            _matchInfo.Player2Score = _player2.Score;
            _matchInfo.Player2Won = _player2.Score > _player1.Score;

            return _matchInfo;
        }
    }
}
