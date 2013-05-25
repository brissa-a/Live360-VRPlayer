using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Models.Plugins
{
    public class DynamicPluginManager : IPluginManager
    {
        [ImportMany]
        private IEnumerable<IPlugin<EffectBase>> _effects;
        public IEnumerable<IPlugin<EffectBase>> Effects
        {
            get
            {
                return _effects.Where(effect => effect.Content == null)
                    .Concat(_effects.Where(effect => effect.Content != null));
            }
        }
        
        [ImportMany]
        private IEnumerable<IPlugin<DistortionBase>> _distortions;
        public IEnumerable<IPlugin<DistortionBase>> Distortions
        {
            get
            {
                return _distortions.Where(distortion => distortion.Content == null)
                    .Concat(_distortions.Where(distortion => distortion.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<IProjection>> _projections;
        public IEnumerable<IPlugin<IProjection>> Projections
        {
            get
            {
                return _projections.Where(projection => projection.Content == null)
                    .Concat(_projections.Where(projection => projection.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<ITracker>> _trackers;
        public IEnumerable<IPlugin<ITracker>> Trackers
        {
            get
            {
                return _trackers.Where(trackers => trackers.Content == null)
                    .Concat(_trackers.Where(trackers => trackers.Content != null));
            }
        }

        public DynamicPluginManager()
        {
            var catalog = new AggregateCatalog();
            var path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

            string[] pluginFolders = { "Effects", "Distortions", "Trackers", "Projections" };
            foreach (var folder in pluginFolders.SelectMany(pluginFolder => Directory.GetDirectories(Path.Combine(path, pluginFolder))).Where(Directory.Exists))
            {
                catalog.Catalogs.Add(new DirectoryCatalog(folder));
            }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void Dispose()
        {
        }
    }
}