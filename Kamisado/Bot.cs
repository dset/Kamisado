using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Kamisado
{
    class Bot : IPlayer
    {
        private Func<GameState, bool, double> _evaluate;
        private int _maxDepth;
        private bool _imPlayerTwo;

        private double _numVisited;
        private double _numWinningEnd;
        private double _numLoosingEnd;

        public Bot(int maxDepth, Func<GameState, bool, double> evaluate)
        {
            _maxDepth = maxDepth;
            _evaluate = evaluate;
            _numVisited = 0;
            _numWinningEnd = 0;
            _numLoosingEnd = 0;
        }

        public Move GetMove(GameState currentState)
        {
            IEnumerable<Move> possible;
            if (!currentState.PieceToMove.HasValue)
            {
                LinkedList<Move> tmp = new LinkedList<Move>();
                tmp.AddLast(new Move(new Point(3, 7), new Point(3, 3)));
                possible = tmp;
            }
            else
            {
                possible = currentState.PossibleMoves.Reverse();
            }

            _imPlayerTwo = currentState.IsPlayerTwo;
            double currentMax = -1;
            Move bestMove = currentState.PossibleMoves.First.Value;

            double alpha = -1;
            double beta = 1;

            foreach (Move possibleMove in possible)
            {
                double v = Min(new GameState(currentState, possibleMove), 1, alpha, beta);
                Debug.WriteLine("Done with one branch. Value: " + v);
                if (v > currentMax)
                {
                    bestMove = possibleMove;
                    currentMax = v;
                }

                alpha = Math.Max(alpha, v);
            }
            Debug.WriteLine("Best was: " + currentMax);
            Debug.WriteLine("Visited: " + _numVisited + ", Winning: " + _numWinningEnd + ", Loosing: " + _numLoosingEnd);
            return bestMove;
        }

        private double Max(GameState gs, int depth, double alpha, double beta)
        {
            _numVisited++;
            if (depth == _maxDepth)
            {
                return EvaluateGameState(gs);
            }

            if (gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double v = Double.MinValue;
            foreach(Move possibleMove in gs.PossibleMoves.Reverse())
            {
                v = Math.Max(v, Min(new GameState(gs, possibleMove), depth + 1, alpha, beta));
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
            _numVisited++;
            if (depth == _maxDepth)
            {
                return EvaluateGameState(gs);
            }

            if (gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double v = Double.MaxValue;
            foreach (Move possibleMove in gs.PossibleMoves.Reverse())
            {
                v = Math.Min(v, Max(new GameState(gs, possibleMove), depth + 1, alpha, beta));
                if (v <= alpha)
                {
                    return v;
                }
                beta = Math.Min(beta, v);
            }

            return v;
        }

        private Random rand = new Random();

        private double EvaluateGameState(GameState gs)
        {
            if (gs.PlayerTwoWinning.HasValue && ((gs.PlayerTwoWinning.Value && _imPlayerTwo) || (!gs.PlayerTwoWinning.Value && !_imPlayerTwo)))
            {
                _numWinningEnd++;
                return 1;
            }
            else if(gs.PlayerTwoWinning.HasValue)
            {
                _numLoosingEnd++;
                return -1;
            }

            return _evaluate(gs, _imPlayerTwo);
        }
    }
}
