using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kamisado
{
    class GamePlayViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand SelectTileCommand { get; private set; }

        private GameState _displayState;
        public GameState CurrentState
        {
            get
            {
                return _displayState;
            }
        }

        private GameEngine _engine;
        private Piece _selectedPiece;

        public GamePlayViewModel(GameEngine engine)
        {
            _engine = engine;
            UpdateDisplayState();
            _selectedPiece = null;
            SelectTileCommand = new RelayCommand(param =>
            {
                if (engine.ActivePlayer is Human && _selectedPiece == null)
                {
                    Point chosenPoint = ParamToPoint(param);
                    if ((engine.CurrentState.PieceToMove == null && chosenPoint.Y == 7)
                        || (engine.CurrentState.PieceToMove != null && chosenPoint.Equals(engine.CurrentState.PieceToMove.Position)))
                    {
                        _selectedPiece = _engine.CurrentState.BoardPositions[chosenPoint.Y][chosenPoint.X];
                    }
                }
                else if (engine.ActivePlayer is Human)
                {
                    Point chosenPoint = ParamToPoint(param);
                    IMove chosenMove = null;
                    foreach (IMove move in engine.CurrentState.PossibleMoves)
                    {
                        if(move.Piece == _selectedPiece && move.End.Equals(chosenPoint))
                        {
                            chosenMove = move;
                            break;
                        }
                    }

                    if(chosenMove != null)
                    {
                        (engine.ActivePlayer as Human).ChosenMove = chosenMove;
                        (engine.ActivePlayer as Human).GotMove = true;
                        _selectedPiece = null;
                    }
                }
            }, param => { return true; });
        }

        public void OnGameStateChanged(object sender, EventArgs e)
        {
            UpdateDisplayState();
            NotifyPropertyChanged("CurrentState");
        }

        private void UpdateDisplayState()
        {
            _displayState = _engine.CurrentState.Copy();
        }

        public void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show("Player two won: " + e.PlayerTwoWon);
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Point ParamToPoint(object param)
        {
            string s = param.ToString();
            int y = Int32.Parse(s[0] + "");
            int x = Int32.Parse(s[2] + "");
            return new Point(x, y);
        }
    }
}
