using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kamisado
{
    public class CompositeBot : IPlayer
    {
        public int Score { get; set; }

        private Bot[] _bots;
        private double[] _weights;

        public CompositeBot(Bot[] bots, double[] weights)
        {
            _bots = bots;
            _weights = weights;
            Score = 0;
        }

        public MoveInfo GetMove(GameState currentState)
        {
            return GetMove(currentState, -1);
        }

        public MoveInfo GetMove(GameState currentState, int maxDepth)
        {
            List<IMove> possibleMoves = currentState.PossibleMoves;
            IMove[] moves = new IMove[possibleMoves.Count];
            possibleMoves.CopyTo(moves, 0);

            // For every bot, find the value of the moves
            MoveInfo[][] moveInfos = new MoveInfo[_bots.Length][];
            for (int i = 0; i < _bots.Length; i++)
            {
                double[] moveValues;
                if (maxDepth < 0)
                {
                    moveValues = _bots[i].GetMoveValues(currentState);
                }
                else
                {
                    moveValues = _bots[i].GetMoveValues(currentState, maxDepth);
                }

                moveInfos[i] = new MoveInfo[moveValues.Length];
                for (int j = 0; j < moveValues.Length; j++)
                {
                    moveInfos[i][j] = new MoveInfo(moves[j], moveValues[j], -1);
                }
            }

            // Find each bots highest value
            double[] highestValues = new double[_bots.Length];
            for (int i = 0; i < _bots.Length; i++)
            {
                highestValues[i] = Double.MinValue;
                for (int j = 0; j < moveInfos[i].Length; j++)
                {
                    if (moveInfos[i][j].Value > highestValues[i])
                    {
                        highestValues[i] = moveInfos[i][j].Value;
                    }
                }
            }

            // Check if there is a winning move
            for (int i = 0; i < highestValues.Length; i++)
            {
                if (highestValues[i] > Double.MaxValue / 10000)
                {
                    for (int j = 0; j < moveInfos[i].Length; j++)
                    {
                        if (moveInfos[i][j].Value == highestValues[i])
                        {
                            return moveInfos[i][j];
                        }
                    }
                }
            }

            double[] lowestNonLosingValues = new double[_bots.Length];
            for (int i = 0; i < _bots.Length; i++)
            {
                lowestNonLosingValues[i] = Double.MaxValue;
                for (int j = 0; j < moveInfos[i].Length; j++)
                {
                    if (moveInfos[i][j].Value > Double.MinValue / 10000 && moveInfos[i][j].Value < lowestNonLosingValues[i])
                    {
                        lowestNonLosingValues[i] = moveInfos[i][j].Value;
                    }
                }
            }

            // Check if all moves are losing
            for (int i = 0; i < lowestNonLosingValues.Length; i++)
            {
                if (lowestNonLosingValues[i] == Double.MaxValue)
                {
                    // All moves are losing
                    return moveInfos[0][0];
                }
            }

            for (int i = 0; i < _bots.Length; i++)
            {
                Debug.Write("Bot " + i + " has: ");
                for (int j = 0; j < moveInfos[i].Length; j++)
                {
                    Debug.Write(moveInfos[i][j].Value + ", ");
                }
                Debug.WriteLine("");
            }

            // Make all values between -1 and 1
            for (int i = 0; i < _bots.Length; i++)
            {
                for (int j = 0; j < moveInfos[i].Length; j++)
                {
                    if (moveInfos[i][j].Value < Double.MinValue / 10000)
                    {
                        continue;
                    }
                    else if (moveInfos[i][j].Value < 0)
                    {
                        moveInfos[i][j].Value = -(moveInfos[i][j].Value / lowestNonLosingValues[i]);
                    }
                    else if (moveInfos[i][j].Value > 0)
                    {
                        moveInfos[i][j].Value = moveInfos[i][j].Value / highestValues[i];
                    }
                }
            }

            for (int i = 0; i < _bots.Length; i++)
            {
                Debug.Write("Bot " + i + " has: ");
                for (int j = 0; j < moveInfos[i].Length; j++)
                {
                    Debug.Write(moveInfos[i][j].Value + ", ");
                }
                Debug.WriteLine("");
            }

            // Combine values
            List<int> highestCombinedIndeces = new List<int>();
            double highestCombined = Double.MinValue;
            double[] combinedMoveValues = new double[moves.Length];
            for (int i = 0; i < combinedMoveValues.Length; i++)
            {
                for (int j = 0; j < _bots.Length; j++)
                {
                    if (moveInfos[j][i].Value < Double.MinValue / 10000)
                    {
                        combinedMoveValues[i] = moveInfos[j][i].Value;
                        break;
                    }
                    else
                    {
                        combinedMoveValues[i] += _weights[j] * moveInfos[j][i].Value;
                    }
                }

                if (combinedMoveValues[i] > highestCombined)
                {
                    highestCombined = combinedMoveValues[i];
                    highestCombinedIndeces = new List<int>();
                    highestCombinedIndeces.Add(i);
                }
                else if (combinedMoveValues[i] == highestCombined)
                {
                    highestCombinedIndeces.Add(i);
                }
            }

            highestCombinedIndeces.Shuffle();

            return new MoveInfo(moves[highestCombinedIndeces[0]], highestCombined, -1);
        }
    }
}
