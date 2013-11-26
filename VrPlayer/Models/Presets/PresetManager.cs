using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
            o.Add("Media", GenerateMediaPresets());
            File.WriteAllText(fileName, o.ToString());
        }
   
        private JObject GenerateMediaPresets()
        {
            var o = new JObject();
            try
            {
                //Media
                /*
                if (_state.MediaPlugin != null)
                {
                    var mediasConfigs = new JObject();
                    if (_pluginManager.Medias != null)
                    {
                        foreach (var plugin in _pluginManager.Medias
                            .Where(p => p.Content != null)
                            .Where(p => p == _state.MediaPlugin))
                        {
                            mediasConfigs.Add("Type", plugin.Content.GetType().FullName);
                            mediasConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (mediasConfigs.HasValues)
                        o.Add("Media", mediasConfigs);
                }
                */
                //Projection
                if (_state.ProjectionPlugin != null)
                {
                    var projectionConfigs = new JObject();
                    if (_pluginManager.Projections != null)
                    {
                        foreach (var plugin in _pluginManager.Projections
                            .Where(p => p.Content != null)
                            .Where(p => p == _state.ProjectionPlugin))
                        {
                            projectionConfigs.Add("Type", plugin.Content.GetType().FullName);
                            projectionConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (projectionConfigs.HasValues)
                        o.Add("Projection", projectionConfigs);
                }
                //Effect
                if (_state.EffectPlugin != null)
                {
                    var effectConfigs = new JObject();
                    if (_pluginManager.Effects != null)
                    {
                        foreach (var plugin in _pluginManager.Effects
                            .Where(p => p.Content != null)
                            .Where(p => p == _state.EffectPlugin))
                        {
                            effectConfigs.Add("Type", plugin.Content.GetType().FullName);
                            effectConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (effectConfigs.HasValues)
                        o.Add("Effect", effectConfigs);
                }
                //Stabilizer
                if (_state.StabilizerPlugin != null)
                {
                    var stabilizerConfigs = new JObject();
                    if (_pluginManager.Stabilizers != null)
                    {
                        foreach (var plugin in _pluginManager.Stabilizers
                            .Where(p => p.Content != null)
                            .Where(p => p == _state.StabilizerPlugin))
                        {
                            stabilizerConfigs.Add("Type", plugin.Content.GetType().FullName);
                            stabilizerConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (stabilizerConfigs.HasValues)
                        o.Add("Stabilizer", stabilizerConfigs);
                }
                //Stereo mode
                o.Add("StereoMode", new JValue(_state.StereoInput.ToString()));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving media settings.", exc);
            }
            return o;
        }

        public void LoadFromUri(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                var uri = new Uri(path);

                if (uri.IsFile)
                {
                    if (string.IsNullOrEmpty(uri.LocalPath) || !File.Exists(uri.LocalPath))
                        return;

                    using (var reader = File.OpenText(uri.LocalPath))
                    {
                        Load(reader.ReadToEnd());
                    }
                }
                else
                {
                    var json = new WebClient().DownloadString(uri.AbsoluteUri);
                    Load(json);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading preset from URI", exc);
            }
        }

        public void Load(string json)
        {
            if (string.IsNullOrEmpty(json)) 
                return;

            try
            {
                var o = JObject.Parse(json);
                LoadMediaSettings((JObject)o["Media"]);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading json data.", exc);
            }
        }

        private void LoadMediaSettings(JObject o)
        {
            try
            {
                //Stereo mode
                if (o["StereoMode"] != null)
                {
                    var stereoMode = o["StereoMode"].ToString();
                    if (!string.IsNullOrEmpty(stereoMode))
                        _state.StereoInput = (StereoMode)Enum.Parse(typeof(StereoMode), stereoMode);
                }
                
                if (o["Media"] == null) return;
                
                //Medias
                /*
                var media = (JObject)o["Media"];
                if (media["Type"] != null)
                {
                    var plugin = _pluginManager.Medias.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == media["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.MediaPlugin = plugin;
                        var token = media["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                */
                //Projection
                var projection = (JObject)o["Projection"];
                if (projection["Type"] != null)
                {
                    var plugin = _pluginManager.Projections.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == projection["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.ProjectionPlugin = plugin;
                        var token = projection["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                //Effect
                var effect = (JObject)o["Effect"];
                if (effect["Type"] != null)
                {
                    var plugin = _pluginManager.Effects.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == effect["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.EffectPlugin = plugin;
                        var token = effect["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                //Stabilizer
                var stabilizer = (JObject)o["Stabilizer"];
                if (stabilizer["Type"] != null)
                {
                    var plugin = _pluginManager.Stabilizers.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == stabilizer["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.StabilizerPlugin = plugin;
                        var token = stabilizer["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading media settings.", exc);
            }
        }

        #region Helpers

        private static void Inject(object target, JToken properties)
        {
            if (target == null || properties == null) return;

            foreach (var token in properties.Children())
            {
                var property = token as JProperty;
                if (property == null) continue;

                var propertyInfo = target.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null || !propertyInfo.CanWrite) continue;

                try
                {
                    var value = property.ToObject(propertyInfo.PropertyType);
                    propertyInfo.SetValue(target, value, null);
                }
                catch (Exception exc)
                {
                }
            }
        }

        #endregion
    }
}