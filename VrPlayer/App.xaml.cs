using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using VrPlayer.Helpers;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.Settings;
using VrPlayer.Models.State;
using VrPlayer.Properties;
using VrPlayer.Services;
using VrPlayer.ViewModels;

namespace VrPlayer
{
    public partial class App : Application
    {
        private const string DefaultMedia = @"Samples\1-Grid.jpg";

        private readonly IApplicationConfig _appConfig;
        private readonly SettingsManager _settingsManager;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationState _appState;
        private readonly IPresetsManager _presetsManager;
        private readonly IMediaSevice _mediaService;

        public ViewModelFactory ViewModelFactory { get; private set; }

        private App()
        {
            try
            {
                var applicationPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                string[] pluginFolders = { "Medias", "Effects", "Distortions", "Trackers", "Projections", "Stabilizers" };
            
                //Init services and inject dependancies
                _appConfig = new AppSettingsApplicationConfig();
                _pluginManager = new DynamicPluginManager(applicationPath, pluginFolders);
                _appState = new DefaultApplicationState(_appConfig, _pluginManager);
                _settingsManager = new SettingsManager(_appState, _pluginManager, _appConfig, Settings.Default);
                _mediaService = new MediaService(_appState, _pluginManager);
                _presetsManager = new PresetsManager(_appConfig, _appState, _pluginManager);
                
                ViewModelFactory = new ViewModelFactory(_appConfig, _pluginManager, _appState, _presetsManager);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initializing application.", exc);
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                //Set default culture
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                //Load settings
                _settingsManager.Load();
                
                //Load media and preset from command line arguments
                var args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                    _mediaService.Load(args[1]);
                else
                    _mediaService.Load(Path.GetFullPath(DefaultMedia));

                if (args.Length > 2)
                    _presetsManager.LoadFromUri(args[2]);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while starting up application.", exc);
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
