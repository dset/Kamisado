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

        public Bot(int maxDepth, Func<GameState, bool, double> evaluate)
        {
            _maxDepth = maxDepth;
            _evaluate = evaluate;
        }

        public Move GetMove(GameState currentState)
        {
            _imPlayerTwo = currentState.IsPlayerTwo;

            Move[] moves = new Move[currentState.PossibleMoves.Count];
            currentState.PossibleMoves.CopyTo(moves, 0);

            //moves = new Move[1];
            //moves[0] = new Move(new Point(3, 7), new Point(3, 3));

            double[] moveValues = new double[moves.Length];

            for (int i = 0; i < moves.Length; i++)
            {
                moveValues[i] = Min(new GameState(currentState, moves[i]), 1, Double.MinValue, Double.MaxValue);
            }

            Debug.WriteLine("");
            for (int i = 0; i < moveValues.Length; i++)
            {
                Debug.WriteLine(moveValues[i] + " ");
            }
            Debug.WriteLine("");

            double best = Double.MinValue;
            Move bestMove = moves[0];
            for (int i = 0; i < moves.Length; i++)
            {
                if (moveValues[i] > best)
                {
                    best = moveValues[i];
                    bestMove = moves[i];
                }
            }

            return bestMove;
        }

        private double Max(GameState gs, int depth, double alpha, double beta)
        {
            if (depth >= _maxDepth || gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double v = Double.MinValue;

            foreach (Move move in gs.PossibleMoves.Reverse())
            {
                v = Math.Max(v, Min(new GameState(gs, move), depth + 1, alpha, beta));
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

            foreach (Move move in gs.PossibleMoves.Reverse())
            {
                v = Math.Min(v, Max(new GameState(gs, move), depth + 1, alpha, beta));
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
                return Double.MaxValue;
            }
            else if (gs.PlayerTwoWinning.HasValue)
            {
                return Double.MinValue;
            }

            return _evaluate(gs, _imPlayerTwo);
        }
    }
}
