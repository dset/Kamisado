using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class GameEngine
    {
        public EventHandler<GameOverEventArgs> GameOver = delegate { };

        public IPlayer Player1 { get; private set; }
        public IPlayer Player2 { get; private set; }
        public GameState CurrentState { get; private set; }

        private IPlayer _activePlayer;

        public GameEngine(IPlayer player1, IPlayer player2)
        {
            Player1 = player1;
            Player2 = player2;
            _activePlayer = Player1;
            CurrentState = new GameState();
        }


        public void Run()
        {
            CurrentState = new GameState(CurrentState, _activePlayer.GetMove(CurrentState));
            if (CurrentState.PlayerTwoWinning.HasValue)
            {
                NotifyGameOver(CurrentState.PlayerTwoWinning.Value);
                return;
            }


            _activePlayer = _activePlayer == Player1 ? Player2 : Player1;
        }

        private void NotifyGameOver(bool playerTwoWon)
        {
            GameOverEventArgs e = new GameOverEventArgs(playerTwoWon);
            GameOver(this, e);
        }
    }
}
