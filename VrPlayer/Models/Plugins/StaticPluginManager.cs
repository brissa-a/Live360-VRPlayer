using System.Collections.Generic;
using VrPlayer.Helpers;
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

        public List<EffectPlugin> Effects { get; private set; }
        public List<WrapperPlugin> Wrappers { get; private set; }
        public List<TrackerPlugin> Trackers { get; private set; }
        public List<ShaderPlugin> Shaders { get; private set; }

        public StaticPluginManager(IApplicationConfig config)
        {
            _config = config;

            Effects = new List<EffectPlugin>();
            Wrappers = new List<WrapperPlugin>();
            Shaders = new List<ShaderPlugin>();
            Trackers = new List<TrackerPlugin>();
            
            LoadEffects();
            LoadWrappers();
            LoadTrackers();
            LoadShaders();
        }

        private void LoadEffects()
        {
            var nullEffectPlugin = new EffectPlugin(null, "None");
            Effects.Add(nullEffectPlugin);

            var depthMapOverUnderEffect = new DepthMapOverUnderEffect();
            depthMapOverUnderEffect.MaxOffset = _config.DepthMapMaxOffset;
            var depthMapOverUnderEffectPlugin = new EffectPlugin(depthMapOverUnderEffect, "Depth Map Over/Under");
            Effects.Add(depthMapOverUnderEffectPlugin);

            var depthMapSbsEffect = new DepthMapSbsEffect();
            depthMapSbsEffect.MaxOffset = _config.DepthMapMaxOffset;
            var depthMapSbsEffectPlugin = new EffectPlugin(depthMapSbsEffect, "Depth Map SBS");
            Effects.Add(depthMapSbsEffectPlugin);

            var colorKeyAlphaEffect = new ColorKeyAlphaEffect();
            colorKeyAlphaEffect.ColorKey = (Color)ColorConverter.ConvertFromString(_config.ColorKeyAlphaColor);
            colorKeyAlphaEffect.Tolerance = _config.ColorKeyTolerance;
            var colorKeyAlphaEffectPlugin = new EffectPlugin(colorKeyAlphaEffect, "Color Key Alpha");
            Effects.Add(colorKeyAlphaEffectPlugin);

            var unwrapFishEyeEffect = new UnwrapFishEyeEffect();
            var unwrapFishEyeEffectPlugin = new EffectPlugin(unwrapFishEyeEffect, "Unwrap Fisheye");
            Effects.Add(unwrapFishEyeEffectPlugin);

            var unwrapFishEyeStereoEffect = new UnwrapFishEyeStereoEffect();
            var unwrapFishEyeStereoEffectPlugin = new EffectPlugin(unwrapFishEyeStereoEffect, "Unwrap Stereo Fisheye");
            Effects.Add(unwrapFishEyeStereoEffectPlugin);
        }

        private void LoadWrappers()
        {

            var planeWrapper = new PlaneWrapper();
            var planeWrapperPlugin = new WrapperPlugin(planeWrapper, "Plane");
            Wrappers.Add(planeWrapperPlugin);

            var cubeWrapper = new CubeWrapper();
            var cubeWrapperPlugin = new WrapperPlugin(cubeWrapper, "Cube");
            Wrappers.Add(cubeWrapperPlugin);

            var cylinderWrapper = new CylinderWrapper();
            var cylinderWrapperPlugin = new WrapperPlugin(cylinderWrapper, "Cylinder");
            Wrappers.Add(cylinderWrapperPlugin);

            var domeWrapper = new DomeWrapper();
            var domeWrapperPlugin = new WrapperPlugin(domeWrapper, "Dome");
            Wrappers.Add(domeWrapperPlugin);

            var sphereWrapper = new SphereWrapper();
            var sphereWrapperPlugin = new WrapperPlugin(sphereWrapper, "Sphere");
            Wrappers.Add(sphereWrapperPlugin);
        }

        private void LoadTrackers()
        {
            var mouseTracker = new MouseTracker();
            mouseTracker.MouseSensitivity = _config.MouseSensitivity;
            var mouseTrackerPlugin = new TrackerPlugin(mouseTracker, "Mouse");
            Trackers.Add(mouseTrackerPlugin);

            var kinectTracker = new KinectTracker();
            kinectTracker.PositionScaleFactor = _config.KinectPositionScaleFactor;
            var kinectTrackerPlugin = new TrackerPlugin(kinectTracker, "Microsoft Kinect");
            Trackers.Add(kinectTrackerPlugin);

            var wiimoteTracker = new WiimoteTracker();
            var wiimoteTrackerPlugin = new TrackerPlugin(wiimoteTracker, "Nintendo WiiMote");
            Trackers.Add(wiimoteTrackerPlugin);

            var psMoveTracker = new PsMoveTracker();
            psMoveTracker.PositionScaleFactor = _config.PsMovePositionScaleFactor;
            var psMoveTrackerPlugin = new TrackerPlugin(psMoveTracker, "PlayStation Move");
            Trackers.Add(psMoveTrackerPlugin);

            var hydraTracker = new RazerHydraTracker();
            hydraTracker.PositionScaleFactor = _config.HydraPositionScaleFactor;
            hydraTracker.RotationOffset = QuaternionHelper.QuaternionFromEulerAngles(_config.HydraPitchOffset,0,0);
            var hydraTrackerPlugin = new TrackerPlugin(hydraTracker, "Razer Hydra");
            Trackers.Add(hydraTrackerPlugin);

            var vrpnTracker = new VrpnTracker(_config.VrpnTrackerAddress, _config.VrpnButtonAddress);
            vrpnTracker.PositionScaleFactor = _config.VrpnPositionScaleFactor;
            var vrpnTrackerPlugin = new TrackerPlugin(vrpnTracker, "VRPN Client");
            Trackers.Add(vrpnTrackerPlugin);
        }

        private void LoadShaders()
        {
            var nullShaderPlugin = new ShaderPlugin(null, "None");
            Shaders.Add(nullShaderPlugin);

            var barrelEffect = new BarrelEffect();
            var barrelEffectPlugin = new ShaderPlugin(barrelEffect, "Barrel Distortion");
            Shaders.Add(barrelEffectPlugin);

            var customPincushionEffect = new PincushionEffect();
            customPincushionEffect.Factor = _config.CustomPincushionFactor;
            var customPincushionEffectPlugin = new ShaderPlugin(customPincushionEffect, "Pincushion Distortion");
            Shaders.Add(customPincushionEffectPlugin);
        }
    }
}
