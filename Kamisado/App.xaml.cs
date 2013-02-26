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

            IPlayer player1 = new Bot(17, evaluator);
            IPlayer player2 = new Human();

            List<Piece> pieces = new List<Piece>();
            pieces.Add(new Piece(false, new System.Drawing.Point(2, 3), PieceColor.Brown, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(6, 4), PieceColor.Green, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(5, 6), PieceColor.Red, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(4, 3), PieceColor.Yellow, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(3, 2), PieceColor.Pink, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(3, 3), PieceColor.Purple, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(0, 7), PieceColor.Blue, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(4, 7), PieceColor.Orange, 0));

            pieces.Add(new Piece(true, new System.Drawing.Point(4, 2), PieceColor.Brown, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(1, 2), PieceColor.Green, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(6, 2), PieceColor.Red, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(0, 0), PieceColor.Yellow, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(2, 4), PieceColor.Pink, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(5, 1), PieceColor.Purple, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(7, 5), PieceColor.Blue, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(2, 7), PieceColor.Orange, 0));

            GameState prevState = new GameState(pieces, null);
            prevState.IsPlayerTwo = false;
            prevState.LastMove = new Move(prevState, prevState.PiecePositions[1][7], new System.Drawing.Point(2, 7));
            prevState.PieceToMove = prevState.PiecePositions[0][2];
            GameState startState = GameState.GenerateNextRound(prevState, false);

            GameEngine engine = new GameEngine(player1, player2, new GameState());
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
