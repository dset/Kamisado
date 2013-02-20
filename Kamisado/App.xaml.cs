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
                    return 1.3 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
                    //return 0;
                };

            IPlayer player1 = new Bot(7, evaluator);
            IPlayer player2 = new Bot(7, evaluator);

            List<System.Drawing.Point> player1Pos = new List<System.Drawing.Point>();
            List<System.Drawing.Point> player2Pos = new List<System.Drawing.Point>();
            for (int i = 0; i < 8; i++)
            {
                player1Pos.Add(new System.Drawing.Point(i, 7));
                player2Pos.Add(new System.Drawing.Point(i, 0));
            }

            player1Pos.Shuffle();
            player2Pos.Shuffle();

            List<Piece> pieces = new List<Piece>();
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Piece(false, player1Pos[i], (PieceColor)i, 0));
                pieces.Add(new Piece(true, player2Pos[i], (PieceColor)i, 0));
            }

            pieces[2] = new Piece(false, pieces[2].Position, (PieceColor)1, 3);
            //pieces[2] = new Piece(false, pieces[2].Position, (PieceColor)1, 2);

            pieces[1] = new Piece(true, pieces[1].Position, (PieceColor)0, 1);
            pieces[3] = new Piece(true, pieces[3].Position, (PieceColor)1, 1);
            pieces[5] = new Piece(true, pieces[5].Position, (PieceColor)2, 1);
            pieces[7] = new Piece(true, pieces[7].Position, (PieceColor)3, 1);

            GameState startState = new GameState(pieces, null);

            GameEngine engine = new GameEngine(player1, player2, startState);
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
