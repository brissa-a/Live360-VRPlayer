using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;

namespace VrPlayer.ViewModels
{
    public class SettingsWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        private readonly IPluginManager _pluginManager;
        public IPluginManager PluginManager
        {
            get { return _pluginManager; }
        }

        public SettingsWindowViewModel(IApplicationState state, IApplicationConfig config, IPluginManager pluginManager)
        {
            _state = state;
            _config = config;
            _pluginManager = pluginManager;
        }
    }
}