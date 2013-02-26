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
            GameState gs = new GameState();
            List<IMove> tmp = gs.PossibleMoves;
            List<IMove> possibleMoves = new List<IMove>();

            foreach (IMove m in tmp)
            {
                if (m.Piece.Position.X < 4)
                {
                    possibleMoves.Add(m);
                }
            }

            possibleMoves.Sort(new Comparison<IMove>((IMove m1, IMove m2) =>
            {
                return m1.End.Y - m2.End.Y;
            }));

            foreach (IMove move in possibleMoves)
            {
                System.IO.File.AppendAllText(@"C:\Users\Dan\Documents\Visual Studio 2012\Projects\Kamisado\IterativeDeepening\bin\Debug\Output.txt", "Starting move " + move + Environment.NewLine);
                Console.WriteLine("Starting move " + move);

                for (int depth = 1; depth <= 30; depth++)
                {
                    GameState smallState = new GameState();
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
