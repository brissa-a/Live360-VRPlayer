using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Trackers;
using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Media;

namespace VrPlayer.Models.State
{
    public class DefaultApplicationState : ViewModelBase, IApplicationState
    {
        #region Fields

        private readonly MediaUriElement _mediaPlayer;
        public MediaUriElement MediaPlayer
        {
            get
            {
                return _mediaPlayer;
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

        private ProjectionPlugin _projectionPlugin;
        public ProjectionPlugin ProjectionPlugin
        {
            get
            {
                return _projectionPlugin;
            }
            set
            {
                _projectionPlugin = value;
                OnPropertyChanged("ProjectionPlugin");
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

        private DistortionPlugin _distortionPlugin;
        public DistortionPlugin DistortionPlugin
        {
            get
            {
                return _distortionPlugin;
            }
            set
            {
                _distortionPlugin = value;
                OnPropertyChanged("DistortionPlugin");
            }
        }

        #endregion

        public DefaultApplicationState(IApplicationConfig config)
        {
            if (config.PositionalAudio)
            {
                _mediaPlayer = new MediaGraphElement();
            }
            else
            {
                _mediaPlayer = new MediaUriElement();
            }

            if (config.EvrRendering)
            {
                _mediaPlayer.VideoRenderer = WPFMediaKit.DirectShow.MediaPlayers.VideoRendererType.EnhancedVideoRenderer;
            }

            _mediaPlayer.BeginInit();
            _mediaPlayer.EndInit();
        }
    }
}
