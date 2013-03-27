using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public class SumoDazedMove : IMove
    {
        private GameState _state;
        private Piece _piece;
        private Point _start;
        private Point _end;

        private Piece _oldPieceToMove;
        private IMove _oldLastMove;

        public bool IsTrivial
        {
            get
            {
                return true;
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

        public SumoDazedMove(GameState state, Piece piece)
        {
            _state = state;
            _piece = piece;
            _end = _piece.Position;
            _start = _piece.Position;
        }

        public GameState Execute()
        {
            _oldPieceToMove = _state.PieceToMove;
            _oldLastMove = _state.LastMove;

            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _state.PiecePositions[_state.IsPlayerTwo ? 1 : 0][(int)Board.Tile[_end.Y, _end.X]];
            _state.PossibleMoves = null;
            _state.LastMove = this;

            return _state;
        }

        public GameState Reverse()
        {
            _state.IsPlayerTwo = !_state.IsPlayerTwo;
            _state.PieceToMove = _oldPieceToMove;
            _state.LastMove = _oldLastMove;

            List<IMove> possible = new List<IMove>();
            possible.Add(new SumoDazedMove(_state, _piece));
            _state.PossibleMoves = possible;

            return _state;
        }

        public override string ToString()
        {
            return "Sumo Dazed Move. Color " + _piece.Color + ", Start " + _start + ", End " + _end;
        }
    }
}