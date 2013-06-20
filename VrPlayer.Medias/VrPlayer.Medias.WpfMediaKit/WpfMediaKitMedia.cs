using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using WPFMediaKit.DirectShow.Controls;

namespace VrPlayer.Medias.WpfMediaKit
{
    public class WpfMediaKitMedia : MediaBase
    {
        private readonly MediaUriElement _player;

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
            _player = PositionalAudio ? new MediaGraphElement() : new MediaUriElement();

            if (EvrRendering)
                _player.VideoRenderer = WPFMediaKit.DirectShow.MediaPlayers.VideoRendererType.EnhancedVideoRenderer;
          
            _player.BeginInit();
            _player.EndInit();

            _player.Play();
            
            /*
            var parameters = Environment.GetCommandLineArgs();

            if (parameters.Length > 1)
            {
                Logger.Instance.Info(string.Format("Loading '{0}'...", parameters[1]));
                var uri = new Uri(parameters[1]);
                var uriWithoutScheme = uri.Host + uri.PathAndQuery;
                _mediaPlayer.Source = new Uri(uriWithoutScheme, UriKind.RelativeOrAbsolute);
            }
            else
            {
                var samples = new DirectoryInfo(config.SamplesFolder);
                if (samples.GetFiles().Any())
                {
                    _mediaPlayer.Source = new Uri(samples.GetFiles().First().FullName, UriKind.RelativeOrAbsolute);
                }
            }
            */

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

        #region Commands

        private void OpenFile(object o)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (!openFileDialog.ShowDialog().Value) return;
            try
            {
                _player.Source = new Uri(openFileDialog.FileName, UriKind.Absolute);
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
        }

        private void OpenStream(object o)
        {
        }

        private void OpenDevice(object o)
        {
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
            _player.MediaPosition = 0;
        }

        private bool CanStop(object o)
        {
            return _player.MediaPosition > 0;
        }

        private void Seek(object o)
        {
            if (!CanSeek(o)) return;
            _player.MediaPosition = (long)Convert.ToDouble(o);
        }

        private bool CanSeek(object o)
        {
            return _player.MediaDuration > 0;
        }

        private void Loop(object o)
        {
            _player.Loop = (bool)o;
        }

        #endregion

    }
}
