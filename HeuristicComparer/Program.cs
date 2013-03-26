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

        static void Main(string[] args)
        {
            Bot player1 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            });
            string player1Description = "depth 5, 1, 1, MoveFar";

            Bot player2 = new Bot(6, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 4);
            string player2Description = "depth 6, 1, 1, MoveFar, Cut width 4";

            Compare(player1, player1Description, player2, player2Description, "511_MoveFar_vs_611_c4_MoveFar");


            Bot player3 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            });
            string player3Description = "depth 5, 1, 1, MoveFar";

            Bot player4 = new Bot(8, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 3);
            string player4Description = "depth 8, 1, 1, MoveFar, Cut width 3";

            Compare(player3, player3Description, player4, player4Description, "511_MoveFar_vs_811_c3_MoveFar");



            Bot player5 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            });
            string player5Description = "depth 5, 1, 1, MoveFar";

            Bot player6 = new Bot(15, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 2);
            string player6Description = "depth 15, 1, 1, MoveFar, Cut width 2";

            Compare(player5, player5Description, player6, player6Description, "511_MoveFar_vs_1511_c2_MoveFar");




            Bot player7 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player7Description = "depth 5, 1, 1, PiecesInStriking";

            Bot player8 = new Bot(6, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 4);
            string player8Description = "depth 6, 1, 1, MoveFar, Cut width 4";

            Compare(player7, player7Description, player8, player8Description, "511_PiecesInStriking_vs_611_c4_MoveFar");


            Bot player9 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player9Description = "depth 5, 1, 1, PiecesInStriking";

            Bot player10 = new Bot(8, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 3);
            string player10Description = "depth 8, 1, 1, MoveFar, Cut width 3";

            Compare(player9, player9Description, player10, player10Description, "511_PiecesInStriking_vs_811_c3_MoveFar");



            Bot player11 = new Bot(5, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player11Description = "depth 5, 1, 1, PiecesInStriking";

            Bot player12 = new Bot(15, (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
            }, 2);
            string player12Description = "depth 15, 1, 1, MoveFar, Cut width 2";

            Compare(player11, player11Description, player12, player12Description, "511_PiecesInStriking_vs_1511_c2_MoveFar");
        }

        private static void Compare(Bot player1, string player1Description, Bot player2, string player2Description, string dir)
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
