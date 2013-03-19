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
                    //return 1.3 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
                    return 0;
                };

            IPlayer player1 = new Human();
            IPlayer player2 = new Bot(5, evaluator);

            GameEngine engine = new GameEngine(player1, player2, new GameState());
            GamePlayViewModel gpvm = new GamePlayViewModel(engine);
            
            engine.StateChanged += gpvm.OnGameStateChanged;
            engine.GameOver += gpvm.OnGameOver;

            GamePlayPage gpp = new GamePlayPage();
            gpp.DataContext = gpvm;

            MainWindow window = new MainWindow();
            window.Navigate(gpp);

            Thread engineThread = new Thread(new ThreadStart(engine.RunInThread));
            engineThread.IsBackground = true;
            engineThread.Start();
            while (!engineThread.IsAlive) ;

            window.Show();
        }

        private double PiecesInStriking(GameState currentState, bool imPlayerTwo)
        {
            int numStriking = 0;
            foreach (Piece myPiece in currentState.PiecePositions[Convert.ToInt32(imPlayerTwo)])
            {
                List<IMove> possibleMoves = myPiece.GetPossibleMoves(currentState);

                foreach (IMove m in possibleMoves)
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

    static class MyExtensions
    {
        static readonly Random Random = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
