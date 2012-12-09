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

namespace VrPlayer.ViewModels
{
	public class MenuViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;

        #region Data

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

        public List<MenuItem> StereoModeMenu
        {
            get
            {
                var menuItems = new List<MenuItem>();
                foreach (var stereoMode in Enum.GetValues(typeof(StereoMode)))
                {
                    var menuItem = new MenuItem
                    {
                        Header = stereoMode.ToString(),
                        Command = new DelegateCommand(SetStereoMode),
                        CommandParameter = stereoMode,
                    };
                    var binding = new Binding
                    {
                        Source = _state,
                        Path = new PropertyPath("StereoMode"),
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
                    var binding = new Binding
                    {
						Source = _state,
                        Path = new PropertyPath("TrackerPlugin"),
                        Converter = new CompareParameterConverter(),
                        ConverterParameter = trackerPlugin
                    };
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
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

        private readonly ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

        #endregion

        public MenuViewModel(IApplicationState state, IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
            _state = state;

            //Commands
            _loadCommand = new DelegateCommand(Load);
            _exitCommand = new DelegateCommand(Exit);
            _aboutCommand = new DelegateCommand(About);

            //Todo: Extract Default values
            _state.StereoMode = StereoMode.Mono;
            _state.EffectPlugin = _pluginManager.Effects[0];
            _state.WrapperPlugin = _pluginManager.Wrappers[0];
            _state.TrackerPlugin = _pluginManager.Trackers[0];
            _state.ShaderPlugin = _pluginManager.Shaders[0];
        }

        #region Logic

        private void Load(object o)
        {
            //Todo: Extract dialog to UI layer
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Movies (*.mpeg,*.wmv,*.avi)|*.mpeg;*.wmv;*.avi|Images (*.jpg,*.png,*.bmp,*.gif)|*.jpg;*.bmp;*.gif;*.png|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog().Value)
            {
                //Todo: Bind source change event?
                _state.Media.Source = new Uri(openFileDialog.FileName, UriKind.Absolute);
            }
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

        private void SetStereoMode(object o)
        {
            _state.StereoMode = (StereoMode)o;
            _state.WrapperPlugin.Wrapper.StereoMode = _state.StereoMode;
        }

        private void SetWrapper(object o)
        {
            WrapperPlugin wrapperPlugin = (WrapperPlugin)o;
            wrapperPlugin.Wrapper.StereoMode = _state.StereoMode;
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

        private void About(object o)
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
