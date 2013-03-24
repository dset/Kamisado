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
        public bool BelongsToPlayerTwo { get; set; }
        public Point Position { get; set; }
        public PieceColor Color { get; private set; }
        public int MaxMoveLength { get; protected set; }
        public int Sumoness { get; set; }

        public Piece(bool belongsToPlayerTwo, Point position, PieceColor color, int sumoness)
        {
            BelongsToPlayerTwo = belongsToPlayerTwo;
            Position = position;
            Color = color;
            MaxMoveLength = 7 - 2 * sumoness;
            Sumoness = sumoness;
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

        public Piece Copy()
        {
            return new Piece(BelongsToPlayerTwo, new Point(Position.X, Position.Y), Color, Sumoness);
        }

        protected virtual List<IMove> GenerateStraightMoves(GameState state, int maxLength, int ystep)
        {
            if (0 <= Position.Y + ystep && Position.Y + ystep < 8 && state.BoardPositions[Position.Y + ystep][Position.X] != null)
            {
                return GenerateSumoPushMoves(state, ystep);
            }
            else
            {
                return GenerateMovesOnLine(state, MaxMoveLength, 0, ystep);
            }
        }

        private List<IMove> GenerateSumoPushMoves(GameState state, int ystep)
        {
            if (Sumoness == 0)
            {
                return new List<IMove>();
            }

            int currentX = Position.X;
            int currentY = Position.Y;
            bool goodGuys = true;
            bool foundNull = false;

            for (int i = 0; i <= Sumoness; i++)
            {
                currentY += ystep;
                if (currentY < 0 || currentY >= 8)
                {
                    break;
                }

                if (state.BoardPositions[currentY][currentX] == null)
                {
                    foundNull = true;
                    break;
                }

                if (Sumoness <= state.BoardPositions[currentY][currentX].Sumoness ||
                    state.BoardPositions[currentY][currentX].BelongsToPlayerTwo == BelongsToPlayerTwo)
                {
                    goodGuys = false;
                    break;
                }
            }

            if (foundNull && goodGuys)
            {
                List<IMove> res = new List<IMove>();
                res.Add(new SumoPushMove(state, this, new Point(Position.X, Position.Y + ystep)));
                return res;
            }
            else
            {
                return new List<IMove>();
            }
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

        public override string ToString()
        {
            return "Piece: " + Enum.GetName(typeof(PieceColor), Color) + " " + Position + " " + Sumoness;
        }
    }
}
