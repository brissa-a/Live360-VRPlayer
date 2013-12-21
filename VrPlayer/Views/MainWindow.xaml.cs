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

        private readonly MenuViewModel _menuViewModel;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateMainWindowViewModel();
                DataContext = _viewModel;

                _menuViewModel = ((App)Application.Current).ViewModelFactory.CreateMenuViewModel();

            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing Main window.", exc);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.KeyboardCommand.Execute(e.Key);
        }

        private void Window_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                //////////////////////////////////////////////////////////////////////
                // get filename that was dropped onto the window 
                //////////////////////////////////////////////////////////////////////
                var filename = ((DataObject)e.Data).GetFileDropList()[0];

                //////////////////////////////////////////////////////////////////////
                // play it
                //////////////////////////////////////////////////////////////////////
                _viewModel.MediaService.Load(filename);
            }
            catch (Exception exc)
            {
                //////////////////////////////////////////////////////////////////////
                // didn't work - why?
                //////////////////////////////////////////////////////////////////////
                Logger.Instance.Error("Error with the Drag&Drop", exc);
            }
        }


        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
