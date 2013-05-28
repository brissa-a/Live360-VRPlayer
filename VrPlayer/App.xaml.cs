using System.Windows;
using System.Windows.Navigation;
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
        private readonly IPluginManager _pluginManager;

        public ViewModelFactory ViewModelFactory { get; private set; }

        private App()
        {
            IApplicationConfig config = new AppSettingsApplicationConfig();
            _pluginManager = new DynamicPluginManager();
            IApplicationState state = new DefaultApplicationState(config);
            ViewModelFactory = new ViewModelFactory(config, _pluginManager, state);

            _settingsManager = new SettingsManager(Settings.Default, state, _pluginManager);
            _settingsManager.Load();
        }
    
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _settingsManager.Save();
            _pluginManager.Dispose();
            foreach (NavigationWindow win in Current.Windows)
            {
                win.Close();
            }
        }
    }
}
