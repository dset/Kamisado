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
                possible = currentState.PossibleMoves.Reverse();
            }
            else
            {
                possible = currentState.PossibleMoves.Reverse();
            }

            _imPlayerTwo = currentState.IsPlayerTwo;
            double currentMax = Double.MinValue;
            Move bestMove = currentState.PossibleMoves.First.Value;

            foreach (Move possibleMove in possible)
            {
                double v = Min(new GameState(currentState, possibleMove), 1);
                Debug.WriteLine("Move " + possibleMove.Start + " to " + possibleMove.End + " had value " + v);
                if (v > currentMax)
                {
                    bestMove = possibleMove;
                    currentMax = v;
                }
            }
            Debug.WriteLine("Best was: " + currentMax);
            Debug.WriteLine("Visited: " + _numVisited + ", Winning: " + _numWinningEnd + ", Loosing: " + _numLoosingEnd);
            return bestMove;
        }

        private double Max(GameState gs, int depth)
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
                v = Math.Max(v, Min(new GameState(gs, possibleMove), depth + 1));
            }

            return v;
        }

        private double Min(GameState gs, int depth)
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
                v = Math.Min(v, Max(new GameState(gs, possibleMove), depth + 1));
            }

            return v;
        }

        private Random rand = new Random();

        private double EvaluateGameState(GameState gs)
        {
            if (gs.PlayerTwoWinning.HasValue && ((gs.PlayerTwoWinning.Value && _imPlayerTwo) || (!gs.PlayerTwoWinning.Value && !_imPlayerTwo)))
            {
                _numWinningEnd++;
                return Double.MaxValue;
            }
            else if(gs.PlayerTwoWinning.HasValue)
            {
                _numLoosingEnd++;
                return Double.MinValue;
            }

            return _evaluate(gs, _imPlayerTwo);
        }
    }
}
