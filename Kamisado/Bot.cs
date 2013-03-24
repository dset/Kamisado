using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Kamisado
{
    public class Bot : IPlayer
    {
        private Func<GameState, bool, double> _evaluate;
        private int _maxDepth;
        private bool _imPlayerTwo;

        public int Score { get; set; }

        public Bot(int maxDepth, Func<GameState, bool, double> evaluate)
        {
            _maxDepth = maxDepth;
            _evaluate = evaluate;
            Score = 0;
        }

        public MoveInfo GetMove(GameState currentState)
        {
            _imPlayerTwo = currentState.IsPlayerTwo;

            IMove[] moves = new IMove[currentState.PossibleMoves.Count];
            currentState.PossibleMoves.CopyTo(moves, 0);

            double[] moveValues = new double[moves.Length];

            for (int move = 0; move < moves.Length; move++)
            {
                moves[move].Execute();
                moveValues[move] = Min(currentState, 1, Double.MinValue, Double.MaxValue);
                moves[move].Reverse();

                //Debug.WriteLine("Move " + moves[move] + " had value " + moveValues[move]);
            }

            double bestValue = moveValues[0];
            for (int i = 0; i < moves.Length; i++)
            {
                if (moveValues[i] > bestValue)
                {
                    bestValue = moveValues[i];
                }
            }

            List<IMove> bestMoves = new List<IMove>();
            for (int i = 0; i < moves.Length; i++)
            {
                if (moveValues[i] == bestValue)
                {
                    bestMoves.Add(moves[i]);
                }
            }

            /*
            MoveInfo[] moveInfos = new MoveInfo[moves.Length];
            for (int i = 0; i < moves.Length; i++)
            {
                moveInfos[i] = new MoveInfo(moves[i], moveValues[i], -1);
            }

            Array.Sort(moveInfos, (MoveInfo mi1, MoveInfo mi2) =>
            {
                return Math.Sign(mi1.Value - mi2.Value);
            });*/


            /*Debug.WriteLine("Sorterad: ");
            for (int i = 0; i < moveInfos.Length; i++)
            {
                MoveInfo mi = moveInfos[i];
                Debug.WriteLine("Move " + mi.Move + " had value " + mi.Value);
            }*/

            bestMoves.Shuffle();
            Debug.WriteLine("Längden är " + bestMoves.Count);

            return new MoveInfo(bestMoves[0], bestValue, -1);
        }

        public MoveInfo GetMove(GameState currentState, int maxDepth)
        {
            int cachedMaxDepth = _maxDepth;
            _maxDepth = maxDepth;
            MoveInfo res = GetMove(currentState);
            _maxDepth = cachedMaxDepth;
            return res;
        }

        /*public MoveInfo GetMove(GameState currentState)
        {
            _imPlayerTwo = currentState.IsPlayerTwo;

            IMove[] moves = new IMove[currentState.PossibleMoves.Count];
            currentState.PossibleMoves.CopyTo(moves, 0);

            double[][] moveValues = new double[_maxDepth][];
            for (int i = 0; i < _maxDepth; i++)
            {
                moveValues[i] = new double[moves.Length];
            }

            int cachedMaxDepth = _maxDepth;
            for (int depth = 1; depth <= cachedMaxDepth; depth++)
            {
                _maxDepth = depth;
                for (int move = 0; move < moves.Length; move++)
                {
                    moves[move].Execute();
                    moveValues[depth - 1][move] = Min(currentState, 1, Double.MinValue, Double.MaxValue);
                    moves[move].Reverse();
                }
                Debug.WriteLine("Hello, im done with depth " + depth);
            }
            _maxDepth = cachedMaxDepth;

            Debug.WriteLine("Last row is: ");
            for (int move = 0; move < moves.Length; move++)
            {
                Debug.WriteLine(moves[move] + " had value " + moveValues[_maxDepth - 1][move]);
            }

            double lastRowHighestValue = Double.MinValue;
            IList<Int32> lastRowHighestValueIndices = new List<Int32>();
            for (int move = 0; move < moves.Length; move++)
            {
                if (moveValues[_maxDepth - 1][move] > lastRowHighestValue)
                {
                    lastRowHighestValueIndices = new List<Int32>();
                    lastRowHighestValue = moveValues[_maxDepth - 1][move];
                    lastRowHighestValueIndices.Add(move);
                }
                else if (moveValues[_maxDepth - 1][move] == lastRowHighestValue)
                {
                    lastRowHighestValueIndices.Add(move);
                }
            }

            if (lastRowHighestValue >= Double.MaxValue / 4)
            {
                int shortestIndex = -1;
                int shortestDepth = Int32.MaxValue;
                foreach (int index in lastRowHighestValueIndices)
                {
                    int currentSwitchIndex = _maxDepth - 1;
                    while (moveValues[currentSwitchIndex][index] == lastRowHighestValue)
                    {
                        currentSwitchIndex--;
                        if (currentSwitchIndex < 0)
                        {
                            break;
                        }
                    }

                    if (currentSwitchIndex + 2 < shortestDepth)
                    {
                        shortestDepth = currentSwitchIndex + 2;
                        shortestIndex = index;
                    }
                }

                return new MoveInfo(moves[shortestIndex], lastRowHighestValue, shortestDepth);
            }
            else if (lastRowHighestValue <= Double.MinValue / 4)
            {
                for (int depth = _maxDepth; depth > 0; depth--)
                {
                    foreach (int index in lastRowHighestValueIndices)
                    {
                        if (moveValues[depth - 1][index] > Double.MinValue / 4)
                        {
                            return new MoveInfo(moves[index], lastRowHighestValue, depth + 1);
                        }
                    }
                }

                return new MoveInfo(moves[lastRowHighestValueIndices[0]], lastRowHighestValue, 1);
            }
            else
            {
                lastRowHighestValueIndices.Shuffle();
                Debug.WriteLine("Detta är längden " + lastRowHighestValueIndices.Count);
                return new MoveInfo(moves[lastRowHighestValueIndices[0]], lastRowHighestValue, _maxDepth);
            }
        }*/

        private double Max(GameState gs, int depth, double alpha, double beta)
        {
            if (depth >= _maxDepth || gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double v = Double.MinValue;

            IEnumerable<IMove> possibleMoves = gs.PossibleMoves.Reverse<IMove>();

            foreach (IMove move in possibleMoves)
            {
                move.Execute();

                double current = Min(gs, depth + 1, alpha, beta);
                if (current > v)
                {
                    v = current;
                }

                move.Reverse();
                if (v >= beta)
                {
                    return v;
                }

                alpha = Math.Max(alpha, v);
            }

            return v;
        }

        private double Min(GameState gs, int depth, double alpha, double beta)
        {
            if (depth >= _maxDepth || gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double v = Double.MaxValue;

            IEnumerable<IMove> possibleMoves = gs.PossibleMoves.Reverse<IMove>();

            foreach (IMove move in possibleMoves)
            {
                move.Execute();

                double current = Max(gs, depth + 1, alpha, beta);
                if (current < v)
                {
                    v = current;
                }

                move.Reverse();
                if (v <= alpha)
                {
                    return v;
                }

                beta = Math.Min(beta, v);
            }

            return v;
        }

        private double EvaluateGameState(GameState gs)
        {
            if (gs.PlayerTwoWinning.HasValue && ((gs.PlayerTwoWinning.Value && _imPlayerTwo) || (!gs.PlayerTwoWinning.Value && !_imPlayerTwo)))
            {
                //return Double.MaxValue - 3 + gs.WinningPiece.Sumoness;
                return Double.MaxValue / (Math.Pow(10, 3 - gs.WinningPiece.Sumoness));
            }
            else if (gs.PlayerTwoWinning.HasValue)
            {
                //return Double.MinValue + 3 - gs.WinningPiece.Sumoness;
                return Double.MinValue / (Math.Pow(10, 3 - gs.WinningPiece.Sumoness));
            }

            return _evaluate(gs, _imPlayerTwo);
        }
    }
}
