using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Models.Config;

namespace VrPlayer.Models.Plugins
{
    public class DynamicPluginManager : IPluginManager
    {
        private readonly IApplicationConfig _config;

        [ImportMany(typeof(EffectBase))]
        private IEnumerable<EffectBase> _effects;
        public List<EffectPlugin> Effects
        {
            get
            {
                return _effects.Select(effect =>
                    new EffectPlugin(effect, effect.GetType().FullName)).ToList();
            }
        }

        [ImportMany(typeof(DistortionBase))]
        private IEnumerable<DistortionBase> _shaders;
        public List<ShaderPlugin> Shaders
        {
            get
            {
                return _shaders.Select(shader =>
                    new ShaderPlugin(shader, shader.GetType().FullName)).ToList();
            }
        }

        [ImportMany(typeof(IProjection))]
        private IEnumerable<IProjection> _projections;
        public List<ProjectionPlugin> Projections
        {
            get
            {
                return _projections.Select(projection =>
                    new ProjectionPlugin(projection, projection.GetType().FullName)).ToList();
            }
        }

        [ImportMany(typeof(ITracker))]
        private IEnumerable<ITracker> _trackers;
        public List<TrackerPlugin> Trackers
        {
            get
            {
                return _trackers.Select(tracker =>
                    new TrackerPlugin(tracker, tracker.GetType().FullName)).ToList();
            }
        }

        public DynamicPluginManager(IApplicationConfig config)
        {
            Import();
        }

        public void Import()
        {
            var catalog = new AggregateCatalog();
            var path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

            string[] pluginFolders = {"Effects","Distortions","Trackers","Projections"};
            foreach (var folder in pluginFolders.SelectMany(pluginFolder => Directory.GetDirectories(Path.Combine(path, pluginFolder))))
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