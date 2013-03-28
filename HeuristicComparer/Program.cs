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
    public class Program
    {
        private const int NUM_MATCHES = 20;
        private const int WIN_SCORE = 15;

        static void Main(string[] args)
        {
            Match1();
            Match2();
            Match3();
            Match4();
            Match5();
            Match6();
            Match7();
            Match8();
            Match9();
        }

        static void Match1()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, Heuristics.MoveFarEval);
            string player2Description = "depth 5, MoveFar";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_MoveFar");
        }

        static void Match2()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, Heuristics.PiecesInStrikingEval);
            string player2Description = "depth 5, PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_PiecesInStriking");
        }

        static void Match3()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot player2 = new Bot(5, Heuristics.NumPossibleMovesEval);
            string player2Description = "depth 5, NumPossibleMoves";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_NumPossibleMoves");
        }

        static void Match4()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

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
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

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
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 1;
            weights2[1] = 3;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 1 MoveFar, 3 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_1MoveFar_3PiecesInStriking");
        }

        static void Match7()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 3;
            weights2[1] = 1;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 3 MoveFar, 1 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_3MoveFar_1PiecesInStriking");
        }

        static void Match8()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 1;
            weights2[1] = 5;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 1 MoveFar, 5 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_1MoveFar_5PiecesInStriking");
        }

        static void Match9()
        {
            Bot[] comp = new Bot[2];
            comp[0] = new Bot(5, Heuristics.MoveFarEval);
            comp[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights = new double[2];
            weights[0] = 1;
            weights[1] = 1;

            CompositeBot player1 = new CompositeBot(comp, weights);
            string player1Description = "depth 5, 1 MoveFar, 1 PiecesInStriking";

            Bot[] comp2 = new Bot[2];
            comp2[0] = new Bot(5, Heuristics.MoveFarEval);
            comp2[1] = new Bot(5, Heuristics.PiecesInStrikingEval);

            double[] weights2 = new double[2];
            weights2[0] = 5;
            weights2[1] = 1;

            CompositeBot player2 = new CompositeBot(comp2, weights2);
            string player2Description = "depth 5, 5 MoveFar, 1 PiecesInStriking";

            Compare(player1, player1Description, player2, player2Description, "5_1MoveFar_1PiecesInStriking_vs_5_5MoveFar_1PiecesInStriking");
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
    }
}
