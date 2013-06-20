using System;
using System.Windows;
using System.Windows.Input;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Contracts.Medias
{
    public abstract class MediaBase: ViewModelBase, IMedia
    {
        public abstract FrameworkElement Media { get; }

        public ICommand OpenFileCommand { get; protected set; }
        public ICommand OpenDiscCommand { get; protected set; }
        public ICommand OpenStreamCommand { get; protected set; }
        public ICommand OpenDeviceCommand { get; protected set; }
        public ICommand PlayCommand { get; protected set; }
        public ICommand PauseCommand { get; protected set; }
        public ICommand StopCommand { get; protected set; }
        public ICommand SeekCommand { get; protected set; }
        public ICommand LoopCommand { get; protected set; }
        
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

        private TimeSpan _position;
        public TimeSpan Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public bool HasDuration
        {
            get
            {
                return Duration.TotalMilliseconds > 0;
            }
        }

        public double Progress
        {
            get
            {
                return (Position.TotalMilliseconds / Duration.TotalMilliseconds) * 100;
            }
        }
    }
}
