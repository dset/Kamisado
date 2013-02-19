using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    public class Piece
    {
        public bool BelongsToPlayerTwo { get; private set; }
        public Point Position { get; set; }
        public PieceColor Color { get; private set; }
        public int MaxMoveLength { get; protected set; }

        public Piece(bool belongsToPlayerTwo, Point position, PieceColor color)
        {
            BelongsToPlayerTwo = belongsToPlayerTwo;
            Position = position;
            Color = color;
            MaxMoveLength = 7;
        }

        public List<IMove> GetPossibleMoves(GameState state)
        {
            List<IMove> res = new List<IMove>();

            int ystep = -1;
            if (BelongsToPlayerTwo)
            {
                ystep = 1;
            }

            res.AddRange(GenerateMovesOnLine(state, MaxMoveLength, -1, ystep));
            res.AddRange(GenerateStraightMoves(state, MaxMoveLength, ystep));
            res.AddRange(GenerateMovesOnLine(state, MaxMoveLength, 1, ystep));

            if (res.Count == 0)
            {
                res.Add(new Move(state, this, Position));
            }

            return res;
        }

        protected virtual List<IMove> GenerateStraightMoves(GameState state, int maxLength, int ystep)
        {
            return GenerateMovesOnLine(state, MaxMoveLength, 0, ystep);
        }

        protected List<IMove> GenerateMovesOnLine(GameState state, int maxLength, int xstep, int ystep)
        {
            List<IMove> res = new List<IMove>();

            int col = Position.X + xstep;
            int row = Position.Y + ystep;

            int count = 0;
            while (count < maxLength &&
                0 <= col && col < 8 &&
                0 <= row && row < 8 &&
                state.BoardPositions[row][col] == null)
            {
                Move move = new Move(state, this, new Point(col, row));
                res.Add(move);
                col += xstep;
                row += ystep;
                count++;
            }

            return res;
        }
    }
}
