using System;
using System.Collections.Generic;
using System.Windows.Media.Effects;

using VrPlayer.Models.Config;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Shaders;
using VrPlayer.Models.Wrappers;

namespace VrPlayer.Models.Plugins
{
    public class StaticPluginManager: IPluginManager
    {
        private readonly IApplicationConfig _config;

        private readonly List<WrapperPlugin> _wrappers = new List<WrapperPlugin>();
        private readonly List<TrackerPlugin> _trackers = new List<TrackerPlugin>();
        private readonly List<ShaderPlugin> _shaders = new List<ShaderPlugin>();

        public List<WrapperPlugin> Wrappers
        {
            get { return _wrappers; }
        }

        public List<TrackerPlugin> Trackers
        {
            get { return _trackers; }
        }

        public List<ShaderPlugin> Shaders
        {
            get { return _shaders; }
        }

        public StaticPluginManager(IApplicationConfig config)
        {
            _config = config;

            LoadWrappers();
            LoadTrackers();
            LoadShaders();
        }

        private void LoadWrappers()
        {
            var planeWrapper = new PlaneWrapper();
            var planeWrapperPlugin = new WrapperPlugin(planeWrapper, "Virtual Movie Screen 1:1");
            _wrappers.Add(planeWrapperPlugin);

            var sphereWrapper = new SphereWrapper();
            var sphereWrapperPlugin = new WrapperPlugin(sphereWrapper, "Spherical Movie 360x180");
            _wrappers.Add(sphereWrapperPlugin);

            var cubeWrapper = new CubeWrapper();
            var cubeWrapperPlugin = new WrapperPlugin(cubeWrapper, "Cave Simulator (6 sides)");
            _wrappers.Add(cubeWrapperPlugin);
        }

        private void LoadTrackers()
        {
            var mouseTracker = new MouseTracker();
            mouseTracker.MouseSensitivity = _config.MouseSensitivity;
            var mouseTrackerPlugin = new TrackerPlugin(mouseTracker, "Mouse");
            _trackers.Add(mouseTrackerPlugin);

            var psMoveTracker = new PsMoveTracker();
            var psMoveTrackerPlugin = new TrackerPlugin(psMoveTracker, "PlayStation Move");
            _trackers.Add(psMoveTrackerPlugin);

            var wiimoteTracker = new WiimoteTracker();
            var wiimoteTrackerPlugin = new TrackerPlugin(wiimoteTracker, "WiiMote + MotionPlus");
            _trackers.Add(wiimoteTrackerPlugin);
        }

        private void LoadShaders()
        {
            var lightBarrelWarpEffect = new BarrelWarpEffect();
            lightBarrelWarpEffect.BarrelFactor = 10;
            var lightBarrelWarpEffectPlugin = new ShaderPlugin(lightBarrelWarpEffect, "Light Barrel Warp");
            _shaders.Add(lightBarrelWarpEffectPlugin);

            var mediumBarrelWarpEffect = new BarrelWarpEffect();
            mediumBarrelWarpEffect.BarrelFactor = 3;
            var mediumBarrelWarpEffectPlugin = new ShaderPlugin(mediumBarrelWarpEffect, "Medium Barrel Warp");
            _shaders.Add(mediumBarrelWarpEffectPlugin);

            var heavyBarrelWarpEffect = new BarrelWarpEffect();
            heavyBarrelWarpEffect.BarrelFactor = 1;
            var heavyBarrelWarpEffectPlugin = new ShaderPlugin(heavyBarrelWarpEffect, "Heavy Barrel Warp");
            _shaders.Add(heavyBarrelWarpEffectPlugin);

            var customBarrelWarpEffect = new BarrelWarpEffect();
            customBarrelWarpEffect.BarrelFactor = _config.CustomBarrelWrapFactor;
            var customBarrelWarpEffectPlugin = new ShaderPlugin(customBarrelWarpEffect, "Custom Barrel Warp");
            _shaders.Add(customBarrelWarpEffectPlugin);
        }
    }
}
