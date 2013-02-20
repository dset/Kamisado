using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public class Move : IMove
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
                return _piece.Position.Equals(_end);
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

        public Move(GameState state, Piece piece, Point end)
        {
            _state = state;
            _piece = piece;
            _end = end;
        }

        public GameState Execute()
        {
            _oldPieceToMove = _state.PieceToMove;
            _start = _piece.Position;

            _state.BoardPositions[_start.Y][_start.X] = null;
            _state.BoardPositions[_end.Y][_end.X] = _piece;
            _piece.Position = _end;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _state.PiecePositions[_state.IsPlayerTwo ? 1 : 0][(int)Board.Tile[_end.Y, _end.X]];
            _state.PossibleMoves = null;

            return _state;
        }

        public GameState Reverse()
        {
            _state.BoardPositions[_end.Y][_end.X] = null;
            _state.BoardPositions[_start.Y][_start.X] = _piece;
            _piece.Position = _start;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _oldPieceToMove;
            _state.PossibleMoves = null;

            return _state;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            Move m = (Move)obj;

            return _piece == m._piece && _end.Equals(m._end);
        }

        public override string ToString()
        {
            return "Color " + _piece.Color + ", Start " + _piece.Position + ", End " + _end;
        }
    }
}
