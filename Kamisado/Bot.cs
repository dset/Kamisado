﻿using System;
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
                            return new MoveInfo(moves[index], lastRowHighestValue, depth);
                        }
                    }
                }

                return new MoveInfo(moves[lastRowHighestValueIndices[0]], lastRowHighestValue, 1);
            }
            else
            {
                return new MoveInfo(moves[lastRowHighestValueIndices[0]], lastRowHighestValue, _maxDepth);
            }

            /*
            bool lastRowIsLoss = true;
            double lastRowHighestValue = Double.MinValue;
            for (int move = 0; move < moves.Length; move++)
            {
                if (moveValues[_maxDepth - 1][move] > Double.MinValue / 4)
                {
                    lastRowIsLoss = false;
                }

                if (moveValues[_maxDepth - 1][move] > lastRowHighestValue)
                {
                    lastRowHighestValue = moveValues[_maxDepth - 1][move];
                }
            }

            if (lastRowIsLoss)
            {
                IList<Int32> highestValuesIndices = new List<Int32>();
                for (int move = 0; move < moves.Length; move++)
                {
                    if (moveValues[_maxDepth - 1][move] == lastRowHighestValue)
                    {
                        highestValuesIndices.Add(move);
                    }
                }


                for (int depth = _maxDepth; depth > 0; depth--)
                {
                    foreach (int index in highestValuesIndices)
                    {
                        if (moveValues[depth - 1][index] > Double.MinValue / 4)
                        {
                            return new MoveInfo(moves[index], lastRowHighestValue, depth);
                        }
                    }
                }
            }

            if (lastRowHighestValue >= Double.MaxValue / 4)
            {
                Debug.WriteLine("Hello everypeople");
                Debug.WriteLine("LastRowHighest " + lastRowHighestValue);
                IList<Int32> highestValuesIndices = new List<Int32>();
                for (int move = 0; move < moves.Length; move++)
                {
                    if (moveValues[_maxDepth - 1][move] == lastRowHighestValue)
                    {
                        highestValuesIndices.Add(move);
                    }
                }

                Debug.WriteLine("List length " + highestValuesIndices.Count);

                Debug.WriteLine("This is the matrix");
                for (int k = 0; k < _maxDepth; k++)
                {
                    for (int j = 0; j < moves.Length; j++)
                    {
                        Debug.Write(moveValues[k][j] + " ");
                    }
                    Debug.WriteLine("");
                }

                int shortestIndex = -1;
                int shortestDepth = Int32.MaxValue;
                foreach (int index in highestValuesIndices)
                {
                    for (int depth = _maxDepth; depth > 0; depth--)
                    {
                        if (moveValues[depth - 1][index] < lastRowHighestValue || (moveValues[depth - 1][index] == lastRowHighestValue && depth == 1))
                        {
                            if (depth < shortestDepth)
                            {
                                shortestDepth = depth;
                                shortestIndex = index;
                            }
                            break;
                        }
                    }
                }

                return new MoveInfo(moves[shortestIndex], lastRowHighestValue, shortestDepth);
            }

            for (int move = 0; move < moves.Length; move++)
            {
                if (moveValues[_maxDepth - 1][move] == lastRowHighestValue)
                {
                    return new MoveInfo(moves[move], lastRowHighestValue, _maxDepth);
                }
            }

            throw new Exception("Should not be here");*/
        }

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
