using System;
using System.Windows;
using System.Windows.Input;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;

namespace VrPlayer
{
    public partial class MainWindow : FullScreenWindow
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateMainWindowViewModel();
                DataContext = _viewModel;
            }
            catch (Exception exc)
            {
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.KeyboardCommand.Execute(e.Key);
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
