using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;

namespace VrPlayer.Models.Presets
{
    public class PresetsManager : IPresetsManager
    {
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationConfig _config;

        public PresetsManager(IApplicationConfig config, IApplicationState state, IPluginManager pluginManager)
        {
            _config = config;
            _state = state;
            _pluginManager = pluginManager;
        }

        public void SaveMediaToFile(string fileName)
        {
            var o = new JObject();
            o.Add("Plugins", SaveMediaPluginsSettings());
            o.Add("Media", SaveMediaSettings());
            File.WriteAllText(fileName, o.ToString());
        }


    //    public void Reset()
    //    {
    //        LoadFromFile("Default.vrpl");
    //    }

    //    #region Saving

    //    public void SaveAllToSettings()
    //    {
    //        var o = new JObject();
    //        o.Add("Plugins", SaveAllPluginsSettings());
    //        o.Add("Media", SaveMediaSettings());
    //        o.Add("Device", SaveDeviceSettings());
    //        o.Add("Config", SaveConfigSettings());
    //        _settings["Config"] = o.ToString();
    //        _settings.Save();
    //    }
        
    //    public void SaveDeviceToFile(string fileName)
    //    {
    //        var o = new JObject();
    //        o.Add("Plugins", SaveDevicePluginsSettings());
    //        o.Add("Device", SaveDeviceSettings());
    //        File.WriteAllText(fileName, o.ToString());
    //    }

    //    public void SaveConfigToFile(string fileName)
    //    {
    //        var o = new JObject();
    //        o.Add("Config", SaveConfigSettings());
    //        File.WriteAllText(fileName, o.ToString());
    //    }

    //    public void SaveAllToFile(string fileName)
    //    {
    //        var o = new JObject();
    //        o.Add("Plugins", SaveAllPluginsSettings());
    //        o.Add("Media", SaveMediaSettings());
    //        o.Add("Device", SaveDeviceSettings());
    //        o.Add("Config", SaveConfigSettings());
    //        File.WriteAllText(fileName, o.ToString());
    //    }

    //    private JObject SaveAllPluginsSettings()
    //    {
    //        var o = new JObject();
    //        try
    //        {
    //            //Medias
    //            var mediasConfigs = new JObject();
    //            if (_pluginManager.Medias != null)
    //            {
    //                foreach (var plugin in _pluginManager.Medias.Where(plugin => plugin.Content != null))
    //                {
    //                    mediasConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (mediasConfigs.HasValues)
    //                o.Add("Medias", mediasConfigs);

    //            //Projections
    //            var projectionsConfigs = new JObject();
    //            if (_pluginManager.Projections != null)
    //            {
    //                foreach (var plugin in _pluginManager.Projections.Where(plugin => plugin.Content != null))
    //                {
    //                    projectionsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (projectionsConfigs.HasValues)
    //                o.Add("Projections", projectionsConfigs);

    //            //Effects
    //            var effectsConfigs = new JObject();
    //            if (_pluginManager.Effects != null)
    //            {
    //                foreach (var plugin in _pluginManager.Effects.Where(plugin => plugin.Content != null))
    //                {
    //                    effectsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (effectsConfigs.HasValues)
    //                o.Add("Effects", effectsConfigs);

    //            //Distortions
    //            var distortionsConfigs = new JObject();
    //            if (_pluginManager.Distortions != null)
    //            {
    //                foreach (var plugin in _pluginManager.Distortions.Where(plugin => plugin.Content != null))
    //                {
    //                    distortionsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (distortionsConfigs.HasValues)
    //                o.Add("Distortions", distortionsConfigs);

    //            //Trackers
    //            var trackersConfigs = new JObject();
    //            if (_pluginManager.Trackers != null)
    //            {
    //                foreach (var plugin in _pluginManager.Trackers.Where(plugin => plugin.Content != null))
    //                {
    //                    trackersConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (trackersConfigs.HasValues)
    //                o.Add("Trackers", trackersConfigs);

    //            //Stabilizers
    //            var stabilizersConfigs = new JObject();
    //            if (_pluginManager.Stabilizers != null)
    //            {
    //                foreach (var plugin in _pluginManager.Stabilizers.Where(plugin => plugin.Content != null))
    //                {
    //                    stabilizersConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (stabilizersConfigs.HasValues)
    //                o.Add("Stabilizers", stabilizersConfigs);
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while saving all plugins settings.", exc);
    //        }
    //        return o;
    //    }

        private JObject SaveMediaPluginsSettings()
        {
            var o = new JObject();
            try
            {
                //Projections
                var mediasConfigs = new JObject();
                if (_pluginManager.Medias != null)
                {
                    foreach (
                        var plugin in
                            _pluginManager.Medias.Where(plugin => plugin.Content != null)
                                          .Where(plugin => plugin == _state.MediaPlugin))
                    {
                        mediasConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
                    }
                }
                if (mediasConfigs.HasValues)
                    o.Add("Medias", mediasConfigs);

                //Projections
                var projectionsConfigs = new JObject();
                if (_pluginManager.Projections != null)
                {
                    foreach (
                        var plugin in
                            _pluginManager.Projections.Where(plugin => plugin.Content != null)
                                          .Where(plugin => plugin == _state.ProjectionPlugin))
                    {
                        projectionsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
                    }
                }
                if (projectionsConfigs.HasValues)
                    o.Add("Projections", projectionsConfigs);

                //Effects
                var effectsConfigs = new JObject();
                if (_pluginManager.Effects != null)
                {
                    foreach (
                        var plugin in
                            _pluginManager.Effects.Where(plugin => plugin.Content != null)
                                          .Where(plugin => plugin == _state.EffectPlugin))
                    {
                        effectsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
                    }
                }
                if (effectsConfigs.HasValues)
                    o.Add("Effects", effectsConfigs);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving media plugins settings.", exc);
            }
            return o;
        }

        private JObject SaveMediaSettings()
        {
            var o = new JObject();
            try
            {
                if (_state.MediaPlugin != null)
                    o.Add("CurrentMedias", new JValue(_state.MediaPlugin.GetType().FullName));
                if (_state.ProjectionPlugin != null)
                    o.Add("CurrentProjection", new JValue(_state.ProjectionPlugin.GetType().FullName));
                if (_state.EffectPlugin != null)
                    o.Add("CurrentEffect", new JValue(_state.EffectPlugin.GetType().FullName));
                if (_state.StabilizerPlugin != null)
                    o.Add("CurrentStabilizer", new JValue(_state.StabilizerPlugin.GetType().FullName));

                o.Add("StereoMode", new JValue(_state.StereoInput.ToString()));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving media settings.", exc);
            }
            return o;
        }

        //    private JObject SaveDevicePluginsSettings()
    //    {
    //        var o = new JObject();
    //        try
    //        {
    //            //Distortions
    //            var distortionsConfigs = new JObject();
    //            if (_pluginManager.Distortions != null)
    //            {
    //                foreach (var plugin in _pluginManager.Distortions
    //                    .Where(plugin => plugin.Content != null)
    //                    .Where(plugin => plugin == _state.DistortionPlugin))
    //                {
    //                    distortionsConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (distortionsConfigs.HasValues)
    //                o.Add("Distortions", distortionsConfigs);

    //            //Trackers
    //            var trackersConfigs = new JObject();
    //            if (_pluginManager.Trackers != null)
    //            {
    //                foreach (var plugin in _pluginManager.Trackers
    //                    .Where(plugin => plugin.Content != null)
    //                    .Where(plugin => plugin == _state.TrackerPlugin))
    //                {
    //                    trackersConfigs.Add(plugin.Content.GetType().FullName, JObject.FromObject(plugin.Content));
    //                }
    //            }
    //            if (trackersConfigs.HasValues)
    //                o.Add("Trackers", trackersConfigs);
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while saving device plugins settings.", exc);
    //        }
    //        return o;
    //    }

    //    private JObject SaveDeviceSettings()
    //    {
    //        var o = new JObject();
    //        try
    //        {
    //            if (_state.DistortionPlugin != null)
    //                o.Add("CurrentDistortion", new JValue(_state.DistortionPlugin.GetType().FullName));
    //            if (_state.TrackerPlugin != null)
    //                o.Add("CurrentTracker", new JValue(_state.TrackerPlugin.GetType().FullName));

    //            o.Add("FieldOfView", new JValue(_config.CameraFieldOfView.ToString(CultureInfo.InvariantCulture)));
    //            o.Add("LayoutMode", new JValue(_state.StereoOutput.ToString()));
    //            o.Add("HorizontalOffset", new JValue(_config.ViewportsHorizontalOffset.ToString(CultureInfo.InvariantCulture)));
    //            o.Add("VerticalOffset", new JValue(_config.ViewportsVerticalOffset.ToString(CultureInfo.InvariantCulture)));
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while saving device settings.", exc);
    //        }
    //        return o;
    //    }

    //    private JObject SaveConfigSettings()
    //    {
    //        var o = new JObject();
    //        try
    //        {
    //            o.Add("DefaultPlaylist", new JValue(_config.DefaultMedia));
    //            o.Add("DefaultPreset", new JValue(_config.DefaultPreset));
    //            o.Add("LoadSidecarFile", new JValue(_config.SidecarReadOnLoad.ToString()));
    //            o.Add("LoadMetadata", new JValue(_config.MetaDataReadOnLoad.ToString()));
    //            o.Add("LoadPlaylist", new JValue(_config.PlaylistReadOnLoad.ToString()));

    //            //Todo: Refactor!
    //            o.Add("Shortcuts", new JValue(
    //                _config.KeyPlayPause + "," +
    //                _config.KeyStop + "," +
    //                _config.KeyNext + "," +
    //                _config.KeyPrevious + "," +
    //                _config.KeyLoop + "," +
    //                _config.KeyMoveLeft + "," +
    //                _config.KeyMoveRight + "," +
    //                _config.KeyMoveForward + "," +
    //                _config.KeyMoveBackward + "," +
    //                _config.KeyMoveUp + "," +
    //                _config.KeyMoveDown + "," +
    //                _config.KeyTrackerCalibrate + "," +
    //                _config.KeyTrackerReset + "," +
    //                _config.KeyFieldOfViewMinus + "," +
    //                _config.KeyFieldOfViewPlus + "," +
    //                _config.KeyHorizontalOffsetMinus + "," +
    //                _config.KeyHorizontalOffsetPlus + "," +
    //                _config.KeyVerticalOffsetMinus + "," +
    //                _config.KeyVerticalOffsetPlus));
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while saving configs settings.", exc);
    //        }
    //        return o;
    //    }

    //    #endregion

    //    #region Loading

    //    public void LoadFromSettings()
    //    {
    //        try
    //        {
    //            if (string.IsNullOrEmpty(_settings["Config"].ToString()))
    //                return;
    //            Load(_settings["Config"].ToString());
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading user settings.", exc);
    //        }
    //    }

    //    public void LoadFromFile(string path)
    //    {
    //        if (!File.Exists(path))
    //            return;

    //        using (var reader = File.OpenText(path))
    //        {
    //            Load(reader.ReadToEnd());
    //        }
    //    }

    //    public void LoadFromMetadata(string path)
    //    {
    //        if (!File.Exists(path))
    //            return;

    //        var json = MetadataParser.Parse(path);
    //        Load(json);
    //    }

    //    public void Load(string json)
    //    {
    //        if (string.IsNullOrEmpty(json)) return;
    //        try
    //        {
    //            var o = JObject.Parse(json);
    //            LoadAllPluginsSettings((JObject)o["Plugins"]);
    //            LoadMediaSettings((JObject)o["Media"]);
    //            LoadDeviceSettings((JObject)o["Device"]);
    //            LoadConfigSettings((JObject)o["Config"]);
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading json data.", exc);
    //        }
    //    }

    //    private void LoadAllPluginsSettings(JObject o)
    //    {
    //        try
    //        {
    //            //Medias
    //            if (o["Medias"] != null)
    //            {
    //                foreach (var token in o["Medias"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var media = _pluginManager.Medias.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (media == null) continue;
    //                    Inject(media.Content, prop.Value);
    //                }
    //            }

    //            //Projections
    //            if (o["Projections"] != null)
    //            {
    //                foreach (var token in o["Projections"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var projection = _pluginManager.Projections.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (projection == null) continue;
    //                    Inject(projection.Content, prop.Value);
    //                }
    //            }

    //            //Effects
    //            if (o["Effects"] != null)
    //            {
    //                foreach (var token in o["Effects"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var effect = _pluginManager.Effects.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (effect == null) continue;
    //                    Inject(effect.Content, prop.Value);
    //                }
    //            }

    //            //Distortions
    //            if (o["Distortions"] != null)
    //            {
    //                foreach (var token in o["Distortions"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var distortion = _pluginManager.Distortions.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (distortion == null) continue;
    //                    Inject(distortion.Content, prop.Value);
    //                }
    //            }

    //            //Trackers
    //            if (o["Trackers"] != null)
    //            {
    //                foreach (var token in o["Trackers"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var tracker = _pluginManager.Trackers.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (tracker == null) continue;
    //                    Inject(tracker.Content, prop.Value);
    //                }
    //            }

    //            //Stabilizers
    //            if (o["Stabilizers"] != null)
    //            {
    //                foreach (var token in o["Stabilizers"].Children())
    //                {
    //                    if (!(token is JProperty)) continue;
    //                    var prop = token as JProperty;
    //                    var stabilizer = _pluginManager.Stabilizers.FirstOrDefault(plugin => plugin.Content != null && plugin.Content.GetType().FullName == prop.Name);
    //                    if (stabilizer == null) continue;
    //                    Inject(stabilizer.Content, prop.Value);
    //                }
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading plugins settings.", exc);
    //        }
    //    }

    //    private void LoadMediaSettings(JObject o)
    //    {
    //        try
    //        {
    //            if (o["StereoMode"] != null)
    //            {
    //                var stereoMode = o["StereoMode"].ToString();
    //                if (!string.IsNullOrEmpty(stereoMode))
    //                    _state.StereoInput = (StereoMode)Enum.Parse(typeof(StereoMode), stereoMode);
    //            }

    //            if (o["CurrentMedia"] != null)
    //            {
    //                var currentMedia = o["CurrentMedia"].ToString();
    //                if (!string.IsNullOrEmpty(currentMedia))
    //                    _state.MediaPlugin = _pluginManager.Medias.FirstOrDefault(plugin => plugin.GetType().FullName == currentMedia);
    //            }

    //            if (o["CurrentProjection"] != null)
    //            {
    //                var currentProjection = o["CurrentProjection"].ToString();
    //                if (!string.IsNullOrEmpty(currentProjection))
    //                    _state.ProjectionPlugin = _pluginManager.Projections.FirstOrDefault(plugin => plugin.GetType().FullName == currentProjection);
    //            }

    //            if (o["CurrentEffect"] != null)
    //            {
    //                var currentEffect = o["CurrentEffect"].ToString();
    //                if (!string.IsNullOrEmpty(currentEffect))
    //                    _state.EffectPlugin = _pluginManager.Effects.FirstOrDefault(plugin => plugin.GetType().FullName == currentEffect);
    //            }

    //            if (o["CurrentStabilizer"] != null)
    //            {
    //                var currentStabilizer = o["CurrentStabilizer"].ToString();
    //                if (!string.IsNullOrEmpty(currentStabilizer))
    //                    _state.StabilizerPlugin = _pluginManager.Stabilizers.FirstOrDefault(plugin => plugin.GetType().FullName == currentStabilizer);
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading media settings.", exc);
    //        }
    //    }

    //    private void LoadDeviceSettings(JObject o)
    //    {
    //        try
    //        {
    //            if (o["LayoutMode"] != null)
    //            {
    //                var layoutMode = o["LayoutMode"].ToString();
    //                if (!string.IsNullOrEmpty(layoutMode))
    //                    _state.StereoOutput = (LayoutMode)Enum.Parse(typeof(LayoutMode), layoutMode);
    //            }

    //            if (o["CurrentDistortion"] != null)
    //            {
    //                var currentDistortion = o["CurrentDistortion"].ToString();
    //                if (!string.IsNullOrEmpty(currentDistortion))
    //                    _state.DistortionPlugin = _pluginManager.Distortions.FirstOrDefault(plugin => plugin.GetType().FullName == currentDistortion);
    //            }

    //            if (o["CurrentTracker"] != null)
    //            {
    //                var currentTracker = o["CurrentTracker"].ToString();
    //                if (!string.IsNullOrEmpty(currentTracker))
    //                    _state.TrackerPlugin = _pluginManager.Trackers.FirstOrDefault(plugin => plugin.GetType().FullName == currentTracker);
    //            }

    //            if (o["FieldOfView"] != null)
    //            {
    //                var fieldOfView = o["FieldOfView"].ToString();
    //                if (!string.IsNullOrEmpty(fieldOfView))
    //                    _config.CameraFieldOfView = int.Parse(fieldOfView);
    //            }

    //            if (o["HorizontalOffset"] != null)
    //            {
    //                var horizontalOffset = o["HorizontalOffset"].ToString();
    //                if (!string.IsNullOrEmpty(horizontalOffset))
    //                    _config.ViewportsHorizontalOffset = int.Parse(horizontalOffset);
    //            }

    //            if (o["VerticalOffset"] != null)
    //            {
    //                var verticalOffset = o["VerticalOffset"].ToString();
    //                if (!string.IsNullOrEmpty(verticalOffset))
    //                    _config.ViewportsVerticalOffset = int.Parse(verticalOffset);
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading device settings.", exc);
    //        }
    //    }

    //    private void LoadConfigSettings(JObject o)
    //    {
    //        try
    //        {
    //            if (o["DefaultMedia"] != null)
    //            {
    //                var media = o["DefaultMedia"].ToString();
    //                if (!string.IsNullOrEmpty(media))
    //                    _config.DefaultMedia = media;
    //            }
    //            if (o["DefaultPreset"] != null)
    //            {
    //                var preset = o["DefaultPreset"].ToString();
    //                if (!string.IsNullOrEmpty(preset))
    //                    _config.DefaultPreset = preset;
    //            }
    //            if (o["LoadMetadata"] != null)
    //            {
    //                var metadata = o["LoadMetadata"].ToString();
    //                if (!string.IsNullOrEmpty(metadata))
    //                    _config.MetaDataReadOnLoad = bool.Parse(metadata);
    //            }
    //            if (o["LoadSidecarFile"] != null)
    //            {
    //                var sidecarFile = o["LoadSidecarFile"].ToString();
    //                if (!string.IsNullOrEmpty(sidecarFile))
    //                    _config.SidecarReadOnLoad = bool.Parse(sidecarFile);
    //            }
    //            if (o["LoadPlaylist"] != null)
    //            {
    //                var playlist = o["LoadPlaylist"].ToString();
    //                if (!string.IsNullOrEmpty(playlist))
    //                    _config.PlaylistReadOnLoad = bool.Parse(playlist);
    //            }

    //            //Todo: Refactor!
    //            if (o["Shortcuts"] != null)
    //            {
    //                var shortcuts = o["Shortcuts"].ToString();
    //                if (!string.IsNullOrEmpty(shortcuts))
    //                {
    //                    var i = 0;
    //                    var keys = shortcuts.Split(',');
    //                    _config.KeyPlayPause = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyStop = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyNext = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyPrevious = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyLoop = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveLeft = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveRight = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveForward = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveBackward = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveUp = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyMoveDown = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyTrackerCalibrate = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyTrackerReset = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyFieldOfViewMinus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyFieldOfViewPlus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyHorizontalOffsetMinus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyHorizontalOffsetPlus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyVerticalOffsetMinus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                    _config.KeyVerticalOffsetPlus = (Key)Enum.Parse(typeof(Key), keys[i++]);
    //                }
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            Logger.Instance.Error("Error while loading configs settings.", exc);
    //        }
    //    }

    //    #endregion

    //    #region Helpers

    //    private static void Inject(object target, JToken properties)
    //    {
    //        if (target == null || properties == null) return;

    //        foreach (var token in properties.Children())
    //        {
    //            var property = token as JProperty;
    //            if (property == null) continue;

    //            var propertyInfo = target.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
    //            if (propertyInfo == null || !propertyInfo.CanWrite) continue;

    //            try
    //            {
    //                var value = property.ToObject(propertyInfo.PropertyType);
    //                propertyInfo.SetValue(target, value, null);
    //            }
    //            catch (Exception exc)
    //            {
    //            }
    //        }
    //    }

    //    #endregion

    }
}