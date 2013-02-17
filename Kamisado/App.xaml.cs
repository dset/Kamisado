using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Kamisado
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            Func<GameState, bool, double> evaluator = (currentState, imPlayerTwo) =>
                {
                    return 0;
                    //return 1.3 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
                };

            IPlayer player1 = new Bot(10, evaluator);
            IPlayer player2 = new Human();

            GameEngine engine = new GameEngine(player1, player2);
            GamePlayViewModel gpvm = new GamePlayViewModel(engine);
            
            engine.StateChanged += gpvm.OnGameStateChanged;
            engine.GameOver += gpvm.OnGameOver;

            GamePlayPage gpp = new GamePlayPage();
            gpp.DataContext = gpvm;

            MainWindow window = new MainWindow();
            window.Navigate(gpp);

            Thread engineThread = new Thread(new ThreadStart(engine.Run));
            engineThread.IsBackground = true;
            engineThread.Start();
            while (!engineThread.IsAlive) ;

            window.Show();
        }

        private double PiecesInStriking(GameState currentState, bool imPlayerTwo)
        {
            int numStriking = 0;
            foreach (System.Drawing.Point myPiece in currentState.PiecePositions[Convert.ToInt32(imPlayerTwo)])
            {
                LinkedList<Move> possibleMoves;
                if (imPlayerTwo)
                {
                    possibleMoves = currentState.GenerateNextMovesPlayerTwo(myPiece);
                }
                else
                {
                    possibleMoves = currentState.GenerateNextMovesPlayerOne(myPiece);
                }

                foreach (Move m in possibleMoves)
                {
                    if (imPlayerTwo && m.End.Y == 7)
                    {
                        numStriking += 1;
                        break;
                    }
                    else if (!imPlayerTwo && m.End.Y == 0)
                    {
                        numStriking += 1;
                        break;
                    }
                }
            }

            return ((double)numStriking) / 8.0;
        }
    }
}
