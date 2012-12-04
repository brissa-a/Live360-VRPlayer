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
            var barrelEffect = new BarrelEffect();
            var barrelEffectPlugin = new ShaderPlugin(barrelEffect, "Barrel Distortion");
            _shaders.Add(barrelEffectPlugin);

            var lightPincushionEffect = new PincushionEffect();
            lightPincushionEffect.Factor = 10;
            var lightPincushionEffectPlugin = new ShaderPlugin(lightPincushionEffect, "Light Pincushion Distortion");
            _shaders.Add(lightPincushionEffectPlugin);

            var mediumPincushionEffect = new PincushionEffect();
            mediumPincushionEffect.Factor = 3;
            var mediumPincushionEffectPlugin = new ShaderPlugin(mediumPincushionEffect, "Medium Pincushion Distortion");
            _shaders.Add(mediumPincushionEffectPlugin);

            var heavyPincushionEffect = new PincushionEffect();
            heavyPincushionEffect.Factor = 1;
            var heavyPincushionEffectPlugin = new ShaderPlugin(heavyPincushionEffect, "Heavy Pincushion Distortion");
            _shaders.Add(heavyPincushionEffectPlugin);

            var customPincushionEffect = new PincushionEffect();
            customPincushionEffect.Factor = _config.CustomPincushionFactor;
            var customPincushionEffectPlugin = new ShaderPlugin(customPincushionEffect, "Custom Pincushion Distortion");
            _shaders.Add(customPincushionEffectPlugin);
        }
    }
}
