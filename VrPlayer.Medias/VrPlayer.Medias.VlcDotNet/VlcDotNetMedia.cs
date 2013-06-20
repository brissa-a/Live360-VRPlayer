using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private readonly VlcControl _player;
        private readonly Image _media; 
        
        public override FrameworkElement Media
        {
            get { return _media; }
        }

        public VlcDotNetMedia()
        {
            _media = new Image();

            VlcContext.CloseAll();

            //Set libvlc.dll and libvlccore.dll directory path
            VlcContext.LibVlcDllsPath = @"C:\Program Files (x86)\VideoLAN\VLC";  //CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_X86;
            //Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = @"C:\Program Files (x86)\VideoLAN\VLC\plugins"; //CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_X86;

            //Set the startup options
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.LogOptions.LogInFile = true;
            VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = true;
            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            VlcContext.Initialize();

            //Media player
            _player = new VlcControl();
            _player.LengthChanged += PlayerOnLengthChanged;
            _player.PositionChanged += PlayerOnPositionChanged;

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

        private void PlayerOnLengthChanged(VlcControl sender, VlcEventArgs<long> vlcEventArgs)
        {
            Duration = _player.Duration;
        }

        private void PlayerOnPositionChanged(VlcControl sender, VlcEventArgs<float> vlcEventArgs)
        {
            _media.Source = _player.VideoSource;
            Position = _player.Time;
            OnPropertyChanged("Progress");
        }

        #region Commands

        private void OpenFile(object o)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (!openFileDialog.ShowDialog().Value) return;
            _player.Media = new PathMedia(openFileDialog.FileName);
            _player.Play();
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

        //Todo: Dispose with VlcContext.CloseAll();
    }
}
