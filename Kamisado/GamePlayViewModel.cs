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

        public GameState CurrentState { get; private set; }
        public RelayCommand SelectTileCommand { get; private set; }

        private Point? _selectedPiece;
        private Bot _player2;

        public GamePlayViewModel(GameState startState, Bot player2)
        {
            CurrentState = startState;
            _player2 = player2;
            _selectedPiece = null;
            SelectTileCommand = new RelayCommand(param =>
            {
                if (!_selectedPiece.HasValue && !CurrentState.IsPlayerTwo)
                {
                    Point chosen = ParamToPoint(param);
                    if (CurrentState.PiecePositions[Convert.ToInt32(CurrentState.IsPlayerTwo)].Contains(chosen))
                    {
                        _selectedPiece = chosen;
                    }
                }
                else if(!CurrentState.IsPlayerTwo)
                {
                    Move move = new Move(_selectedPiece.Value, ParamToPoint(param));
                    _selectedPiece = null;
                    if (CurrentState.PossibleMoves.Contains(move))
                    {
                        CurrentState = new GameState(CurrentState, move);
                        NotifyPropertyChanged("CurrentState");
                        if (CurrentState.PlayerTwoWinning.HasValue)
                        {
                            MessageBox.Show("Player two won: " + CurrentState.PlayerTwoWinning.Value);
                        }
                        else
                        {
                            CurrentState = new GameState(CurrentState, _player2.GetMove(CurrentState));
                            NotifyPropertyChanged("CurrentState");
                            if (CurrentState.PlayerTwoWinning.HasValue)
                            {
                                MessageBox.Show("Player two won: " + CurrentState.PlayerTwoWinning.Value);
                            }
                        }
                    }
                }
            }, param => { return true; });
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
