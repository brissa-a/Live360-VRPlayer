using System.Windows;

using VrPlayer.Models;
using VrPlayer.Models.Config;
using VrPlayer.Models.State;
using VrPlayer.Models.Plugins;
using VrPlayer.ViewModels;

namespace VrPlayer
{
    public partial class App : Application
    {
        private readonly ViewModelFactory _viewModelFactory;
        public ViewModelFactory ViewModelFactory
        {
            get { return _viewModelFactory; }
        }

        private App()
        {
            IApplicationConfig config = new AppSettingsApplicationConfig();
            IPluginManager pluginManager = new StaticPluginManager(config);
            IApplicationState state = new DefaultApplicationState(config);
            _viewModelFactory = new ViewModelFactory(config, pluginManager, state);
        }
    }
}
