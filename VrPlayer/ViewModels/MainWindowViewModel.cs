using System.ComponentModel;
using System.Windows.Input;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Models.State;

namespace VrPlayer.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        #region Commands

        private readonly ICommand _keyBoardCommand;
        public ICommand KeyboardCommand
        {
            get { return _keyBoardCommand; }
        }

        #endregion

        public MainWindowViewModel(IApplicationState state, IApplicationConfig config )
        {
            _state = state;
            _config = config;

            _state.PropertyChanged += ChangeShortcutsMapping;
            _config.PropertyChanged += ChangeShortcutsMapping;
            
            ChangeShortcutsMapping(null, new PropertyChangedEventArgs("Keys"));

            //Commands
            _keyBoardCommand = new RelayCommand(ExecuteShortcut);
        }

        private void ChangeShortcutsMapping(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "Keys" 
                && propertyChangedEventArgs.PropertyName != "TrackerPlugin" 
                && propertyChangedEventArgs.PropertyName != "MediaPlugin") 
                return;
            
            _state.Shortcuts.Register(_config.KeyFieldOfViewMinus, new DelegateCommand(DecreaseFieldOfView));
            _state.Shortcuts.Register(_config.KeyFieldOfViewPlus, new DelegateCommand(IncreaseFieldOfView));
            _state.Shortcuts.Register(_config.KeyHorizontalOffsetMinus, new DelegateCommand(DecreaseHorizontalOffset));
            _state.Shortcuts.Register(_config.KeyHorizontalOffsetPlus, new DelegateCommand(IncreaseHorizontalOffset));
            _state.Shortcuts.Register(_config.KeyVerticalOffsetMinus, new DelegateCommand(DecreaseVerticalOffset));
            _state.Shortcuts.Register(_config.KeyVerticalOffsetPlus, new DelegateCommand(InreaseVerticalOffset));
            
            if(_state.TrackerPlugin != null && _state.TrackerPlugin.Content != null)
            {
                _state.Shortcuts.Register(_config.KeyMoveForward, _state.TrackerPlugin.Content.MoveForwardCommand);
                _state.Shortcuts.Register(_config.KeyMoveBackward, _state.TrackerPlugin.Content.MoveBackwardCommand);
                _state.Shortcuts.Register(_config.KeyMoveLeft, _state.TrackerPlugin.Content.MoveLeftCommand);
                _state.Shortcuts.Register(_config.KeyMoveRight, _state.TrackerPlugin.Content.MoveRightCommand);
                _state.Shortcuts.Register(_config.KeyMoveUp, _state.TrackerPlugin.Content.MoveUpCommand);
                _state.Shortcuts.Register(_config.KeyMoveDown, _state.TrackerPlugin.Content.MoveDownCommand);
                _state.Shortcuts.Register(_config.KeyTrackerCalibrate, _state.TrackerPlugin.Content.CalibrateCommand);
                //_state.Shortcuts.Register(_config.KeyTrackerCalibrate, _state.TrackerPlugin.Content.ResetCommand);
            }

            if (_state.MediaPlugin != null && _state.MediaPlugin.Content != null)
            {
                _state.Shortcuts.Register(_config.KeyPlayPause, _state.MediaPlugin.Content.PlayCommand);
                _state.Shortcuts.Register(_config.KeyStop, _state.MediaPlugin.Content.StopCommand);
                _state.Shortcuts.Register(_config.KeyPrevious, _state.MediaPlugin.Content.PreviousCommand);
                _state.Shortcuts.Register(_config.KeyNext, _state.MediaPlugin.Content.NextCommand);
                _state.Shortcuts.Register(_config.KeyLoop, _state.MediaPlugin.Content.LoopCommand);
            }
        }

        #region Commands

        private void ExecuteShortcut(object o)
        {
            var key = (Key)o;
            _state.Shortcuts.Execute(key);
        }

        private void DecreaseFieldOfView(object obj)
        {
            if(_config.CameraFieldOfView > 0)
                _config.CameraFieldOfView--;
        }

        private void IncreaseFieldOfView(object obj)
        {
            if (_config.CameraFieldOfView < 360)
                _config.CameraFieldOfView++;
        }

        private void DecreaseHorizontalOffset(object obj)
        {
            _config.ViewportsHorizontalOffset--;
        }

        private void IncreaseHorizontalOffset(object obj)
        {
            _config.ViewportsHorizontalOffset++;
        }

        private void DecreaseVerticalOffset(object obj)
        {
            _config.ViewportsVerticalOffset--;
        }

        private void InreaseVerticalOffset(object obj)
        {
            _config.ViewportsVerticalOffset++;
        }

        private void TrackerCalibrate(object o)
        {
            if (_state.TrackerPlugin != null && _state.TrackerPlugin.Content != null)
                _state.TrackerPlugin.Content.Calibrate();
        }

        private void MediaPlayPause(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            if (!_state.MediaPlugin.Content.IsPlaying)
                _state.MediaPlugin.Content.PlayCommand.Execute(null);
            else
                _state.MediaPlugin.Content.PauseCommand.Execute(null);
        }

        #endregion
    }
}