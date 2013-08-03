using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using VrPlayer.Helpers;
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
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                IApplicationConfig config = new AppSettingsApplicationConfig();
                _pluginManager = new DynamicPluginManager();
                IApplicationState state = new DefaultApplicationState(config, _pluginManager);
                
                ViewModelFactory = new ViewModelFactory(config, _pluginManager, state);

                _settingsManager = new SettingsManager(Settings.Default, state, _pluginManager, config);
                _settingsManager.Load();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initializing application.", exc);
            }
        }
    
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                _settingsManager.Save();
                _pluginManager.Dispose();
                foreach (NavigationWindow win in Current.Windows)
                {
                    win.Close();
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while closing application.", exc);
            }
            
        }
    }
}
