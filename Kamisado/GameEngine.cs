using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kamisado
{
    class GameEngine
    {
        public EventHandler<EventArgs> StateChanged = delegate { };
        public EventHandler<GameOverEventArgs> GameOver = delegate { };

        public IPlayer Player1 { get; private set; }
        public IPlayer Player2 { get; private set; }
        public GameState CurrentState { get; private set; }
        public IPlayer ActivePlayer { get; private set; }

        private RoundInfo _roundInfo;

        public GameEngine(IPlayer player1, IPlayer player2, GameState startState)
        {
            Player1 = player1;
            Player2 = player2;
            ActivePlayer = Player1;
            CurrentState = startState;
            _roundInfo = new RoundInfo(startState.Copy());
        }

        public void RunInThread()
        {
            Run();
        }

        public RoundInfo Run()
        {
            while (true)
            {
                MoveInfo moveInfo = ActivePlayer.GetMove(CurrentState);
                Debug.WriteLine("Chosen move: " + moveInfo);
                IMove move = moveInfo.Move;
                _roundInfo.MadeMoves.AddLast(moveInfo);
                move.Execute();
                NotifyStateChanged();

                if (CurrentState.PlayerTwoWinning.HasValue)
                {
                    NotifyGameOver(CurrentState.PlayerTwoWinning.Value);
                    _roundInfo.PlayerTwoWon = CurrentState.PlayerTwoWinning.Value;
                    _roundInfo.Score = (int)Math.Pow(2, CurrentState.WinningPiece.Sumoness);
                    return _roundInfo;
                }

                ActivePlayer = ActivePlayer == Player1 ? Player2 : Player1;
            }
        }

        private void NotifyGameOver(bool playerTwoWon)
        {
            GameOverEventArgs e = new GameOverEventArgs(playerTwoWon);
            GameOver(this, e);
        }

        private void NotifyStateChanged()
        {
            EventArgs e = new EventArgs();
            StateChanged(this, e);
        }
    }
}
