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
            InitVlcContext();
            _media = new Image(); 
            _player = new VlcControl();
            _player.LengthChanged += PlayerOnLengthChanged;
            _player.PositionChanged += PlayerOnPositionChanged;
            OnPropertyChanged("Media");
        }

        public override void Unload()
        {
            if (_player != null)
                _player.Stop();
            _player = null;
            _media = null;
            VlcContext.CloseAll();
            OnPropertyChanged("Media");
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "LibVlcDllsPath" ||
                propertyChangedEventArgs.PropertyName == "LibVlcPluginsPath" ||
                propertyChangedEventArgs.PropertyName == "DebugMode")
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
            _player.Media = new PathMedia(openFileDialog.FileName);
            _player.Play();
            OnPropertyChanged("Media");
        }

        private void OpenDisc(object o)
        {
            _player.Media = new LocationMedia("cdda:///D:");
            _player.Play();
            OnPropertyChanged("Media");
        }

        private void OpenStream(object o)
        {
            var input = Interaction.InputBox("Enter the stream URL", "Open Stream", "http://", 0, 0);
            _player.Media = new LocationMedia(input);
            _player.Play();
            OnPropertyChanged("Media");
        }

        private void OpenDevice(object o)
        {
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
            _player.Position = 0;
        }

        private bool CanStop(object o)
        {
            return _player.Position > 0;
        }

        private void Seek(object o)
        {
            if (!CanSeek(o)) return;
            _player.Position = (float)Convert.ToDouble(o);
        }

        private bool CanSeek(object o)
        {
            return _player.Duration.Ticks > 0;
        }

        private void Loop(object o)
        {
            var loop = (bool)o;
            _player.PlaybackMode = loop? PlaybackModes.Loop: PlaybackModes.Default;
        }

        #endregion
    }
}
