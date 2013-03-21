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
        private const int NUM_MATCHES = 10;
        private const int WIN_SCORE = 15;

        static void Main(string[] args)
        {
            Bot player1 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player1Description = "depth 7, 1, 1";

            Bot player2 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return 1.5 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player2Description = "depth 7, 1.5, 1";

            Compare(player1, player1Description, player2, player2Description, "711vs7151");



            Bot player3 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo);
            });
            string player3Description = "depth 7, 1, 0";

            Bot player4 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return 1.5 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player4Description = "depth 7, 1.5, 1";

            Compare(player3, player3Description, player4, player4Description, "710vs7151");

            Bot player5 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - 1.5 * PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player5Description = "depth 7, 1, 1.5";

            Bot player6 = new Bot(7, (currentState, imPlayerTwo) =>
            {
                return 1.5 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            });
            string player6Description = "depth 7, 1.5, 1";

            Compare(player5, player5Description, player6, player6Description, "7115vs7151");
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

            return ((double)numStriking) / 8.0;
        }
    }
}
