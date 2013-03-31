using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    public class Heuristics
    {
        public static Func<GameState, bool, double> MoveFarEval = (currentState, imPlayerTwo) =>
        {
            return MoveFar(currentState, imPlayerTwo) - MoveFar(currentState, !imPlayerTwo);
        };

        public static Func<GameState, bool, double> PiecesInStrikingEval = (currentState, imPlayerTwo) =>
        {
            return PiecesInStriking(currentState, imPlayerTwo) - PiecesInStriking(currentState, !imPlayerTwo);
        };

        public static Func<GameState, bool, double> NumPossibleMovesEval = (currentState, imPlayerTwo) =>
        {
            return NumPossibleMoves(currentState, imPlayerTwo) - NumPossibleMoves(currentState, !imPlayerTwo);
        };

        public static Func<GameState, bool, double> NumPossibleColorsEval = (currentState, imPlayerTwo) =>
        {
            return NumPossibleColors(currentState, imPlayerTwo) - NumPossibleColors(currentState, !imPlayerTwo);
        };

        public static Func<GameState, bool, double> WantToPushEval = (currentState, imPlayerTwo) =>
        {
            return WantToPush(currentState, imPlayerTwo) - WantToPush(currentState, !imPlayerTwo);
        };

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

            return ((double)numStriking);
        }

        private static double NumPossibleMoves(GameState currentState, bool imPlayerTwo)
        {
            int numPossible = 0;
            for (int i = 0; i < 8; i++)
            {
                numPossible += currentState.PiecePositions[imPlayerTwo ? 1 : 0][i].GetPossibleMoves(currentState).Count;
            }

            return ((double)numPossible);
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

            return ((double)score);
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

            return res;
        }

        private static double WantToPush(GameState currentState, bool imPlayerTwo)
        {
            int res = 0;
            foreach (Piece myPiece in currentState.PiecePositions[imPlayerTwo ? 1 : 0])
            {
                foreach (IMove move in myPiece.GetPossibleMoves(currentState))
                {
                    if (move.GetType() == typeof(SumoPushMove))
                    {
                        res++;
                    }
                }
            }

            return res;
        }
    }
}
