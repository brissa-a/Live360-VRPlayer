using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Trackers;
using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Contracts.Projections;
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

        private LayoutMode _stereoOutput;
        public LayoutMode StereoOutput
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

        private IPlugin<EffectBase> _effectPlugin;
        public IPlugin<EffectBase> EffectPlugin
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

        private IPlugin<IProjection> _projectionPlugin;
        public IPlugin<IProjection> ProjectionPlugin
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

        private IPlugin<ITracker> _trackerPlugin;
        public IPlugin<ITracker> TrackerPlugin
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

        private IPlugin<DistortionBase> _distortionPlugin;
        public IPlugin<DistortionBase> DistortionPlugin
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
