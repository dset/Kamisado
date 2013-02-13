using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class GameOverEventArgs : EventArgs
    {
        public bool PlayerTwoWon { get; private set; }

        public GameOverEventArgs(bool playerTwoWon)
        {
            PlayerTwoWon = playerTwoWon;
        }
    }
}
