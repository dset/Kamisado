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

        public Bot(int maxDepth, Func<GameState, bool, double> evaluate)
        {
            _maxDepth = maxDepth;
            _evaluate = evaluate;
        }

        public MoveInfo GetMove(GameState currentState)
        {
            _imPlayerTwo = currentState.IsPlayerTwo;

            IMove[] moves = new IMove[currentState.PossibleMoves.Count];
            currentState.PossibleMoves.CopyTo(moves, 0);

            BranchInfo[] moveValues = new BranchInfo[moves.Length];

            for (int i = 0; i < moves.Length; i++)
            {
                moves[i].Execute();
                moveValues[i] = Min(currentState, 1, Double.MinValue, Double.MaxValue);
                moves[i].Reverse();
                Debug.WriteLine(moves[i] + " " + moveValues[i] + " ");
            }

            BranchInfo best = moveValues[0];
            IMove bestMove = moves[0];
            for (int i = 0; i < moves.Length; i++)
            {
                if ((moveValues[i].Value > best.Value) ||
                    (best.Value <= Double.MinValue / 4 && best.Value == moveValues[i].Value && moveValues[i].Depth > best.Depth) ||
                    (best.Value >= Double.MaxValue / 4 && best.Value == moveValues[i].Value && moveValues[i].Depth < best.Depth))
                {
                    best = moveValues[i];
                    bestMove = moves[i];
                }
            }

            return new MoveInfo(bestMove, best.Value);
        }

        private BranchInfo Max(GameState gs, int depth, double alpha, double beta)
        {
            if (depth >= _maxDepth || gs.PlayerTwoWinning.HasValue)
            {
                return new BranchInfo(EvaluateGameState(gs), depth);
            }

            BranchInfo v = new BranchInfo(Double.MinValue, 0);

            IEnumerable<IMove> possibleMoves = gs.PossibleMoves.Reverse<IMove>();

            foreach (IMove move in possibleMoves)
            {
                move.Execute();

                BranchInfo current = Min(gs, depth + 1, alpha, beta);
                if ((current.Value > v.Value) ||
                    (v.Value <= Double.MinValue / 4 && v.Value == current.Value && current.Depth > v.Depth) ||
                    (v.Value >= Double.MaxValue / 4 && v.Value == current.Value && current.Depth < v.Depth))
                {
                    v = current;
                }

                move.Reverse();
                if (v.Value >= beta)
                {
                    return v;
                }

                alpha = Math.Max(alpha, v.Value);
            }

            return v;
        }

        private BranchInfo Min(GameState gs, int depth, double alpha, double beta)
        {
            if (depth >= _maxDepth || gs.PlayerTwoWinning.HasValue)
            {
                return new BranchInfo(EvaluateGameState(gs), depth);
            }

            BranchInfo v = new BranchInfo(Double.MaxValue, 0);

            IEnumerable<IMove> possibleMoves = gs.PossibleMoves.Reverse<IMove>();

            foreach (IMove move in possibleMoves)
            {
                move.Execute();

                BranchInfo current = Max(gs, depth + 1, alpha, beta);
                if ((current.Value < v.Value) ||
                    (v.Value <= Double.MinValue / 4 && v.Value == current.Value && current.Depth < v.Depth) ||
                    (v.Value >= Double.MaxValue / 4 && v.Value == current.Value && current.Depth > v.Depth))
                {
                    v = current;
                }

                move.Reverse();
                if (v.Value <= alpha)
                {
                    return v;
                }

                beta = Math.Min(beta, v.Value);
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
