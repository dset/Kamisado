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
        public bool IsPlayerTwo { get; set; }
        public Piece[][] PiecePositions { get; set; }
        public Piece[][] BoardPositions { get; set; }
        public Piece PieceToMove { get; set; }

        public bool? PlayerTwoWinning
        {
            get
            {
                bool? isPieceWinning = IsPieceWinning();
                if (isPieceWinning.HasValue)
                {
                    return isPieceWinning.Value;
                }

                bool? isDeadlockWinning = IsDeadlockWinning();
                if (isDeadlockWinning.HasValue)
                {
                    return isDeadlockWinning.Value;
                }

                return null;
            }
        }

        private bool? IsPieceWinning()
        {
            foreach (Piece player1Piece in PiecePositions[0])
            {
                if (player1Piece.Position.Y == 0)
                {
                    return false;
                }
            }

            foreach (Piece player2Piece in PiecePositions[1])
            {
                if (player2Piece.Position.Y == 7)
                {
                    return true;
                }
            }

            return null;
        }

        private bool? IsDeadlockWinning()
        {
            if (PieceToMove == null)
            {
                return null;
            }

            bool[][] visited = new bool[2][];
            visited[0] = new bool[8];
            visited[1] = new bool[8];

            Piece piece = PieceToMove;
            List<IMove> moves = PossibleMoves;
            while (moves.Count == 1 && moves[0].IsTrivial)
            {
                if (visited[piece.BelongsToPlayerTwo ? 1 : 0][(int)piece.Color])
                {
                    return PieceToMove.BelongsToPlayerTwo;
                }
                else
                {
                    visited[piece.BelongsToPlayerTwo ? 1 : 0][(int)piece.Color] = true;
                }

                piece = PiecePositions[piece.BelongsToPlayerTwo ? 0 : 1][(int)Board.Tile[piece.Position.Y, piece.Position.X]];
                moves = piece.GetPossibleMoves(this);
            }

            return null;
        }

        private List<IMove> _possibleMoves;
        public List<IMove> PossibleMoves
        {
            get
            {
                if (_possibleMoves == null)
                {
                    _possibleMoves = GenerateNextMoves();
                }

                return _possibleMoves;
            }

            set
            {
                _possibleMoves = value;
            }
        }

        protected List<IMove> GenerateNextMoves()
        {
            if (PieceToMove == null)
            {
                List<IMove> res = new List<IMove>();
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
                if (i == 0)
                {
                    PiecePositions[0][i] = new Sumo(false, new Point(i, 7), (PieceColor)i);
                }
                BoardPositions[7][i] = PiecePositions[0][i];

                PiecePositions[1][i] = new Piece(true, new Point(7 - i, 0), (PieceColor)i);
                BoardPositions[0][7 - i] = PiecePositions[1][i];

            }
        }

        /*
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
         * */
    }
}
