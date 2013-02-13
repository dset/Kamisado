using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Kamisado;

namespace KamisadoTests
{
    [TestClass]
    public class GameStateTests
    {
        [TestMethod]
        public void TestShouldHave102OpeningMoves()
        {
            GameState gs = new GameState();
            Assert.AreEqual(102, gs.NextStates.Count);
        }

        [TestMethod]
        public void TestSmallAmountOfMoves()
        {
            Point[][] pos = new Point[2][];
            pos[0] = new Point[8];
            pos[1] = new Point[8];
            pos[0][0] = new Point(0, 7);
            pos[0][1] = new Point(1, 2);
            pos[0][2] = new Point(2, 2);
            pos[0][3] = new Point(3, 7);
            pos[0][4] = new Point(1, 4);
            pos[0][5] = new Point(5, 1);
            pos[0][6] = new Point(6, 1);
            pos[0][7] = new Point(7, 7);

            pos[1][0] = new Point(7, 4);
            pos[1][1] = new Point(6, 0);
            pos[1][2] = new Point(4, 1);
            pos[1][3] = new Point(4, 5);
            pos[1][4] = new Point(3, 5);
            pos[1][5] = new Point(2, 0);
            pos[1][6] = new Point(1, 0);
            pos[1][7] = new Point(0, 3);

            Point tomove = new Point(1, 4);

            GameState gs = new GameState(pos, tomove, false);

            Assert.AreEqual(3, gs.NextStates.Count);
        }

        [TestMethod]
        public void TestMove()
        {
            Point[][] pos = new Point[2][];
            pos[0] = new Point[8];
            pos[1] = new Point[8];
            pos[0][0] = new Point(0, 7);
            pos[0][1] = new Point(1, 2);
            pos[0][2] = new Point(2, 2);
            pos[0][3] = new Point(3, 7);
            pos[0][4] = new Point(1, 4);
            pos[0][5] = new Point(5, 1);
            pos[0][6] = new Point(6, 1);
            pos[0][7] = new Point(7, 7);

            pos[1][0] = new Point(7, 4);
            pos[1][1] = new Point(6, 0);
            pos[1][2] = new Point(4, 1);
            pos[1][3] = new Point(4, 5);
            pos[1][4] = new Point(3, 5);
            pos[1][5] = new Point(2, 0);
            pos[1][6] = new Point(1, 0);
            pos[1][7] = new Point(0, 3);

            Point tomove = new Point(1, 4);

            GameState gs = new GameState(pos, tomove, false);
            Assert.IsFalse(gs.IsPlayerTwo);

            GameState gs2 = new GameState(gs, new Move(new Point(1, 4), new Point(3, 2)));
            Assert.IsTrue(gs2.IsPlayerTwo);
            Assert.AreEqual(5, gs2.NextStates.Count);

            GameState gs3 = new GameState(gs2, new Move(new Point(4, 1), new Point(5, 2)));
            Assert.IsFalse(gs3.IsPlayerTwo);
            Assert.AreEqual(7, gs3.NextStates.Count);
        }
    }
}
