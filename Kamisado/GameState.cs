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
        public IMove LastMove { get; set; }

        public static GameState GenerateNextRound(GameState endState, bool left)
        {
            List<Piece> oldPieces = new List<Piece>();
            oldPieces.AddRange(endState.PiecePositions[0]);
            oldPieces.AddRange(endState.PiecePositions[1]);
            List<Piece> newPiecesOne = new List<Piece>();
            List<Piece> newPiecesTwo = new List<Piece>();

            Piece winningPiece = endState.WinningPiece;

            oldPieces.Remove(winningPiece);
            foreach (Piece oldPiece in oldPieces)
            {
                if (oldPiece.BelongsToPlayerTwo)
                {
                    newPiecesTwo.Add(oldPiece.Copy());
                }
                else
                {
                    newPiecesOne.Add(oldPiece.Copy());
                }
            }
            if (winningPiece.BelongsToPlayerTwo)
            {
                Piece p = winningPiece.Copy();
                p.Sumoness += 1;
                newPiecesTwo.Add(p);
            }
            else
            {
                Piece p = winningPiece.Copy();
                p.Sumoness += 1;
                newPiecesOne.Add(p);
            }

            Comparison<Piece> comp = new Comparison<Piece>((Piece p1, Piece p2) =>
                {
                    if (p1.Position.Y == p2.Position.Y)
                    {
                        return (left ? 1 : -1) * (p1.Position.X - p2.Position.X);
                    }
                    else
                    {
                        return p2.Position.Y - p1.Position.Y;
                    }
                });

            newPiecesOne.Sort(comp);
            newPiecesTwo.Sort(comp);

            if (endState.PlayerTwoWinning.Value)
            {
                if (!left)
                {
                    newPiecesOne.Reverse();
                    newPiecesTwo.Reverse();
                }

                for (int i = 0; i < 8; i++)
                {
                    newPiecesOne[i].Position = new Point(i, 7);
                    newPiecesTwo[i].Position = new Point(i, 0);
                }
            }
            else
            {
                if (left)
                {
                    newPiecesOne.Reverse();
                    newPiecesTwo.Reverse();
                }

                for (int i = 0; i < 8; i++)
                {
                    newPiecesOne[i].BelongsToPlayerTwo = true;
                    newPiecesOne[i].Position = new Point(i, 0);

                    newPiecesTwo[i].BelongsToPlayerTwo = false;
                    newPiecesTwo[i].Position = new Point(i, 7);
                }
            }

            List<Piece> newPieces = new List<Piece>();
            newPieces.AddRange(newPiecesOne);
            newPieces.AddRange(newPiecesTwo);
            return new GameState(newPieces, null);
        }

        public Piece WinningPiece
        {
            get
            {
                Piece winningPiece;
                if (IsPieceWinning().HasValue)
                {
                    winningPiece = BoardPositions[LastMove.End.Y][LastMove.End.X];
                }
                else
                {
                    PieceColor winningColor = Board.Tile[LastMove.End.Y, LastMove.End.X];
                    winningPiece = PiecePositions[IsPlayerTwo ? 1 : 0][(int)winningColor];
                }

                return winningPiece;
            }
        }

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

        public GameState Copy()
        {
            List<Piece> pieces = new List<Piece>();
            Piece pieceToMove = null;
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(PiecePositions[0][i].Copy());
                pieces.Add(PiecePositions[1][i].Copy());
            }

            if (PieceToMove != null)
            {
                foreach (Piece p in pieces)
                {
                    if (p.Position.Equals(PieceToMove.Position))
                    {
                        pieceToMove = p;
                        break;
                    }
                }
            }

            return new GameState(pieces, pieceToMove);
        }

        public GameState()
        {
            IsPlayerTwo = false;

            PieceToMove = null;

            LastMove = null;

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
                PiecePositions[0][i] = new Piece(false, new Point(i, 7), (PieceColor)i, 0);
                BoardPositions[7][i] = PiecePositions[0][i];

                PiecePositions[1][i] = new Piece(true, new Point(7 - i, 0), (PieceColor)i, 0);
                BoardPositions[0][7 - i] = PiecePositions[1][i];

            }
        }

        public GameState(List<Piece> ps, Piece pToMove)
        {
            List<Piece> pieces = new List<Piece>();
            Piece pieceToMove = null;
            foreach (Piece p in ps)
            {
                pieces.Add(p.Copy());
            }
            if (pToMove != null)
            {
                foreach (Piece p in pieces)
                {
                    if (p.Position.Equals(pToMove.Position))
                    {
                        pieceToMove = p;
                        break;
                    }
                }
            }

            if (pieceToMove == null)
            {
                IsPlayerTwo = false;
                PieceToMove = null;
            }
            else
            {
                IsPlayerTwo = pieceToMove.BelongsToPlayerTwo;
                PieceToMove = pieceToMove;
            }

            LastMove = null;

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

            foreach (Piece piece in pieces)
            {
                BoardPositions[piece.Position.Y][piece.Position.X] = piece;
                PiecePositions[piece.BelongsToPlayerTwo ? 1 : 0][(int)piece.Color] = piece;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Challenger pieces:");
            foreach (Piece challengerPiece in PiecePositions[0])
            {
                sb.Append(challengerPiece + ", ");
            }

            sb.AppendLine();

            sb.AppendLine("Defender pieces:");
            foreach (Piece defenderPiece in PiecePositions[1])
            {
                sb.Append(defenderPiece + ", ");
            }

            sb.AppendLine();

            return sb.ToString();
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
