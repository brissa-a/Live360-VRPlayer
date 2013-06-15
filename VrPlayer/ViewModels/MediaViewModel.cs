using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.State;
using System.Windows.Media.Effects;

namespace VrPlayer.ViewModels
{
	public class MediaViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        #region Fields

        private bool _hasDuration;
        public bool HasDuration
        {
            get
            {
                return _hasDuration;
            }
            set
            {
                _hasDuration = value;
                OnPropertyChanged("HasDuration");
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        private double _progress;
        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        #endregion

        #region Commands

        private readonly ICommand _playCommand;
        public ICommand PlayCommand
        {
            get { return _playCommand; }
        }

        private readonly ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get { return _pauseCommand; }
        }

        private readonly ICommand _stopCommand;
        public ICommand StopCommand
        {
            get { return _stopCommand; }
        }

        private readonly ICommand _seekCommand;
        public ICommand SeekCommand
        {
            get { return _seekCommand; }
        }

        private readonly ICommand _setEffectCommand;
        public ICommand SetEffectCommand
        {
            get { return _setEffectCommand; }
        }

        private readonly ICommand _loopCommand;
        public ICommand LoopCommand
        {
            get { return _loopCommand; }
        }

        #endregion

        public MediaViewModel(IApplicationState state)
        {
            _state = state;

            //Todo: VM should not register events in the model
            _state.MediaPlayer.MediaOpened += new RoutedEventHandler(_media_MediaOpened);
            _state.MediaPlayer.MediaEnded += new RoutedEventHandler(_media_MediaEnded);
            _state.MediaPlayer.SourceUpdated += new EventHandler<DataTransferEventArgs>(_media_SourceUpdated);

            //Commands
            _playCommand = new RelayCommand(Play, CanPlay);
            _pauseCommand = new RelayCommand(Pause, CanPause);
            _stopCommand = new RelayCommand(Stop, CanStop);
            _seekCommand = new RelayCommand(Seek, CanSeek);
            _setEffectCommand = new RelayCommand(SetEffect);
            _loopCommand = new RelayCommand(Loop);

            var timer = new DispatcherTimer(DispatcherPriority.DataBind);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += timer_Tick;
            timer.Start();
		}

        #region Events

        void timer_Tick(object sender, EventArgs e)
        {
            if (_state.MediaPlayer.MediaDuration > 0)
            {
                var ratio = _state.MediaPlayer.MediaPosition/(double) _state.MediaPlayer.MediaDuration;
                Progress = ratio * 100;

                //Set Stabilizer plugin frame
                var frame = (int)Math.Round(_state.StabilizerPlugin.Content.GetFramesCount() * ratio);
                _state.StabilizerPlugin.Content.UpdateCurrentFrame(frame);
                //Debug:
                Logger.Instance.Info(
                    frame.ToString() + "/" + _state.StabilizerPlugin.Content.GetFramesCount() 
                    + " (" + _state.MediaPlayer.MediaPosition + "/" + _state.MediaPlayer.MediaDuration + ") ->" + ratio);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        void _media_MediaOpened(object sender, RoutedEventArgs e)
        {
            HasDuration = _state.MediaPlayer.MediaDuration > 0;
            StopCommand.Execute(null);
            PlayCommand.Execute(null);
        }

        void _media_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            HasDuration = _state.MediaPlayer.MediaDuration > 0;
            StopCommand.Execute(null);
            PlayCommand.Execute(null);
        }

        void _media_MediaEnded(object sender, RoutedEventArgs e)
        {
            StopCommand.Execute(null);
        }

        #endregion

        #region Logic

        private void Play(object o)
        {
            if (CanPlay(o))
            {
                _state.MediaPlayer.Play();
                IsPlaying = true;
            }
        }

        private bool CanPlay(object o)
        {
            return HasDuration && !IsPlaying;
        }

        private void Pause(object o)
        {
            if (CanPause(o))
            {
                _state.MediaPlayer.Pause();
                IsPlaying = false;
            }
        }

        private bool CanPause(object o)
        {
            return HasDuration && IsPlaying;
        }

        private void Stop(object o)
        {
            if (CanStop(o))
            {
                _state.MediaPlayer.Stop();
                _state.MediaPlayer.MediaPosition = 0;
                IsPlaying = false;
            }
        }

        private bool CanStop(object o)
        {
            return _state.MediaPlayer.MediaPosition > 0;
        }

        private void Seek(object o)
        {
            if (CanSeek(o))
            {
                double percentComplete = (double)o;
                _state.MediaPlayer.MediaPosition = (long)(_state.MediaPlayer.MediaDuration * percentComplete);
            }
        }

        private bool CanSeek(object o)
        {
            return _state.MediaPlayer.MediaDuration > 0;
        }

        private void SetEffect(object o)
        {
            _state.MediaPlayer.Effect = (Effect)o;
        }

        private void Loop(object o)
        { 
            bool loop = (bool)o;
            _state.MediaPlayer.Loop = loop;
        }

        #endregion

    }
}
