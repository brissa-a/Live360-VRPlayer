using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using VrPlayer.ViewModels;
using Application = System.Windows.Application;

namespace VrPlayer.Views.ViewPorts
{
    public partial class GridViewPort
    {
        private readonly ViewPortViewModel _viewModel;

        public GridViewPort()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;

                var leftViewPort = new LeftViewPort(Resources["Geometry"] as GeometryModel3D);
                PositionWindowsToScreen(leftViewPort, Screen.AllScreens[0]);

                if (SystemInformation.MonitorCount >= 2)
                {
                    var rightViewPort = new RightViewPort(Resources["Geometry"] as GeometryModel3D);
                    PositionWindowsToScreen(rightViewPort, Screen.AllScreens[1]);
                }
            }
            catch (Exception exc)
            {
            }
        }

        private void PositionWindowsToScreen(Window window, Screen screen)
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

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.ToggleNavigationCommand.Execute(null);
        }
    }
}
