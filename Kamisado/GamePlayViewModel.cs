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
        public GameState CurrentState
        {
            get
            {
                return _engine.CurrentState;
            }

            private set
            {
                
            }
        }

        private GameEngine _engine;
        private Point? _selectedPiece;

        public GamePlayViewModel(GameEngine engine)
        {
            _engine = engine;
            _selectedPiece = null;
            SelectTileCommand = new RelayCommand(param =>
            {
                if (engine.ActivePlayer is Human && !_selectedPiece.HasValue)
                {
                    Point chosenPoint = ParamToPoint(param);
                    if ((!engine.CurrentState.PieceToMove.HasValue && engine.CurrentState.PiecePositions[Convert.ToInt32(engine.CurrentState.IsPlayerTwo)].Contains(chosenPoint))
                        || (engine.CurrentState.PieceToMove.HasValue && chosenPoint.Equals(engine.CurrentState.PieceToMove.Value)))
                    {
                        _selectedPiece = chosenPoint;
                    }
                }
                else if (engine.ActivePlayer is Human)
                {
                    (engine.ActivePlayer as Human).ChosenMove = new Move(_selectedPiece.Value, ParamToPoint(param));
                    (engine.ActivePlayer as Human).GotMove = true;
                    _selectedPiece = null;
                }
            }, param => { return true; });
        }

        public void OnGameStateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("CurrentState");
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
