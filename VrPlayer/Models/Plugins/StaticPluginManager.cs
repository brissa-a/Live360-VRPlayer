using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Distortions.Barrel;
using VrPlayer.Distortions.Pincushion;
using VrPlayer.Helpers.Converters;
using VrPlayer.Models.Config;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Wrappers;
using VrPlayer.Models.Effects;
using VrPlayer.Trackers.KinectTracker;
using VrPlayer.Trackers.PsMoveTracker;
using VrPlayer.Trackers.RazerHydraTracker;
using VrPlayer.Trackers.VrpnTracker;
using VrPlayer.Trackers.WiimoteTracker;

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
            BindProperty(_config, "ColorKeyAlphaColor", colorKeyAlphaEffect, ColorKeyAlphaEffect.ColorKeyProperty);
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
            BindProperty(_config, "PlaneRatio", planeWrapper, PlaneWrapper.RatioProperty);
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
            BindProperty(_config, "MouseSensitivity", mouseTracker, MouseTracker.MouseSensitivityProperty);
            var mouseTrackerPlugin = new TrackerPlugin(mouseTracker, "Mouse");
            Trackers.Add(mouseTrackerPlugin);
            /*
            var oculusRiftTracker = new OculusRiftTracker();
            var oculusRiftTrackerPlugin = new TrackerPlugin(oculusRiftTracker, "Oculus Rift");
            Trackers.Add(oculusRiftTrackerPlugin);
            */
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
            BindProperty(_config, "HydraRotationOffset", hydraTracker, TrackerBase.RotationOffsetProperty, new Vector3DToQuaternionConverter());
            BindProperty(_config, "HydraPositionScaleFactor", hydraTracker, TrackerBase.PositionScaleFactorProperty);
            var hydraTrackerPlugin = new TrackerPlugin(hydraTracker, "Razer Hydra");
            Trackers.Add(hydraTrackerPlugin);

            var vrpnTracker = new VrpnTracker(_config.VrpnTrackerAddress, _config.VrpnButtonAddress);
            BindProperty(_config, "VrpnRotationOffset", vrpnTracker, TrackerBase.RotationOffsetProperty, new Vector3DToQuaternionConverter());
            BindProperty(_config, "VrpnPositionScaleFactor", vrpnTracker, TrackerBase.PositionScaleFactorProperty);
            var vrpnTrackerPlugin = new TrackerPlugin(vrpnTracker, "VRPN Client");
            Trackers.Add(vrpnTrackerPlugin);
            /*
            var leapTracker = new LeapTracker();
            BindProperty(_config, "LeapRotationFactor", leapTracker, LeapTracker.RotationFactorProperty);
            BindProperty(_config, "LeapPositionScaleFactor", leapTracker, TrackerBase.PositionScaleFactorProperty);
            var leapTrackerPlugin = new TrackerPlugin(leapTracker, "Leap");
            Trackers.Add(leapTrackerPlugin);
            */
        }

        private void LoadShaders()
        {
            var nullShaderPlugin = new ShaderPlugin(null, "None");
            Shaders.Add(nullShaderPlugin);

            var barrelEffect = new BarrelEffect();
            BindProperty(_config, "BarrelFactor", barrelEffect, BarrelEffect.FactorProperty);
            var barrelEffectPlugin = new ShaderPlugin(barrelEffect, "Barrel Distortion");
            Shaders.Add(barrelEffectPlugin);

            var pincushionEffect = new PincushionEffect();
            BindProperty(_config, "PincushionFactor", pincushionEffect, PincushionEffect.FactorProperty);
            var customPincushionEffectPlugin = new ShaderPlugin(pincushionEffect, "Pincushion Distortion");
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

        private void BindProperty(object source, string path, DependencyObject target, DependencyProperty property, IValueConverter converter)
        {
            var binding = new Binding
            {
                Source = source,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay,
                Converter = converter
            };
            BindingOperations.SetBinding(target, property, binding);
        }

        public void Dispose()
        {
            foreach (var trackerPlugin in Trackers.Where(trackerPlugin => trackerPlugin.Tracker != null))
            {
                trackerPlugin.Tracker.Dispose();
            }
        }
    }
}
