using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Effects;

using VrPlayer.Helpers;
using VrPlayer.Helpers.Converters;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Wrappers;
using VrPlayer.Models.Config;

namespace VrPlayer.ViewModels
{
	public class MenuViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationConfig _config;

        #region Data

		public List<MenuItem> SamplesMenu
		{
			get
			{
                var menuItems = new List<MenuItem>();

				string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + 
                    Path.DirectorySeparatorChar + _config.SamplesFolder;

                if (!Directory.Exists(folder))
                    return menuItems;
                
                string[] files = Directory.GetFiles(folder);
				foreach (var file in files)
				{
					var info = new FileInfo(file);
					var menuItem = new MenuItem
					{
					    Header = info.Name,
						Command = new DelegateCommand(Load),
						CommandParameter = info.FullName
					};
                    menuItems.Add(menuItem);
				}
				return menuItems;
			}
		}

        public List<MenuItem> EffectsMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var effectPlugin in _pluginManager.Effects)
                {
                    var menuItem = new MenuItem
                    {
                        Header = effectPlugin.Name,
                        Command = new DelegateCommand(SetEffect),
                        CommandParameter = effectPlugin,
                    };
                    var binding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("EffectPlugin"),
                        Converter = new CompareParameterConverter(),
                        ConverterParameter = effectPlugin
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        public List<MenuItem> StereoInputMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var stereoMode in Enum.GetValues(typeof(StereoMode)))
                {
                    var menuItem = new MenuItem
                    {
                        Header = stereoMode.ToString(),
                        Command = new DelegateCommand(SetStereoInput),
                        CommandParameter = stereoMode,
                    };
                    var binding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("StereoInput"),
                        Converter = new CompareStringParameterConverter(),
                        ConverterParameter = stereoMode
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        public List<MenuItem> StereoOutputMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var stereoMode in Enum.GetValues(typeof(StereoMode)))
                {
                    var menuItem = new MenuItem
                    {
                        Header = stereoMode.ToString(),
                        Command = new DelegateCommand(SetStereoOutput),
                        CommandParameter = stereoMode,
                    };
                    var binding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("StereoOutput"),
                        Converter = new CompareStringParameterConverter(),
                        ConverterParameter = stereoMode
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        public List<MenuItem> WrappersMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var wrapperPlugin in _pluginManager.Wrappers)
                {
                    var menuItem = new MenuItem
                    {
                        Header = wrapperPlugin.Name,
                        Command = new DelegateCommand(SetWrapper),
                        CommandParameter = wrapperPlugin,
                    };
                    var binding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("WrapperPlugin"),
                        Converter = new CompareParameterConverter(),
                        ConverterParameter = wrapperPlugin
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        public List<MenuItem> TrackersMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var trackerPlugin in _pluginManager.Trackers)
                {
                    var menuItem = new MenuItem
                    {
                        Header = trackerPlugin.Name,
                        Command = new DelegateCommand(SetTracker),
                        CommandParameter = trackerPlugin,
                    };
                    
                    var activeBinding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("TrackerPlugin"),
                        Converter = new CompareParameterConverter(),
                        ConverterParameter = trackerPlugin
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, activeBinding);
                    
                    var enabledBinding = new Binding
                    {
                        Source = trackerPlugin.Tracker,
                        Path = new PropertyPath("IsEnabled")
                    };
                    menuItem.SetBinding(MenuItem.IsEnabledProperty, enabledBinding);

                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        public List<MenuItem> ShadersMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var shaderPlugin in _pluginManager.Shaders)
                {
                    var menuItem = new MenuItem
                    {
                        Header = shaderPlugin.Name,
                        Command = new DelegateCommand(SetShader),
                        CommandParameter = shaderPlugin,
                    };
                    var binding = new Binding
                    {
						Source = _state,
                        Path = new PropertyPath("ShaderPlugin"),
                        Converter = new CompareParameterConverter(),
                        ConverterParameter = shaderPlugin
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
        }

        #endregion

        #region Commands

        private readonly ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get { return _loadCommand; }
        }

        private readonly ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get { return _exitCommand; }
        }

        private readonly ICommand _debugCommand;
        public ICommand DebugCommand
        {
            get { return _debugCommand; }
        }

        private readonly ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

        #endregion

        public MenuViewModel(IApplicationState state, IPluginManager pluginManager, IApplicationConfig config)
        {
            _pluginManager = pluginManager;
            _state = state;
            _config = config;

            //Commands
            _loadCommand = new DelegateCommand(Open);
            _exitCommand = new DelegateCommand(Exit);
            _debugCommand = new DelegateCommand(ShowDebug);
            _aboutCommand = new DelegateCommand(ShowAbout);

            //Todo: Extract Default values
            _state.StereoInput = StereoMode.Mono;
            _state.StereoOutput = StereoMode.Mono;
            _state.EffectPlugin = _pluginManager.Effects[0];
            _state.WrapperPlugin = _pluginManager.Wrappers[0];
            _state.TrackerPlugin = _pluginManager.Trackers[0];
            _state.ShaderPlugin = _pluginManager.Shaders[0];

            //Todo: Should not set the media value directly
            _state.Media.Source = new Uri(SamplesMenu[0].CommandParameter.ToString(), UriKind.RelativeOrAbsolute);
            _state.Media.Play();
        }

        #region Logic

        private void Open(object o)
        {
            //Todo: Extract dialog to UI layer
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Movies|*.avi;*.mpeg;*.wmv;*.flv;*.mp4;*.mkv|Images|*.jpg;*.bmp;*.gif;*.png|All Files|*.*";
            if (openFileDialog.ShowDialog().Value)
            {
                Load(openFileDialog.FileName);
            }
        }

		private void Load(object o)
		{
			string filePath = (string)o;
			_state.Media.Source = new Uri(filePath, UriKind.Absolute);
		}

        private void Exit(object o)
        {
            //Todo: Close windows instead. Move to UI
            App.Current.Shutdown();
        }

        private void SetEffect(object o)
        {
            _state.EffectPlugin = (EffectPlugin)o;
        }

        private void SetStereoInput(object o)
        {
            _state.StereoInput = (StereoMode)o;
            _state.WrapperPlugin.Wrapper.StereoMode = _state.StereoInput;
        }

        private void SetStereoOutput(object o)
        {
            _state.StereoOutput = (StereoMode)o;
        }

        private void SetWrapper(object o)
        {
            WrapperPlugin wrapperPlugin = (WrapperPlugin)o;
            wrapperPlugin.Wrapper.StereoMode = _state.StereoInput;
            _state.WrapperPlugin = wrapperPlugin;
        }

        private void SetTracker(object o)
        {
            _state.TrackerPlugin = (TrackerPlugin)o;
        }

        private void SetShader(object o)
        {
            _state.ShaderPlugin = (ShaderPlugin)o;
        }

        private void ShowDebug(object o)
        {
            DebugWindow window = new DebugWindow();
            window.Show();
        }

        private void ShowAbout(object o)
        {
            //Todo: Extract to UI layer
            MessageBox.Show(
                string.Format("VRPlayer ({0})", Assembly.GetExecutingAssembly().GetName().Version) +
                Environment.NewLine +
                Environment.NewLine +
                "(c)Stephane Levesque 2012-2013",    
                "About", 
                MessageBoxButton.OK);
        }

        #endregion

        #region Helpers

        private List<MenuItem> LoadPluginMenuItems(List<IPlugin> plugins, ICommand command, string propertyPath)
        {
            var menuItems = new List<MenuItem>();
            foreach (var plugin in plugins)
            {
                var menuItem = new MenuItem
                {
                    Header = plugin.Name,
                    Command = command,
                    CommandParameter = plugin,
                };
                var binding = new Binding
                {
                    Source = _state,
                    Path = new PropertyPath(propertyPath),
                    Converter = new CompareParameterConverter(),
                    ConverterParameter = plugin
                };
                menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
            }
            return menuItems;
        }

        #endregion
    }
}
