using System.Linq;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Models.Plugins;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Contracts.Projections;

namespace VrPlayer.Models.State
{
    public class DefaultApplicationState : ViewModelBase, IApplicationState
    {
        #region Fields

        private IPlugin<IMedia> _mediaPlugin;
        public IPlugin<IMedia> MediaPlugin
        {
            get
            {
                return _mediaPlugin;
            }
            set
            {
                if (_mediaPlugin != null) 
                    _mediaPlugin.Unload();
                _mediaPlugin = value;
                _mediaPlugin.Load();
                OnPropertyChanged("MediaPlugin");
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

        private IPlugin<IStabilizer> _stabilizerPlugin;
        public IPlugin<IStabilizer> StabilizerPlugin
        {
            get
            {
                return _stabilizerPlugin;
            }
            set
            {
                _stabilizerPlugin = value;
                OnPropertyChanged("StabilizerPlugin");
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

        #endregion

        public DefaultApplicationState(IApplicationConfig config, IPluginManager pluginManager)
        {
            //Set plugins
            MediaPlugin = pluginManager.Medias
                .Where(m => m.GetType().FullName.Contains(config.DefaultMedia))
                .DefaultIfEmpty(pluginManager.Medias.FirstOrDefault())
                .First();

            EffectPlugin = pluginManager.Effects
                .Where(e => e.GetType().FullName.Contains(config.DefaultEffect))
                .DefaultIfEmpty(pluginManager.Effects.FirstOrDefault())
                .First();

            DistortionPlugin = pluginManager.Distortions
                .Where(d => d.GetType().FullName.Contains(config.DefaultDistortion))
                .DefaultIfEmpty(pluginManager.Distortions.FirstOrDefault())
                .First();
            
            ProjectionPlugin = pluginManager.Projections
                .Where(p => p.GetType().FullName.Contains(config.DefaultProjection))
                .DefaultIfEmpty(pluginManager.Projections.FirstOrDefault())
                .First();
            
            TrackerPlugin = pluginManager.Trackers
                .Where(t => t.GetType().FullName.Contains(config.DefaultTracker))
                .DefaultIfEmpty(pluginManager.Trackers.FirstOrDefault())
                .First();

            StabilizerPlugin = pluginManager.Stabilizers
                .Where(s => s.GetType().FullName.Contains(config.DefaultStabilizer))
                .DefaultIfEmpty(pluginManager.Stabilizers.FirstOrDefault())
                .First();
        }
    }
}
