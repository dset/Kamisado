using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public class GameState
    {

        public bool IsPlayerTwo { get; private set; }
        public Point[][] PiecePositions { get; private set; }
        public PieceColor?[,] BoardPositions { get; private set; }
        public Point? PieceToMove { get; private set; }

        public bool? PlayerTwoWinning
        {
            get
            {
                for (int i = 0; i < PiecePositions[0].Length; i++)
                {
                    if (PiecePositions[0][i].Y == 0)
                    {
                        return false;
                    }
                    else if (PiecePositions[1][i].Y == 7)
                    {
                        return true;
                    }
                }

                bool[][] visited = new bool[2][];
                visited[0] = new bool[8];
                visited[1] = new bool[8];

                if (PossibleMoves.Count == 1 && PieceToMove.Value.Equals(PossibleMoves.First.Value.End))
                {
                    int currentPieceIndex = (int)BoardPositions[PieceToMove.Value.Y, PieceToMove.Value.X].Value;
                    visited[Convert.ToInt32(IsPlayerTwo)][currentPieceIndex] = true;
                    GameState n = new GameState(this, new Move(PieceToMove.Value, PieceToMove.Value));
                    while (n.PossibleMoves.Count == 1 && n.PieceToMove.Value.Equals(n.PossibleMoves.First.Value.End))
                    {
                        currentPieceIndex = (int)n.BoardPositions[n.PieceToMove.Value.Y, n.PieceToMove.Value.X].Value;
                        if(visited[Convert.ToInt32(n.IsPlayerTwo)][currentPieceIndex])
                        {
                            return !PiecePositions[0].Contains(PieceToMove.Value);
                        }
                        else
                        {
                            visited[Convert.ToInt32(n.IsPlayerTwo)][currentPieceIndex] = true;
                        }
                    }
                }

                return null;
            }

            private set
            {

            }
        }

        private LinkedList<Move> _possibleMoves;
        public LinkedList<Move> PossibleMoves
        {
            get
            {
                if (_possibleMoves == null)
                {
                    _possibleMoves = GenerateNextMoves();
                }
                if (_possibleMoves.Count == 0)
                {
                    _possibleMoves.AddLast(new Move(PieceToMove.Value, PieceToMove.Value));
                }

                return _possibleMoves;
            }

            private set
            {
                _possibleMoves = value;
            }
        }

        protected LinkedList<Move> GenerateNextMoves()
        {
            if (PieceToMove == null)
            {
                LinkedList<Move> moves = new LinkedList<Move>();
                for (int i = 0; i < PiecePositions[0].Length; i++)
                {
                    Point p = PiecePositions[0][i];
                    LinkedList<Move> tmp = GenerateNextMovesPlayerOne(p);
                    foreach (var move in tmp)
                    {
                        moves.AddLast(move);
                    }
                }

                return moves;
            }
            else
            {
                if (IsPlayerTwo)
                {
                    return GenerateNextMovesPlayerTwo(PieceToMove.Value);
                }
                else
                {
                    return GenerateNextMovesPlayerOne(PieceToMove.Value);
                }
            }
        }

        public LinkedList<Move> GenerateNextMovesPlayerOne(Point piece)
        {
            LinkedList<Move> moves = new LinkedList<Move>();

            int col = piece.X - 1;
            int row = piece.Y - 1;

            while (col >= 0 && row >= 0 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                col--;
                row--;
            }

            col = piece.X;
            row = piece.Y - 1;

            while (row >= 0 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                row--;
            }

            col = piece.X + 1;
            row = piece.Y - 1;

            while (col < 8 && row >= 0 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                col++;
                row--;
            }

            return moves;
        }

        public LinkedList<Move> GenerateNextMovesPlayerTwo(Point piece)
        {
            LinkedList<Move> moves = new LinkedList<Move>();

            int col = piece.X - 1;
            int row = piece.Y + 1;

            while (col >= 0 && row < 8 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                col--;
                row++;
            }

            col = piece.X;
            row = piece.Y + 1;

            while (row < 8 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                row++;
            }

            col = piece.X + 1;
            row = piece.Y + 1;

            while (col < 8 && row < 8 && !BoardPositions[row, col].HasValue)
            {
                Move move = new Move(piece, new Point(col, row));
                moves.AddLast(move);
                col++;
                row++;
            }

            return moves;
        }

        public GameState()
        {
            IsPlayerTwo = false;

            PieceToMove = null;
            
            PiecePositions = new Point[2][];
            PiecePositions[0] = new Point[8];
            PiecePositions[1] = new Point[8];
            for (int i = 0; i < 8; i++)
            {
                PiecePositions[0][i] = new Point(i, 7);
                PiecePositions[1][7 - i] = new Point(i, 0);
            }

            BoardPositions = new PieceColor?[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardPositions[i, j] = null;
                }
            }
            
            //Player one starting positions
            BoardPositions[7, 0] = PieceColor.Brown;
            BoardPositions[7, 1] = PieceColor.Green;
            BoardPositions[7, 2] = PieceColor.Red;
            BoardPositions[7, 3] = PieceColor.Yellow;
            BoardPositions[7, 4] = PieceColor.Pink;
            BoardPositions[7, 5] = PieceColor.Purple;
            BoardPositions[7, 6] = PieceColor.Blue;
            BoardPositions[7, 7] = PieceColor.Orange;

            //Player two starting positions
            BoardPositions[0, 0] = PieceColor.Orange;
            BoardPositions[0, 1] = PieceColor.Blue;
            BoardPositions[0, 2] = PieceColor.Purple;
            BoardPositions[0, 3] = PieceColor.Pink;
            BoardPositions[0, 4] = PieceColor.Yellow;
            BoardPositions[0, 5] = PieceColor.Red;
            BoardPositions[0, 6] = PieceColor.Green;
            BoardPositions[0, 7] = PieceColor.Brown;
        }

        public GameState(GameState oldState, Move move)
        {
            IsPlayerTwo = !oldState.IsPlayerTwo;

            BoardPositions = new PieceColor?[8, 8];
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
        }

        public GameState(Point[][] piecePositions, Point pieceToMove, bool isPlayerTwo)
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
        }
    }
}
