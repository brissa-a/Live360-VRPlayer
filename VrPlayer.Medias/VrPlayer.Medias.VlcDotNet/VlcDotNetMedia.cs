using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.MediaListPlayer;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using MediaBase = VrPlayer.Contracts.Medias.MediaBase;

namespace VrPlayer.Medias.VlcDotNet
{
    public class VlcDotNetMedia: MediaBase
    {
        private VlcControl _player;
        private Image _media; 
        
        public override FrameworkElement Media
        {
            get { return _media; }
        }

        public static readonly DependencyProperty DebugModeProperty =
            DependencyProperty.Register("DebugMode", typeof(bool),
            typeof(VlcDotNetMedia), new FrameworkPropertyMetadata(false));
        public bool DebugMode
        {
            get { return (bool)GetValue(DebugModeProperty); }
            set { SetValue(DebugModeProperty, value); }
        }

        public static readonly DependencyProperty LibVlcDllsPathProperty =
            DependencyProperty.Register("LibVlcDllsPath", typeof(string),
            typeof(VlcDotNetMedia), new FrameworkPropertyMetadata(""));
        public string LibVlcDllsPath
        {
            get { return (string)GetValue(LibVlcDllsPathProperty); }
            set { SetValue(LibVlcDllsPathProperty, value); }
        }

        public static readonly DependencyProperty LibVlcPluginsPathProperty =
            DependencyProperty.Register("LibVlcPluginsPath", typeof(string),
            typeof(VlcDotNetMedia), new FrameworkPropertyMetadata(""));
        public string LibVlcPluginsPath
        {
            get { return (string)GetValue(LibVlcPluginsPathProperty); }
            set { SetValue(LibVlcPluginsPathProperty, value); }
        }

        private void InitVlcContext()
        {
            VlcContext.CloseAll();

            //Set libvlc.dll and libvlccore.dll directory path
            VlcContext.LibVlcDllsPath = LibVlcDllsPath;
            //Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = LibVlcPluginsPath;

            //Set the startup options
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.LogOptions.LogInFile = DebugMode;
            VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = DebugMode;
            VlcContext.StartupOptions.LogOptions.Verbosity = DebugMode ? VlcLogVerbosities.Debug : VlcLogVerbosities.None;
            VlcContext.Initialize();
        }

        public VlcDotNetMedia()
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
            
            PropertyChanged += OnPropertyChanged;
        }

        public override void Load()
        {
            Reset();
            InitVlcContext();
            _media = new Image(); 
            _player = new VlcControl();
            _player.LengthChanged += PlayerOnLengthChanged;
            _player.PositionChanged += PlayerOnPositionChanged;
        }

        public override void Unload()
        {
            Reset();
            if (_player != null)
                _player.Stop();
            _player = null;
            _media = null;
            VlcContext.CloseAll();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "LibVlcDllsPath" ||
                propertyChangedEventArgs.PropertyName == "LibVlcPluginsPath")
                Load();
        }

        private void PlayerOnLengthChanged(VlcControl sender, VlcEventArgs<long> vlcEventArgs)
        {
            Duration = _player.Duration;
        }

        private void PlayerOnPositionChanged(VlcControl sender, VlcEventArgs<float> vlcEventArgs)
        {
            _media.Source = _player.VideoSource;
            Position = _player.Time;
        }

        #region Commands

        private void OpenFile(object o)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (!openFileDialog.ShowDialog().Value) return;
            try
            {
                _player.Media = new PathMedia(openFileDialog.FileName);
                _player.Play();
                IsPlaying = true;
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load file '{0}'.", openFileDialog.FileName);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnPropertyChanged("Media");
        }

        private void OpenDisc(object o)
        {
            try
            {
                _player.Media = new LocationMedia(string.Format("cdda:///{0}", o));
                _player.Play();
                IsPlaying = true;
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to read disc '{0}'.", o);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnPropertyChanged("Media");
        }

        private void OpenStream(object o)
        {
            var input = Interaction.InputBox(
            "Enter the stream URL" + Environment.NewLine + 
            Environment.NewLine +
            "Examples:" + Environment.NewLine +
            "http://www.example.com/stream.avi" + Environment.NewLine +
            "rtp://@:1234" + Environment.NewLine +
            "mms://mms.examples.com/stream.asx" + Environment.NewLine +
            "rtsp://server.example.org:8080/test.sdp" + Environment.NewLine +
            "http://www.youtube.com/watch?v=gg64x"+ Environment.NewLine, 
            "Open Stream", 
            "http://");
            if (string.IsNullOrEmpty(input))
                return;
            try
            {
                _player.Media = new LocationMedia(input);
                _player.Play();
                IsPlaying = true;
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load stream at '{0}'.", input);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnPropertyChanged("Media");
        }

        private void OpenDevice(object o)
        {
            throw new NotImplementedException();
        }

        private void Play(object o)
        {
            _player.Play();
            IsPlaying = true;
        }

        private void Pause(object o)
        {
            _player.Pause();
            IsPlaying = false;
        }

        private void Stop(object o)
        {
            _player.Position = 0;
            Position = TimeSpan.Zero;
            _player.Stop();
            IsPlaying = false;
        }

        private void Seek(object o)
        {
            _player.Position = (float)Convert.ToDouble(o);
        }

        private void Loop(object o)
        {
            var loop = (bool)o;
            _player.PlaybackMode = loop? PlaybackModes.Loop: PlaybackModes.Default;
        }

        #endregion
    }
}
