using System;
using System.IO;
using System.Linq;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Stabilization;
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

        private Deshaker _deshaker;
        public Deshaker Deshaker
        {
            get
            {
                return _deshaker;
            }
            set
            {
                _deshaker = value;
                OnPropertyChanged("Deshaker");
            }
        }

        #endregion

        public DefaultApplicationState(IApplicationConfig config, IPluginManager pluginManager)
        {
            //Set plugins
            _effectPlugin = pluginManager.Effects
                .Where(e => e.GetType().FullName.Contains(config.DefaultEffect))
                .DefaultIfEmpty(pluginManager.Effects.FirstOrDefault())
                .First();
            
            _distortionPlugin = pluginManager.Distortions
                .Where(d => d.GetType().FullName.Contains(config.DefaultDistortion))
                .DefaultIfEmpty(pluginManager.Distortions.FirstOrDefault())
                .First();
            
            _projectionPlugin = pluginManager.Projections
                .Where(p => p.GetType().FullName.Contains(config.DefaultProjection))
                .DefaultIfEmpty(pluginManager.Projections.FirstOrDefault())
                .First();
            
            _trackerPlugin = pluginManager.Trackers
                .Where(t => t.GetType().FullName.Contains(config.DefaultTracker))
                .DefaultIfEmpty(pluginManager.Trackers.FirstOrDefault())
                .First();

            //Set deshaker
            _deshaker = new Deshaker();

            //Set media player
            _mediaPlayer = config.PositionalAudio ? new MediaGraphElement() : new MediaUriElement();

            if (config.EvrRendering)
            {
                _mediaPlayer.VideoRenderer = WPFMediaKit.DirectShow.MediaPlayers.VideoRendererType.EnhancedVideoRenderer;
            }

            _mediaPlayer.BeginInit();
            _mediaPlayer.EndInit();

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

            _mediaPlayer.Play();
        }
    }
}
