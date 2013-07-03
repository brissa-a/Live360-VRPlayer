using System;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
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
                if (_effectPlugin != null)
                    _effectPlugin.Unload();
                _effectPlugin = value;
                _effectPlugin.Load();
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
                if (_projectionPlugin != null)
                    _projectionPlugin.Unload();
                _projectionPlugin = value;
                _projectionPlugin.Load();
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
                if (_trackerPlugin != null)
                    _trackerPlugin.Unload();
                _trackerPlugin = value;
                if(_trackerPlugin != null)
                    _trackerPlugin.Load();
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
                if (_distortionPlugin != null)
                    _distortionPlugin.Unload();
                _distortionPlugin = value;
                _distortionPlugin.Load();
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
                if (_stabilizerPlugin != null)
                    _stabilizerPlugin.Unload();
                _stabilizerPlugin = value;
                _stabilizerPlugin.Load();
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

            LoadDefaultMedia(config.SamplesFolder);

            //Todo: Use binding instead of a timer
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        //Todo: Create media handler checking best possible open action
        private void LoadDefaultMedia(string defaultMediaFolder)
        {
            var parameters = Environment.GetCommandLineArgs();

            if (parameters.Length > 1)
            {
                Logger.Instance.Info(string.Format("Loading '{0}'...", parameters[1]));
                if (!MediaPlugin.Content.OpenStreamCommand.CanExecute(null)) return;
                var uri = new Uri(parameters[1]);
                var uriWithoutScheme = uri.Host + uri.PathAndQuery;
                MediaPlugin.Content.OpenStreamCommand.Execute(new Uri(uriWithoutScheme, UriKind.RelativeOrAbsolute));
            }
            else
            {
                if (!MediaPlugin.Content.OpenFileCommand.CanExecute(null)) return;
                var samples = new DirectoryInfo(defaultMediaFolder);
                if (samples.GetFiles().Any())
                {
                    MediaPlugin.Content.OpenFileCommand.Execute(samples.GetFiles().First().FullName);
                }
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (MediaPlugin == null || MediaPlugin.Content == null)
                return;
            
            if (StabilizerPlugin != null && StabilizerPlugin.Content != null && 
                StabilizerPlugin.Content.GetFramesCount() > 0)
            {
                var frame = (int) Math.Round(StabilizerPlugin.Content.GetFramesCount()*MediaPlugin.Content.Progress/100);
                StabilizerPlugin.Content.UpdateCurrentFrame(frame);
            }

            if (TrackerPlugin != null && TrackerPlugin.Content != null)
            {
                MediaPlugin.Content.AudioPosition = TrackerPlugin.Content.Position;// +StabilizerPlugin.Content.Translation;
                MediaPlugin.Content.AudioRotation = TrackerPlugin.Content.Rotation;// *StabilizerPlugin.Content.Rotation;
            }
        }
    }
}
