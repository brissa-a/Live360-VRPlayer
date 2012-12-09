using System;
using System.Collections.Generic;
using System.Windows.Media.Effects;

using VrPlayer.Models.Config;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Shaders;
using VrPlayer.Models.Wrappers;
using VrPlayer.Models.Effects;
using System.Windows.Media;

namespace VrPlayer.Models.Plugins
{
    public class StaticPluginManager: IPluginManager
    {
        private readonly IApplicationConfig _config;

        private readonly List<EffectPlugin> _effects = new List<EffectPlugin>();
        private readonly List<WrapperPlugin> _wrappers = new List<WrapperPlugin>();
        private readonly List<TrackerPlugin> _trackers = new List<TrackerPlugin>();
        private readonly List<ShaderPlugin> _shaders = new List<ShaderPlugin>();

        public List<EffectPlugin> Effects
        {
            get { return _effects; }
        }

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

            LoadEffects();
            LoadWrappers();
            LoadTrackers();
            LoadShaders();
        }

        private void LoadEffects()
        {
            var nullEffectPlugin = new EffectPlugin(null, "None");
            _effects.Add(nullEffectPlugin);

            var depthMapSbsEffect = new DepthMapSbsEffect();
            depthMapSbsEffect.MaxOffset = _config.DepthMapMaxOffset;
            var depthMapSbsEffectPlugin = new EffectPlugin(depthMapSbsEffect, "Depth Map SBS");
            _effects.Add(depthMapSbsEffectPlugin);

            var colorKeyAlphaEffect = new ColorKeyAlphaEffect();
            colorKeyAlphaEffect.ColorKey = (Color)ColorConverter.ConvertFromString(_config.ColorKeyAlphaColor);
            colorKeyAlphaEffect.Tolerance = _config.ColorKeyTolerance;
            var colorKeyAlphaEffectPlugin = new EffectPlugin(colorKeyAlphaEffect, "Color Key Alpha");
            _effects.Add(colorKeyAlphaEffectPlugin);
        }

        private void LoadWrappers()
        {
            var planeWrapper = new PlaneWrapper();
            var planeWrapperPlugin = new WrapperPlugin(planeWrapper, "Virtual Movie Screen 1:1");
            _wrappers.Add(planeWrapperPlugin);

            var sphereWrapper = new SphereWrapper();
            var sphereWrapperPlugin = new WrapperPlugin(sphereWrapper, "Spherical 360x180");
            _wrappers.Add(sphereWrapperPlugin);

            var domeWrapper = new DomeWrapper();
            var domeWrapperPlugin = new WrapperPlugin(domeWrapper, "Dome 180x180");
            _wrappers.Add(domeWrapperPlugin);

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
            var nullShaderPlugin = new ShaderPlugin(null, "None");
            _shaders.Add(nullShaderPlugin);

            var barrelEffect = new BarrelEffect();
            var barrelEffectPlugin = new ShaderPlugin(barrelEffect, "Barrel Distortion");
            _shaders.Add(barrelEffectPlugin);

            var customPincushionEffect = new PincushionEffect();
            customPincushionEffect.Factor = _config.CustomPincushionFactor;
            var customPincushionEffectPlugin = new ShaderPlugin(customPincushionEffect, "Pincushion Distortion");
            _shaders.Add(customPincushionEffectPlugin);
        }
    }
}
