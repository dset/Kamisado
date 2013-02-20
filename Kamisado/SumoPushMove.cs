using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    class SumoPushMove : IMove
    {
        private GameState _state;
        private Piece _piece;
        private Point _start;
        private Point _end;

        private Piece _oldPieceToMove;

        public bool IsTrivial
        {
            get
            {
                return false;
            }
        }

        public Piece Piece
        {
            get
            {
                return _piece;
            }
        }

        public Point End
        {
            get
            {
                return _end;
            }
        }

        public SumoPushMove(GameState state, Piece piece, Point end)
        {
            _state = state;
            _piece = piece;
            _end = end;
        }

        public GameState Execute()
        {
            _oldPieceToMove = _state.PieceToMove;
            _start = _piece.Position;

            int ystep = _end.Y - _start.Y;

            Piece otherPiece = _state.BoardPositions[_start.Y + ystep][_start.X];
            _state.BoardPositions[_start.Y + ystep + ystep][_start.X] = otherPiece;
            _state.BoardPositions[_start.Y + ystep][_start.X] = _piece;
            _state.BoardPositions[_start.Y][_start.X] = null;

            otherPiece.Position = new Point(_start.X, _start.Y + ystep + ystep);
            _piece.Position = _end;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = otherPiece;

            List<IMove> possible = new List<IMove>();
            possible.Add(new Move(_state, otherPiece, otherPiece.Position));
            _state.PossibleMoves = possible;

            return _state;
        }

        public GameState Reverse()
        {
            int ystep = _end.Y - _start.Y;

            Piece otherPiece = _state.BoardPositions[_start.Y + ystep + ystep][_start.X];
            _state.BoardPositions[_start.Y + ystep + ystep][_start.X] = null;
            _state.BoardPositions[_start.Y + ystep][_start.X] = otherPiece;
            _state.BoardPositions[_start.Y][_start.X] = _piece;

            otherPiece.Position = new Point(_start.X, _start.Y + ystep);
            _piece.Position = _start;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _oldPieceToMove;
            _state.PossibleMoves = null;

            return _state;
        }

        public override string ToString()
        {
            return "Sumo Push. Color " + _piece.Color + ", Start " + _piece.Position + ", End " + _end;
        }
    }
}
