using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kamisado;

namespace IterativeDeepening
{
    class Program
    {
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            List<Piece> pieces = new List<Piece>();
            pieces.Add(new Piece(false, new System.Drawing.Point(0, 7), PieceColor.Blue, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(1, 7), PieceColor.Yellow, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(2, 7), PieceColor.Brown, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(3, 7), PieceColor.Pink, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(4, 7), PieceColor.Green, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(5, 7), PieceColor.Red, 0));
            pieces.Add(new Piece(false, new System.Drawing.Point(6, 7), PieceColor.Orange, 1));
            pieces.Add(new Piece(false, new System.Drawing.Point(7, 7), PieceColor.Purple, 3));

            pieces.Add(new Piece(true, new System.Drawing.Point(0, 0), PieceColor.Yellow, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(1, 0), PieceColor.Purple, 2));
            pieces.Add(new Piece(true, new System.Drawing.Point(2, 0), PieceColor.Green, 1));
            pieces.Add(new Piece(true, new System.Drawing.Point(3, 0), PieceColor.Orange, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(4, 0), PieceColor.Red, 1));
            pieces.Add(new Piece(true, new System.Drawing.Point(5, 0), PieceColor.Blue, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(6, 0), PieceColor.Pink, 0));
            pieces.Add(new Piece(true, new System.Drawing.Point(7, 0), PieceColor.Brown, 2));

            GameState startState = new GameState(pieces, null);
            List<IMove> possibleMoves = startState.PossibleMoves;

            possibleMoves.Sort(new Comparison<IMove>((IMove m1, IMove m2) =>
            {
                return m1.End.Y - m2.End.Y;
            }));

            foreach (IMove move in possibleMoves)
            {
                System.IO.File.AppendAllText(@"C:\Users\Dan\Documents\Visual Studio 2012\Projects\Kamisado\IterativeDeepening\bin\Debug\Output.txt", "Starting move " + move + Environment.NewLine);
                Console.WriteLine("Starting move " + move);

                for (int depth = 1; depth <= 20; depth++)
                {
                    GameState smallState = new GameState(pieces, null);
                    smallState.PossibleMoves = new List<IMove>();
                    smallState.PossibleMoves.Add(new Move(smallState, smallState.PiecePositions[move.Piece.BelongsToPlayerTwo ? 1 : 0][(int)move.Piece.Color],
                        new Point(move.End.X, move.End.Y)));

                    Bot bot = new Bot(depth, (GameState g, bool imPlayerTwo) => { return 0; });
                    MoveInfo info = bot.GetMove(smallState);

                    System.IO.File.AppendAllText(@"C:\Users\Dan\Documents\Visual Studio 2012\Projects\Kamisado\IterativeDeepening\bin\Debug\Output.txt",
                        move + " at depth " + depth + " had value " + info.Value + Environment.NewLine);
                    Console.WriteLine(move + " at depth " + depth + " had value " + info.Value);

                    if (info.Value != 0)
                    {
                        break;
                    }
                }

                System.IO.File.AppendAllText(@"C:\Users\Dan\Documents\Visual Studio 2012\Projects\Kamisado\IterativeDeepening\bin\Debug\Output.txt", Environment.NewLine);
                Console.WriteLine("");
            }
        }
    }
}
