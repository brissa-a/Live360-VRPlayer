using System;
using System.Configuration;
using System.Linq;
using VrPlayer.Helpers;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;

namespace VrPlayer.Models.Settings
{
    public class SettingsManager
    {
        private readonly ApplicationSettingsBase _settings;
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;

        public SettingsManager(ApplicationSettingsBase settings, IApplicationState state, IPluginManager pluginManager)
        {
            _settings = settings;
            _state = state;
            _pluginManager = pluginManager;
        }

        public void Save()
        {
            //Tracker
            if (_state.TrackerPlugin != null && _state.TrackerPlugin.Content != null)
                _settings["Tracker"] = _state.TrackerPlugin.Content.GetType().FullName;
            else
                _settings["Tracker"] = null;

            //Distortion
            if (_state.DistortionPlugin != null && _state.DistortionPlugin.Content != null)
                _settings["Distortion"] = _state.DistortionPlugin.Content.GetType().FullName;
            else
                _settings["Distortion"] = null;

            //Layout
            _settings["Layout"] = _state.StereoOutput.ToString();

            _settings.Save();
        }

        public void Load()
        {
            //Tracker
            _state.TrackerPlugin = _pluginManager.Trackers
                .Where(plugin => plugin.Content != null)
                .FirstOrDefault(plugin => plugin.Content.GetType().FullName == _settings["Tracker"].ToString());

            //Distortion
            _state.DistortionPlugin = _pluginManager.Distortions
                .Where(plugin => plugin.Content != null)
                .FirstOrDefault(plugin => plugin.Content.GetType().FullName == _settings["Distortion"].ToString());

            //Layout
            _state.StereoOutput = (LayoutMode)Enum.Parse(typeof(LayoutMode), _settings["Layout"].ToString());        
        }
    }
}