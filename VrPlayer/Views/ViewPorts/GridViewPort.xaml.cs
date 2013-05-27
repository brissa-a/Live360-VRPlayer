using System;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.ViewModels;

namespace VrPlayer.Views.ViewPorts
{
    public partial class GridViewPort : UserControl
    {
        private readonly ViewPortViewModel _viewModel;

        public GridViewPort()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;
            }
            catch (Exception exc)
            {
            }
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.ToggleNavigationCommand.Execute(null);
        }
    }
}
