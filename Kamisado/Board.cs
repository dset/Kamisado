using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class Board
    {
        public static readonly PieceColor[,] Tile;

        static Board()
        {
            Board.Tile = new PieceColor[8, 8];
            Board.Tile[0, 0] = PieceColor.Orange;
            Board.Tile[0, 1] = PieceColor.Blue;
            Board.Tile[0, 2] = PieceColor.Purple;
            Board.Tile[0, 3] = PieceColor.Pink;
            Board.Tile[0, 4] = PieceColor.Yellow;
            Board.Tile[0, 5] = PieceColor.Red;
            Board.Tile[0, 6] = PieceColor.Green;
            Board.Tile[0, 7] = PieceColor.Brown;

            Board.Tile[1, 0] = PieceColor.Red;
            Board.Tile[1, 1] = PieceColor.Orange;
            Board.Tile[1, 2] = PieceColor.Pink;
            Board.Tile[1, 3] = PieceColor.Green;
            Board.Tile[1, 4] = PieceColor.Blue;
            Board.Tile[1, 5] = PieceColor.Yellow;
            Board.Tile[1, 6] = PieceColor.Brown;
            Board.Tile[1, 7] = PieceColor.Purple;

            Board.Tile[2, 0] = PieceColor.Green;
            Board.Tile[2, 1] = PieceColor.Pink;
            Board.Tile[2, 2] = PieceColor.Orange;
            Board.Tile[2, 3] = PieceColor.Red;
            Board.Tile[2, 4] = PieceColor.Purple;
            Board.Tile[2, 5] = PieceColor.Brown;
            Board.Tile[2, 6] = PieceColor.Yellow;
            Board.Tile[2, 7] = PieceColor.Blue;

            Board.Tile[3, 0] = PieceColor.Pink;
            Board.Tile[3, 1] = PieceColor.Purple;
            Board.Tile[3, 2] = PieceColor.Blue;
            Board.Tile[3, 3] = PieceColor.Orange;
            Board.Tile[3, 4] = PieceColor.Brown;
            Board.Tile[3, 5] = PieceColor.Green;
            Board.Tile[3, 6] = PieceColor.Red;
            Board.Tile[3, 7] = PieceColor.Yellow;

            Board.Tile[4, 0] = PieceColor.Yellow;
            Board.Tile[4, 1] = PieceColor.Red;
            Board.Tile[4, 2] = PieceColor.Green;
            Board.Tile[4, 3] = PieceColor.Brown;
            Board.Tile[4, 4] = PieceColor.Orange;
            Board.Tile[4, 5] = PieceColor.Blue;
            Board.Tile[4, 6] = PieceColor.Purple;
            Board.Tile[4, 7] = PieceColor.Pink;

            Board.Tile[5, 0] = PieceColor.Blue;
            Board.Tile[5, 1] = PieceColor.Yellow;
            Board.Tile[5, 2] = PieceColor.Brown;
            Board.Tile[5, 3] = PieceColor.Purple;
            Board.Tile[5, 4] = PieceColor.Red;
            Board.Tile[5, 5] = PieceColor.Orange;
            Board.Tile[5, 6] = PieceColor.Pink;
            Board.Tile[5, 7] = PieceColor.Green;

            Board.Tile[6, 0] = PieceColor.Purple;
            Board.Tile[6, 1] = PieceColor.Brown;
            Board.Tile[6, 2] = PieceColor.Yellow;
            Board.Tile[6, 3] = PieceColor.Blue;
            Board.Tile[6, 4] = PieceColor.Green;
            Board.Tile[6, 5] = PieceColor.Pink;
            Board.Tile[6, 6] = PieceColor.Orange;
            Board.Tile[6, 7] = PieceColor.Red;

            Board.Tile[7, 0] = PieceColor.Brown;
            Board.Tile[7, 1] = PieceColor.Green;
            Board.Tile[7, 2] = PieceColor.Red;
            Board.Tile[7, 3] = PieceColor.Yellow;
            Board.Tile[7, 4] = PieceColor.Pink;
            Board.Tile[7, 5] = PieceColor.Purple;
            Board.Tile[7, 6] = PieceColor.Blue;
            Board.Tile[7, 7] = PieceColor.Orange;
        }
    }
}
