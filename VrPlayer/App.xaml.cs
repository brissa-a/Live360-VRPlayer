using System.Windows;

using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Settings;
using VrPlayer.Models.State;
using VrPlayer.Properties;
using VrPlayer.ViewModels;

namespace VrPlayer
{
    public partial class App : Application
    {
        private readonly SettingsManager _settingsManager;

        public ViewModelFactory ViewModelFactory { get; private set; }

        private App()
        {
            IApplicationConfig config = new AppSettingsApplicationConfig();
            IPluginManager pluginManager = new StaticPluginManager(config);
            IApplicationState state = new DefaultApplicationState(config);
            ViewModelFactory = new ViewModelFactory(config, pluginManager, state);

            _settingsManager = new SettingsManager(Settings.Default, state, pluginManager);
            _settingsManager.Load();
        }
    
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _settingsManager.Save();
        }
    }
}
