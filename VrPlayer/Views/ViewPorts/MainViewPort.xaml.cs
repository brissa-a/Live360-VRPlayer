using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using VrPlayer.Models.State;
using VrPlayer.ViewModels;
using Application = System.Windows.Application;

namespace VrPlayer.Views.ViewPorts
{
    public partial class MainViewPort
    {
        private readonly ViewPortViewModel _viewModel;
        private readonly ExternalViewPort _externalViewPort;

        public MainViewPort()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;

                //Todo: Extract to view model
                _externalViewPort = new ExternalViewPort(Resources["Geometry"] as GeometryModel3D);
                _externalViewPort.Closing += ExternalViewPortOnClosing;
                _viewModel.State.PropertyChanged += StateOnPropertyChanged;
                _viewModel.State.StereoOutput = _viewModel.State.StereoOutput;
            }
            catch (Exception exc)
            {
            }
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.ToggleNavigationCommand.Execute(null);
        }

        private void ExternalViewPortOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (!(sender is ExternalViewPort)) return;
            if (_viewModel.State.StereoOutput != LayoutMode.DualScreen) return;

            cancelEventArgs.Cancel = true;
            ((Window)sender).Hide();
            _viewModel.State.StereoOutput = LayoutMode.Mono;
        }

        private void StateOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "StereoOutput") return;

            ViewPort2.Visibility = Visibility.Visible;
            if (_viewModel.State.StereoOutput == LayoutMode.Mono ||
                _viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                ViewPort2.Visibility = Visibility.Hidden;
            }
            
            if (_viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                var secondScreenIndex = (SystemInformation.MonitorCount >= 2) ? 1 : 0;
                PositionWindowToScreen(Application.Current.MainWindow, Screen.AllScreens[0]);
                PositionWindowToScreen(_externalViewPort, Screen.AllScreens[secondScreenIndex]);
            }
            else
            {
                if(_externalViewPort != null)
                    _externalViewPort.Hide();
            }
        }

        private void PositionWindowToScreen(Window window, Screen screen)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = screen.WorkingArea.Left;
            window.Top = screen.WorkingArea.Top;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
            window.Show();
            window.WindowState = WindowState.Maximized;
            window.Focus();
        }
    }
}
