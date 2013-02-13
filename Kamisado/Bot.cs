using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    class Bot : IPlayer
    {
        private struct Action
        {
            public Move Move { get; private set; }
            public double Value { get; private set; }

            public Action(Move move, double value) : this()
            {
                Move = move;
                Value = value;
            }
        }

        private int _maxDepth;
        private bool _imPlayerTwo;

        public Bot()
        {
            _maxDepth = 5;
        }

        public Move GetMove(GameState currentState)
        {
            _imPlayerTwo = currentState.IsPlayerTwo;
            double currentMax = Double.MinValue;
            Move bestMove = currentState.PossibleMoves.First.Value;
            foreach (Move possibleMove in currentState.PossibleMoves)
            {
                double val = Min(new GameState(currentState, possibleMove), 1);
                if (val > currentMax)
                {
                    bestMove = possibleMove;
                    currentMax = val;
                }
            }

            return bestMove;
        }

        private double Max(GameState gs, int depth)
        {
            if (depth == _maxDepth)
            {
                return EvaluateGameState(gs);
            }

            if (gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double alpha = -1000;
            foreach(Move possibleMove in gs.PossibleMoves)
            {
                alpha = Math.Max(alpha, Min(new GameState(gs, possibleMove), depth + 1));
            }

            return alpha;
        }

        private double Min(GameState gs, int depth)
        {
            if (depth == _maxDepth)
            {
                return EvaluateGameState(gs);
            }

            if (gs.PlayerTwoWinning.HasValue)
            {
                return EvaluateGameState(gs);
            }

            double alpha = 1000;
            foreach (Move possibleMove in gs.PossibleMoves)
            {
                alpha = Math.Min(alpha, Max(new GameState(gs, possibleMove), depth + 1));
            }

            return alpha;
        }

        private Random rand = new Random();

        private double EvaluateGameState(GameState gs)
        {
            if (gs.PlayerTwoWinning.HasValue && ((gs.PlayerTwoWinning.Value && _imPlayerTwo) || (!gs.PlayerTwoWinning.Value && !_imPlayerTwo)))
            {
                return 100;
            }
            else if(gs.PlayerTwoWinning.HasValue)
            {
                return -100;
            }

            double diffStriking = 0;
            foreach (Point myPiece in gs.PiecePositions[Convert.ToInt32(_imPlayerTwo)])
            {
                LinkedList<Move> possibleMoves;
                if (_imPlayerTwo)
                {
                    possibleMoves = gs.GenerateNextMovesPlayerTwo(myPiece);
                }
                else
                {
                    possibleMoves = gs.GenerateNextMovesPlayerOne(myPiece);
                }

                foreach (Move m in possibleMoves)
                {
                    if (_imPlayerTwo && m.End.Y == 7)
                    {
                        diffStriking += 1.3;
                    }
                    else if (!_imPlayerTwo && m.End.Y == 0)
                    {
                        diffStriking += 1.3;
                    }
                }
            }

            foreach (Point hensPiece in gs.PiecePositions[Convert.ToInt32(!_imPlayerTwo)])
            {
                LinkedList<Move> possibleMoves;
                if (!_imPlayerTwo)
                {
                    possibleMoves = gs.GenerateNextMovesPlayerTwo(hensPiece);
                }
                else
                {
                    possibleMoves = gs.GenerateNextMovesPlayerOne(hensPiece);
                }

                foreach (Move m in possibleMoves)
                {
                    if (_imPlayerTwo && m.End.Y == 0)
                    {
                        diffStriking--;
                    }
                    else if (!_imPlayerTwo && m.End.Y == 7)
                    {
                        diffStriking--;
                    }
                }
            }

            return diffStriking;
        }
    }
}
