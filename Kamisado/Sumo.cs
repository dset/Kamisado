using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kamisado
{
    class Sumo : Piece
    {
        public Sumo(bool belongsToPlayerTwo, Point position, PieceColor color)
            : base(belongsToPlayerTwo, position, color)
        {
            MaxMoveLength = 5;
        }

        protected override List<IMove> GenerateStraightMoves(GameState state, int maxLength, int ystep)
        {
            int forwardY = Position.Y + ystep;
            if (0 <= forwardY && forwardY < 8 && state.BoardPositions[forwardY][Position.X] == null)
            {
                return base.GenerateStraightMoves(state, maxLength, ystep);
            }
            else
            {
                int forwardForwardY = forwardY + ystep;
                if (0 <= forwardForwardY && forwardForwardY < 8 && state.BoardPositions[forwardForwardY][Position.X] == null &&
                    !(state.BoardPositions[forwardY][Position.X] is Sumo) && BelongsToPlayerTwo != state.BoardPositions[forwardY][Position.X].BelongsToPlayerTwo)
                {
                    List<IMove> res = new List<IMove>();
                    res.Add(new SumoPushMove(state, this, new Point(Position.X, forwardY)));
                    return res;
                }
                else
                {
                    return new List<IMove>();
                }
            }
        }
    }
}
