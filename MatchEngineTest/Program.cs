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
                return NumPossibleMoves(currentState, imPlayerTwo) - NumPossibleMoves(currentState, !imPlayerTwo);
            };

            Func<GameState, bool, double> badevaluator = (currentState, imPlayerTwo) =>
            {
                return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
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

            return ((double)score) / 48;
        }
    }
}
