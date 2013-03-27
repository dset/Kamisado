using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Drawing;

namespace Kamisado
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            Func<GameState, bool, double> moveFarEval = (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            };

            Func<GameState, bool, double> piecesInStrikingEval = (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            };

            Func<GameState, bool, double> numPossibleMovesEval = (currentState, imPlayerTwo) =>
            {
                return NumPossibleMoves(currentState, imPlayerTwo) - NumPossibleMoves(currentState, !imPlayerTwo);
            };

            Func<GameState, bool, double> numPossibleColorsEval = (currentState, imPlayerTwo) =>
            {
                return NumPossibleColors(currentState, imPlayerTwo) - NumPossibleColors(currentState, !imPlayerTwo);
            };

            IPlayer player1 = new Human();

            Bot[] bots = new Bot[3];
            bots[0] = new Bot(5, piecesInStrikingEval);
            bots[1] = new Bot(5, moveFarEval);
            bots[2] = new Bot(5, numPossibleMovesEval);
            //bots[3] = new Bot(5, numPossibleColorsEval);

            double[] weights = new double[3];
            weights[0] = 1;
            weights[1] = 1;
            weights[2] = 1;
            //weights[3] = 1;
            IPlayer player2 = new CompositeBot(bots, weights);

            List<Piece> pieces = new List<Piece>();
            pieces.Add(new Piece(false, new System.Drawing.Point(0, 7), PieceColor.Blue, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(1, 7), PieceColor.Yellow, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(2, 7), PieceColor.Brown, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(3, 7), PieceColor.Pink, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(4, 7), PieceColor.Green, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(5, 7), PieceColor.Red, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(6, 7), PieceColor.Orange, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(7, 7), PieceColor.Purple, 3));

            pieces.Add(new Piece(true, new System.Drawing.Point(0, 0), PieceColor.Yellow, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(1, 0), PieceColor.Purple, 2));
            pieces.Add(new Piece(true, new System.Drawing.Point(2, 0), PieceColor.Green, 1));
            pieces.Add(new Piece(true, new System.Drawing.Point(3, 0), PieceColor.Orange, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(4, 0), PieceColor.Red, 1));
            pieces.Add(new Piece(true, new System.Drawing.Point(5, 0), PieceColor.Blue, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(6, 0), PieceColor.Pink, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(7, 0), PieceColor.Brown, 2));

            GameState startState = new GameState(pieces, null);

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

            return ((double)numStriking) / 24.0;
        }

        private static double NumPossibleMoves(GameState currentState, bool imPlayerTwo)
        {
            int numPossible = 0;
            for (int i = 0; i < 8; i++)
            {
                numPossible += currentState.PiecePositions[imPlayerTwo ? 1 : 0][i].GetPossibleMoves(currentState).Count;
            }

            return ((double)numPossible) / 102.0;
        }

        private static double MoveFar(GameState currentState, bool imPlayerTwo)
        {
            int score = 0;
            for (int i = 0; i < 8; i++)
            {
                if (imPlayerTwo)
                {
                    score += currentState.PiecePositions[1][i].Position.Y;
                }
                else
                {
                    score += 7 - currentState.PiecePositions[0][i].Position.Y;
                }
            }

            return ((double)score) / 48.0;
        }

        private static double BadToMoveUpAndDown(GameState currentState, bool imPlayerTwo)
        {
            double res = 0;
            foreach (Piece myPiece in currentState.PiecePositions[imPlayerTwo ? 1 : 0])
            {
                List<IMove> moves = myPiece.GetPossibleMoves(currentState);
                if (moves.Count == 1 && moves[0].IsTrivial)
                {
                    res -= 1;
                }
            }

            return res / 8.0;
        }

        private static double NumPossibleColors(GameState currentState, bool imPlayerTwo)
        {
            double res = 0;
            foreach (Piece myPiece in currentState.PiecePositions[imPlayerTwo ? 1 : 0])
            {
                int[] colorNumbers = new int[8];
                foreach (IMove move in myPiece.GetPossibleMoves(currentState))
                {
                    colorNumbers[(int)Board.Tile[move.End.Y, move.End.X]]++;
                }

                for (int i = 0; i < colorNumbers.Length; i++)
                {
                    res += Math.Sign(colorNumbers[i]);
                }
            }

            return res / 56.0;
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
