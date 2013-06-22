using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.MediaPlayers;

namespace VrPlayer.Medias.WpfMediaKit
{
    public class WpfMediaKitMedia : MediaBase
    {
        private MediaElementBase _player;
        private DispatcherTimer _timer;

        public override FrameworkElement Media
        {
            get { return _player; }
        }

        public static readonly DependencyProperty PositionalAudioProperty =
            DependencyProperty.Register("PositionalAudio", typeof(bool),
            typeof(WpfMediaKitMedia), new FrameworkPropertyMetadata(false));
        public bool PositionalAudio
        {
            get { return (bool)GetValue(PositionalAudioProperty); }
            set { SetValue(PositionalAudioProperty, value); }
        }

        public static readonly DependencyProperty EvrRenderingProperty =
            DependencyProperty.Register("EvrRendering", typeof(bool),
            typeof(WpfMediaKitMedia), new FrameworkPropertyMetadata(false));
        public bool EvrRendering
        {
            get { return (bool)GetValue(EvrRenderingProperty); }
            set { SetValue(EvrRenderingProperty, value); }
        }

        public WpfMediaKitMedia()
        {
            //Commands
            OpenFileCommand = new RelayCommand(OpenFile);
            OpenDiscCommand = new RelayCommand(OpenDisc);
            OpenStreamCommand = new RelayCommand(OpenStream);
            OpenDeviceCommand = new RelayCommand(OpenDevice);
            PlayCommand = new RelayCommand(Play, CanPlay);
            PauseCommand = new RelayCommand(Pause, CanPause);
            StopCommand = new RelayCommand(Stop, CanStop);
            SeekCommand = new RelayCommand(Seek, CanSeek);
            LoopCommand = new RelayCommand(Loop);
        }

        public override void Load()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Tick += timer_Tick;
            _timer.Start();
            OnPropertyChanged("Media");
        }

        public override void Unload()
        {
            if (_timer != null)
                _timer.Stop();
            _timer = null;
            if (_player != null)
                _player.Stop();
            _player = null;
            OnPropertyChanged("Media");
        }

        private MediaUriElement CreateMediaUriElement()
        {
            var player = PositionalAudio ? new MediaGraphElement() : new MediaUriElement();
            player.BeginInit();
            if (EvrRendering)
                player.VideoRenderer = VideoRendererType.EnhancedVideoRenderer;
            player.MediaOpened += PlayerOnMediaOpened;
            player.EndInit();
            return player;
        }

        private void PlayerOnMediaOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_player is MediaSeekingElement)
                Duration = TimeSpan.FromTicks(((MediaSeekingElement)_player).MediaDuration);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_player == null) return;
            if (_player is MediaSeekingElement)
                Position = TimeSpan.FromTicks(((MediaSeekingElement) _player).MediaPosition);
        }

        #region Commands

        private void OpenFile(object o)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (!openFileDialog.ShowDialog().Value) return;
            try
            {
                var player = CreateMediaUriElement();
                player.Source = new Uri(openFileDialog.FileName, UriKind.Absolute);
                player.Play();
                _player = player;
                OnPropertyChanged("Media");
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load '{0}'.", openFileDialog.FileName);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenDisc(object o)
        {
            var player = new DvdPlayerElement();
            player.BeginInit();
            player.PlayOnInsert = true;
            player.DvdDirectory = new Uri(@"d:\VIDEO_TS").AbsolutePath;
            player.EndInit();
            _player = player;
            OnPropertyChanged("Media");
        }

        private void OpenStream(object o)
        {
            var input = Interaction.InputBox("Enter the stream URL", "Open Stream", "http://", 0, 0);
            var player = CreateMediaUriElement();
            player.Source = new Uri(input, UriKind.Absolute);
            player.Play();
            _player = player;
            OnPropertyChanged("Media");
        }

        private void OpenDevice(object o)
        {
            var player = new VideoCaptureElement();
            player.BeginInit();
            player.Width = 320;
            player.DesiredPixelWidth = 320;
            player.Height = 240;
            player.DesiredPixelHeight = 240;
            player.VideoCaptureDevice = MultimediaUtil.VideoInputDevices[0];
            player.VideoCaptureSource = MultimediaUtil.VideoInputDevices[0].Name;
            player.FPS = 30;
            player.EndInit();
            player.Play();
            _player = player;
            OnPropertyChanged("Media");
        }

        private void Play(object o)
        {
            if (!CanPlay(o)) return;
            _player.Play();
            IsPlaying = true;
        }

        private bool CanPlay(object o)
        {
            return HasDuration && !IsPlaying;
        }

        private void Pause(object o)
        {
            if (!CanPause(o)) return;
            _player.Pause();
            IsPlaying = false;
        }

        private bool CanPause(object o)
        {
            return HasDuration && IsPlaying;
        }

        private void Stop(object o)
        {
            if (!CanStop(o)) return;
            _player.Stop();
            if (_player is MediaSeekingElement)
                ((MediaSeekingElement)_player).MediaPosition = 0;
            IsPlaying = false;
        }

        private bool CanStop(object o)
        {
            if (_player is MediaSeekingElement)
                return ((MediaSeekingElement)_player).MediaPosition > 0;
            return false;
        }

        private void Seek(object o)
        {
            if (!CanSeek(o)) return;
            if (!(_player is MediaSeekingElement)) return;
            var player = ((MediaSeekingElement) _player);
            player.MediaPosition = (long)(player.MediaDuration * Convert.ToDouble(o));
        }

        private bool CanSeek(object o)
        {
            if (_player is MediaSeekingElement)
                return ((MediaSeekingElement)_player).MediaDuration > 0;
            return false;
        }

        private void Loop(object o)
        {
            if(_player is MediaUriElement)
                ((MediaUriElement)_player).Loop = (bool)o;
        }

        #endregion
    }
}
