using System;
using System.Configuration;
using System.Linq;

using VrPlayer.Contracts.Projections;
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
            if (_state.TrackerPlugin != null)
                _settings["Tracker"] = _state.TrackerPlugin.Tracker.GetType().FullName;

            if (_state.DistortionPlugin != null)
            {
                if (_state.DistortionPlugin.Distortion != null)
                    _settings["Distortion"] = _state.DistortionPlugin.Distortion.GetType().FullName;
                else
                    _settings["Distortion"] = null;
            }

            _settings["Layout"] = _state.StereoOutput.ToString();

            _settings.Save();
        }

        public void Load()
        {
            _state.TrackerPlugin = _pluginManager.Trackers
                .Where(plugin => plugin.Tracker != null) 
                .FirstOrDefault(plugin => plugin.Tracker.GetType().FullName == _settings["Tracker"].ToString());

            _state.DistortionPlugin = _pluginManager.Distortions
                .Where(plugin => plugin.Distortion != null) 
                .FirstOrDefault(plugin => plugin.Distortion.GetType().FullName == _settings["Distortion"].ToString());

            _state.StereoOutput = (StereoMode)Enum.Parse(typeof(StereoMode), _settings["Layout"].ToString());
        }
    }
}
