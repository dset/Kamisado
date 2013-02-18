using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Kamisado
{
    public class GameState
    {
        public static int numByDeadlock = 0;

        public bool IsPlayerTwo { get; private set; }
        public Piece[][] PiecePositions { get; private set; }
        public Piece[][] BoardPositions { get; private set; }
        public Piece PieceToMove { get; private set; }

        public bool? PlayerTwoWinning
        {
            get
            {
                for (int i = 0; i < PiecePositions[0].Length; i++)
                {
                    if (PiecePositions[0][i].Position.Y == 0)
                    {
                        return false;
                    }
                    else if (PiecePositions[1][i].Position.Y == 7)
                    {
                        return true;
                    }
                }

                bool[][] visited = new bool[2][];
                visited[0] = new bool[8];
                visited[1] = new bool[8];

                if (PossibleMoves.Count == 1 && PieceToMove.Position.Equals(PossibleMoves[0].End))
                {
                    LinkedList<Move> madeMoves = new LinkedList<Move>();

                    int currentPieceIndex = (int)PieceToMove.Color;
                    visited[Convert.ToInt32(IsPlayerTwo)][currentPieceIndex] = true;

                    Move m = new Move(PieceToMove.Position, PieceToMove.Position);
                    madeMoves.AddLast(m);
                    Move(m);
                    while (PossibleMoves.Count == 1 && PieceToMove.Position.Equals(PossibleMoves[0].End))
                    {
                        currentPieceIndex = (int)PieceToMove.Color;
                        if(visited[Convert.ToInt32(IsPlayerTwo)][currentPieceIndex])
                        {
                            GameState.numByDeadlock++;

                            while (madeMoves.Count > 0)
                            {
                                ReverseMove(madeMoves.Last.Value);
                                madeMoves.RemoveLast();
                            }

                            return PieceToMove.BelongsToPlayerTwo;
                        }
                        else
                        {
                            visited[Convert.ToInt32(IsPlayerTwo)][currentPieceIndex] = true;
                        }

                        madeMoves.AddLast(PossibleMoves[0]);
                        Move(PossibleMoves[0]);
                    }

                    while (madeMoves.Count > 0)
                    {
                        ReverseMove(madeMoves.Last.Value);
                        madeMoves.RemoveLast();
                    }
                }

                return null;
            }

            private set
            {

            }
        }

        private List<Move> _possibleMoves;
        public List<Move> PossibleMoves
        {
            get
            {
                if (_possibleMoves == null)
                {
                    _possibleMoves = GenerateNextMoves();
                }

                return _possibleMoves;
            }

            private set
            {
                
            }
        }

        protected List<Move> GenerateNextMoves()
        {
            if (PieceToMove == null)
            {
                List<Move> res = new List<Move>();
                for (int i = 0; i < PiecePositions[0].Length; i++)
                {
                    res.AddRange(PiecePositions[0][i].GetPossibleMoves(this));
                }

                return res;
            }
            else
            {
                return PieceToMove.GetPossibleMoves(this);
            }
        }

        public GameState()
        {
            IsPlayerTwo = false;

            PieceToMove = null;

            BoardPositions = new Piece[8][];
            for (int i = 0; i < 8; i++)
            {
                BoardPositions[i] = new Piece[8];
                for (int j = 0; j < 8; j++)
                {
                    BoardPositions[i][j] = null;
                }
            }
            
            PiecePositions = new Piece[2][];
            PiecePositions[0] = new Piece[8];
            PiecePositions[1] = new Piece[8];

            for (int i = 0; i < 8; i++)
            {
                PiecePositions[0][i] = new Piece(false, new Point(i, 7), (PieceColor)i);
                BoardPositions[7][i] = PiecePositions[0][i];

                PiecePositions[1][i] = new Piece(true, new Point(7 - i, 0), (PieceColor)i);
                BoardPositions[0][7 - i] = PiecePositions[1][i];

            }
        }

        public GameState Move(Move move)
        {
            if (PieceToMove == null)
            {
                PieceToMove = BoardPositions[move.Start.Y][move.Start.X];
            }

            IsPlayerTwo = !IsPlayerTwo;

            if (BoardPositions[move.End.Y][move.End.X] == null)
            {
                BoardPositions[move.Start.Y][move.Start.X] = null;
                BoardPositions[move.End.Y][move.End.X] = PieceToMove;
                PieceToMove.Position = move.End;

                PieceToMove = PiecePositions[Convert.ToInt32(IsPlayerTwo)][(int)Board.Tile[move.End.Y, move.End.X]];

                _possibleMoves = null;
            }
            else
            {
                Piece next = BoardPositions[move.Start.Y][move.Start.X];
                BoardPositions[move.Start.Y][move.Start.X] = null;
                int ystep = move.End.Y - move.Start.Y;

                while (BoardPositions[next.Position.Y + ystep][next.Position.X] != null)
                {
                    Piece tmp = BoardPositions[next.Position.Y + ystep][next.Position.X];
                    next.Position = new Point(next.Position.X, next.Position.Y + ystep);
                    BoardPositions[next.Position.Y + ystep][next.Position.X] = next;
                    next = tmp;
                }

                next.Position = new Point(next.Position.X, next.Position.Y + ystep);
                BoardPositions[next.Position.Y][next.Position.X] = next;

                PieceToMove = next;
                _possibleMoves = new List<Move>();
                _possibleMoves.Add(new Move(next.Position, next.Position));
            }

            return this;
        }

        public GameState ReverseMove(Move move)
        {
            IsPlayerTwo = !IsPlayerTwo;

            PieceToMove = BoardPositions[move.End.Y][move.End.X];
            BoardPositions[move.End.Y][move.End.X] = null;
            BoardPositions[move.Start.Y][move.Start.X] = PieceToMove;
            PieceToMove.Position = move.Start;

            _possibleMoves = null;

            return this;
        }

        /*public GameState(GameState oldState, Move move)
        {
            IsPlayerTwo = !oldState.IsPlayerTwo;

            BoardPositions = new Piece[8][];
            Array.Copy(oldState.BoardPositions, BoardPositions, 64);

            PieceColor? c = BoardPositions[move.Start.Y, move.Start.X];
            BoardPositions[move.Start.Y, move.Start.X] = null;
            BoardPositions[move.End.Y, move.End.X] = c;

            PiecePositions = new Point[2][];
            PiecePositions[0] = new Point[8];
            PiecePositions[1] = new Point[8];
            Array.Copy(oldState.PiecePositions[0], PiecePositions[0], 8);
            Array.Copy(oldState.PiecePositions[1], PiecePositions[1], 8);
            PiecePositions[Convert.ToInt32(oldState.IsPlayerTwo)][(int)c] = move.End;

            PieceColor endTileColor = Board.Tile[move.End.Y, move.End.X];
            PieceToMove = PiecePositions[Convert.ToInt32(IsPlayerTwo)][(int)endTileColor];
        }*/

        /*public GameState(Point[][] piecePositions, Point pieceToMove, bool isPlayerTwo)
        {
            PiecePositions = piecePositions;
            PieceToMove = pieceToMove;
            IsPlayerTwo = isPlayerTwo;

            BoardPositions = new PieceColor?[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardPositions[i, j] = null;
                }
            }

            foreach (PieceColor color in Enum.GetValues(typeof(PieceColor)))
            {
                int index = (int)color;
                Point p1Piece = piecePositions[0][index];
                Point p2Piece = piecePositions[1][index];

                BoardPositions[p1Piece.Y, p1Piece.X] = color;
                BoardPositions[p2Piece.Y, p2Piece.X] = color;
            }
        }*/
    }
}
