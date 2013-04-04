using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
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
            BindProperty(_config, "DepthMapMaxOffset", depthMapOverUnderEffect, DepthMapOverUnderEffect.MaxOffsetProperty);
            var depthMapOverUnderEffectPlugin = new EffectPlugin(depthMapOverUnderEffect, "Depth Map Over/Under");
            Effects.Add(depthMapOverUnderEffectPlugin);

            var depthMapSbsEffect = new DepthMapSbsEffect();
            BindProperty(_config, "DepthMapMaxOffset", depthMapSbsEffect, DepthMapSbsEffect.MaxOffsetProperty);
            var depthMapSbsEffectPlugin = new EffectPlugin(depthMapSbsEffect, "Depth Map SBS");
            Effects.Add(depthMapSbsEffectPlugin);

            var colorKeyAlphaEffect = new ColorKeyAlphaEffect();
            colorKeyAlphaEffect.ColorKey = (Color)ColorConverter.ConvertFromString(_config.ColorKeyAlphaColor);
            BindProperty(_config, "ColorKeyTolerance", colorKeyAlphaEffect, ColorKeyAlphaEffect.ToleranceProperty);
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
            BindProperty(_config, "CylinderSlices", cylinderWrapper, CylinderWrapper.SlicesProperty);
            BindProperty(_config, "CylinderStacks", cylinderWrapper, CylinderWrapper.StacksProperty);
            var cylinderWrapperPlugin = new WrapperPlugin(cylinderWrapper, "Cylinder");
            Wrappers.Add(cylinderWrapperPlugin);

            var domeWrapper = new DomeWrapper();
            BindProperty(_config, "DomeSlices", domeWrapper, DomeWrapper.SlicesProperty);
            BindProperty(_config, "DomeStacks", domeWrapper, DomeWrapper.StacksProperty);
            var domeWrapperPlugin = new WrapperPlugin(domeWrapper, "Dome");
            Wrappers.Add(domeWrapperPlugin);

            var sphereWrapper = new SphereWrapper();
            BindProperty(_config, "SphereSlices", sphereWrapper, SphereWrapper.SlicesProperty);
            BindProperty(_config, "SphereStacks", sphereWrapper, SphereWrapper.StacksProperty);
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
            BindProperty(_config, "KinectPositionScaleFactor", kinectTracker, TrackerBase.PositionScaleFactorProperty);
            var kinectTrackerPlugin = new TrackerPlugin(kinectTracker, "Microsoft Kinect");
            Trackers.Add(kinectTrackerPlugin);

            var wiimoteTracker = new WiimoteTracker();
            var wiimoteTrackerPlugin = new TrackerPlugin(wiimoteTracker, "Nintendo WiiMote");
            Trackers.Add(wiimoteTrackerPlugin);

            var psMoveTracker = new PsMoveTracker();
            BindProperty(_config, "PsMovePositionScaleFactor", psMoveTracker, TrackerBase.PositionScaleFactorProperty);
            var psMoveTrackerPlugin = new TrackerPlugin(psMoveTracker, "PlayStation Move");
            Trackers.Add(psMoveTrackerPlugin);

            var hydraTracker = new RazerHydraTracker();
            BindProperty(_config, "HydraPositionScaleFactor", hydraTracker, TrackerBase.PositionScaleFactorProperty);
            hydraTracker.RotationOffset = QuaternionHelper.QuaternionFromEulerAngles(_config.HydraPitchOffset, 0, 0);
            var hydraTrackerPlugin = new TrackerPlugin(hydraTracker, "Razer Hydra");
            Trackers.Add(hydraTrackerPlugin);

            var vrpnTracker = new VrpnTracker(_config.VrpnTrackerAddress, _config.VrpnButtonAddress);
            BindProperty(_config, "VrpnPositionScaleFactor", vrpnTracker, TrackerBase.PositionScaleFactorProperty);
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
            BindProperty(_config, "CustomPincushionFactor", customPincushionEffect, PincushionEffect.BarrelFactorProperty);
            var customPincushionEffectPlugin = new ShaderPlugin(customPincushionEffect, "Pincushion Distortion");
            Shaders.Add(customPincushionEffectPlugin);
        }

        private void BindProperty(object source, string path, DependencyObject target, DependencyProperty property)
        {
            var binding = new Binding
            {
                Source = source,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, property, binding);        
        }
    }
}
