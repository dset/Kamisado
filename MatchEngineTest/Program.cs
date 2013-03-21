using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kamisado;

namespace MatchEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<GameState, bool, double> evaluator = (currentState, imPlayerTwo) =>
            {
                return 4 * PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            };

            Func<GameState, bool, double> badevaluator = (currentState, imPlayerTwo) =>
            {
                return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
            };

            MatchEngine me = new MatchEngine(new Bot(4, badevaluator), new Bot(4, evaluator), 15);

            MatchInfo i = me.Run();
            i.Player1Description = "Bot 1 bla bla";
            i.Player2Description = "Bot 2 bla bla";

            Console.WriteLine(i);
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
