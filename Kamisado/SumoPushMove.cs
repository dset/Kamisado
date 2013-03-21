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

        private int _numPushed;
        private Piece _oldPieceToMove;
        private IMove _oldLastMove;

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
            _numPushed = 0;
            _oldPieceToMove = _state.PieceToMove;
            _oldLastMove = _state.LastMove;
            _start = _piece.Position;

            int ystep = _end.Y - _start.Y;

            _state.BoardPositions[_piece.Position.Y][_piece.Position.X] = null;
            Piece currentPiece = _piece;
            while (_state.BoardPositions[currentPiece.Position.Y + ystep][currentPiece.Position.X] != null)
            {
                Piece next = _state.BoardPositions[currentPiece.Position.Y + ystep][currentPiece.Position.X];
                _state.BoardPositions[currentPiece.Position.Y + ystep][currentPiece.Position.X] = currentPiece;
                currentPiece.Position = new Point(currentPiece.Position.X, currentPiece.Position.Y + ystep);
                currentPiece = next;
                _numPushed++;
            }

            _state.BoardPositions[currentPiece.Position.Y + ystep][currentPiece.Position.X] = currentPiece;
            currentPiece.Position = new Point(currentPiece.Position.X, currentPiece.Position.Y + ystep);

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = currentPiece;
            _state.LastMove = this;

            List<IMove> possible = new List<IMove>();
            possible.Add(new Move(_state, currentPiece, currentPiece.Position));
            _state.PossibleMoves = possible;

            return _state;
        }

        public GameState Reverse()
        {
            int ystep = _end.Y - _start.Y;

            int currentY = _piece.Position.Y + ystep * _numPushed;
            Piece currentPiece = _state.BoardPositions[currentY][_piece.Position.X];
            _state.BoardPositions[currentY][_piece.Position.X] = null;
            while (_state.BoardPositions[currentPiece.Position.Y - ystep][currentPiece.Position.X] != null)
            {
                Piece next = _state.BoardPositions[currentPiece.Position.Y - ystep][currentPiece.Position.X];
                _state.BoardPositions[currentPiece.Position.Y - ystep][currentPiece.Position.X] = currentPiece;
                currentPiece.Position = new Point(currentPiece.Position.X, currentPiece.Position.Y - ystep);
                currentPiece = next;
            }

            _state.BoardPositions[_start.Y][_start.X] = _piece;
            _piece.Position = _start;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _oldPieceToMove;
            _state.LastMove = _oldLastMove;
            _state.PossibleMoves = null;

            return _state;
        }

        public override string ToString()
        {
            return "Sumo Push. Color " + _piece.Color + ", Start " + _start + ", End " + _end;
        }
    }
}
