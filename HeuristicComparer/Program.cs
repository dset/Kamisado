using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Kamisado;

namespace HeuristicComparer
{
    class Program
    {
        private const int NUM_MATCHES = 20;
        private const int WIN_SCORE = 15;

        static Func<GameState, bool, double> moveFarEval = (currentState, imPlayerTwo) =>
        {
            return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
        };

        static Func<GameState, bool, double> piecesInStrikingEval = (currentState, imPlayerTwo) =>
        {
            return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
        };

        static Func<GameState, bool, double> numPossibleMovesEval = (currentState, imPlayerTwo) =>
        {
            return NumPossibleMoves(currentState, imPlayerTwo) - NumPossibleMoves(currentState, !imPlayerTwo);
        };

        static Func<GameState, bool, double> numPossibleColorsEval = (currentState, imPlayerTwo) =>
        {
            return NumPossibleColors(currentState, imPlayerTwo) - NumPossibleColors(currentState, !imPlayerTwo);
        };

        static void Main(string[] args)
        {
            Match1();
            Match2();
            Match3();
            Match4();
            Match5();
            Match6();
        }

        static void Match1()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, moveFarEval);
            string player2Description = "depth 5, MoveFar";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_MoveFar");
        }

        static void Match2()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, piecesInStrikingEval);
            string player2Description = "depth 5, PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_PiecesInStriking");
        }

        static void Match3()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, numPossibleMovesEval);
            string player2Description = "depth 5, NumPossibleMoves";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_NumPossibleMoves");
        }

        static void Match4()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, moveFarEval);
            comp2[1] = new Bot(5, piecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 2;
            weights2[1] = 1;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 2 MoveFar, 1 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_2MoveFar_1PiecesInStriking");
        }

        static void Match5()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, moveFarEval);
            comp2[1] = new Bot(5, piecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 1;
            weights2[1] = 2;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 1 MoveFar, 2 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_1MoveFar_2PiecesInStriking");
        }

        static void Match6()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, moveFarEval);
            comp[1] = new Bot(5, piecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, moveFarEval);
            comp2[1] = new Bot(5, piecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 1;
            weights2[1] = 3;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 1 MoveFar, 3 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_1MoveFar_3PiecesInStriking");
        }

        private static void Compare(IPlayer player1, string player1Description, IPlayer player2, string player2Description, string dir)
        {
            string dirname = dir;
            string path = @"C:\Users\Dan\Documents\Visual Studio 2012\Projects\Kamisado\HeuristicComparer\bin\Debug\" + dirname + @"\";
            Directory.CreateDirectory(path);

            int player1Wins = 0;
            int player2Wins = 0;

            for (int i = 0; i < NUM_MATCHES / 2; i++)
            {
                player1.Score = 0;
                player2.Score = 0;
                MatchEngine me = new MatchEngine(player1, player2, WIN_SCORE);
                MatchInfo mi = me.Run();
                mi.Player1Description = player1Description;
                mi.Player2Description = player2Description;

                if (mi.Player2Won)
                {
                    player2Wins++;
                }
                else
                {
                    player1Wins++;
                }

                File.AppendAllText(path + "Match" + (i+1) + ".txt", mi.ToString());

                Console.WriteLine("Done with match");
            }

            for (int i = NUM_MATCHES / 2; i < NUM_MATCHES; i++)
            {
                player1.Score = 0;
                player2.Score = 0;
                MatchEngine me = new MatchEngine(player2, player1, WIN_SCORE);
                MatchInfo mi = me.Run();
                mi.Player1Description = player2Description;
                mi.Player2Description = player1Description;

                if (mi.Player2Won)
                {
                    player1Wins++;
                }
                else
                {
                    player2Wins++;
                }

                File.AppendAllText(path + "Match" + (i + 1) + ".txt", mi.ToString());

                Console.WriteLine("Done with match");
            }

            string output = "Player 1: " + player1Description + Environment.NewLine;
            output += "Player 2: " + player2Description + Environment.NewLine;
            output += "Player 1 Wins: " + player1Wins + Environment.NewLine;
            output += "Player 2 Wins " + player2Wins;

            File.AppendAllText(path + "Output.txt", output);
        }

        private static double PiecesInStriking(GameState currentState, bool imPlayerTwo)
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
}
