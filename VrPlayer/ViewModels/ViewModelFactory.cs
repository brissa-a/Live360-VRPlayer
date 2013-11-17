using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.State;

namespace VrPlayer.ViewModels
{
    public class ViewModelFactory
    {
        private readonly IApplicationConfig _config;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationState _state;
        private readonly IPresetsManager _presetsManager;

        public ViewModelFactory(IApplicationConfig config, IPluginManager pluginManager, IApplicationState state, IPresetsManager presetsManager)
        {
            _config = config;
            _pluginManager = pluginManager;
            _state = state;
            _presetsManager = presetsManager;
        }

        public MainWindowViewModel CreateMainWindowViewModel()
        {
            return new MainWindowViewModel(_state, _config);
        }

        public ViewPortViewModel CreateViewPortViewModel()
        {
            return new ViewPortViewModel(_state, _config);
        }

        public MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_state, _pluginManager, _config, _presetsManager);
        }

        public MediaViewModel CreateMediaViewModel()
        {
            return new MediaViewModel(_state);
        }

        public SettingsWindowViewModel CreateSettingsWindowViewModel()
        {
            return new SettingsWindowViewModel(_state, _config, _pluginManager);
        }
    }
}
