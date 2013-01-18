using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Effects;

using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Wrappers;
using VrPlayer.Models.Shaders;
using VrPlayer.Models.Media;

namespace VrPlayer.Models.State
{
    public class DefaultApplicationState : ViewModelBase, IApplicationState
    {
        #region Fields

        private GraphPlayerElement _mediaPlayer;
        public GraphPlayerElement MediaPlayer
        {
            get
            {
                return _mediaPlayer;
            }
        }

        private IAudioEngine _audioEngine;
        public IAudioEngine AudioEngine
        {
            get
            {
                return _audioEngine;
            }
        }

        private StereoMode _stereoInput;
        public StereoMode StereoInput
        {
            get
            {
                return _stereoInput;
            }
            set
            {
                _stereoInput = value;
                OnPropertyChanged("StereoInput");
            }
        }

        private StereoMode _stereoOutput;
        public StereoMode StereoOutput
        {
            get
            {
                return _stereoOutput;
            }
            set
            {
                _stereoOutput = value;
                OnPropertyChanged("StereoOutput");
            }
        }

        private EffectPlugin _effectPlugin;
        public EffectPlugin EffectPlugin
        {
            get
            {
                return _effectPlugin;
            }
            set
            {
                _effectPlugin = value;
                OnPropertyChanged("EffectPlugin");
            }
        }

        private WrapperPlugin _wrapperPlugin;
        public WrapperPlugin WrapperPlugin
        {
            get
            {
                return _wrapperPlugin;
            }
            set
            {
                _wrapperPlugin = value;
                OnPropertyChanged("WrapperPlugin");
            }
        }

        private TrackerPlugin _trackerPlugin;
        public TrackerPlugin TrackerPlugin
        {
            get
            {
                return _trackerPlugin;
            }
            set
            {
                _trackerPlugin = value;
                OnPropertyChanged("TrackerPlugin");
            }
        }

        private ShaderPlugin _shaderPlugin;
        public ShaderPlugin ShaderPlugin
        {
            get
            {
                return _shaderPlugin;
            }
            set
            {
                _shaderPlugin = value;
                OnPropertyChanged("ShaderPlugin");
            }
        }

        #endregion

        public DefaultApplicationState(IApplicationConfig config, GraphPlayerElement mediaPlayer, IAudioEngine audioEngine)
        {
            _mediaPlayer = mediaPlayer;
            mediaPlayer.BeginInit();
            mediaPlayer.EndInit();

            _audioEngine = audioEngine;
        }
    }
}
