using System.Linq;
using System.Windows;
using System.Windows.Input;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.State;
using VrPlayer.Views.Settings;

namespace VrPlayer.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;

        public IApplicationState State
        {
            get { return _state; }
        }

        #region Commands

        private readonly ICommand _keyBoardCommand;
        public ICommand KeyboardCommand
        {
            get { return _keyBoardCommand; }
        }

        #endregion

        public MainWindowViewModel(IApplicationState state)
        {
            _state = state;

            //Commands
            _keyBoardCommand = new RelayCommand(ExecuteShortcut);
        }

        private void ExecuteShortcut(object o)
        {
            //Todo: Key mapping customization
            Key key = (Key)o;
            switch (key)
            {
                case Key.F1:
                    Calibrate();
                    break;
                case Key.MediaPlayPause:
                case Key.LeftCtrl:
                    SettingsWindow.ShowSingle();
                    break;
                default:
                    break;
            }
        }

        private void Calibrate()
        {
            if (_state.TrackerPlugin != null && _state.TrackerPlugin.Content != null)
                _state.TrackerPlugin.Content.Calibrate();
        }
	}
}