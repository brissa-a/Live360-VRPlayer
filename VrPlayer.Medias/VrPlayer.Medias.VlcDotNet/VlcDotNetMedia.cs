using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            VlcContext.StartupOptions.AddOption("--no-video-title");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
            VlcContext.StartupOptions.AddOption("--ffmpeg-fast");

            //Set debug options
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
            OpenDeviceCommand = new RelayCommand(o => { }, o => false);
            OpenProcessCommand = new RelayCommand(o => { }, o => false); 
            PlayCommand = new RelayCommand(Play, CanPlay);
            PauseCommand = new RelayCommand(Pause, CanPause);
            StopCommand = new RelayCommand(Stop, CanStop);
            PreviousCommand = new RelayCommand(PreviousChapter);
            NextCommand = new RelayCommand(NextChapter);
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
            var path = o.ToString();
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                _player.Media = new PathMedia(o.ToString());
                _player.Play();
                IsPlaying = true;
                HasChapters = false;
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load file '{0}'.", path);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnPropertyChanged("Media");
        }

        private void OpenDisc(object o)
        {
            if (o == null) return;
            var drive = (DriveInfo)o;
            //Todo: detect disc type (cd, dvd, bluray...) See: http://stackoverflow.com/questions/11420365/detecting-if-disc-is-in-dvd-drive
            try
            {
                _player.Media = new LocationMedia(string.Format("dvd:///{0}", drive.Name.Replace("\\","/")));
                _player.Play();
                IsPlaying = true;
                HasChapters = true;
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
            var url = o.ToString();
            if (string.IsNullOrEmpty(url)) return;
            try
            {
                _player.Media = new LocationMedia(url);
                _player.Play();
                IsPlaying = true;
                HasChapters = false;
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load stream at '{0}'.", url);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnPropertyChanged("Media");
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

        private void PreviousChapter(object o)
        {
            _player.Previous();
            Position = TimeSpan.Zero;
        }

        private void NextChapter(object o)
        {
            _player.Next();
            Position = TimeSpan.Zero;
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
