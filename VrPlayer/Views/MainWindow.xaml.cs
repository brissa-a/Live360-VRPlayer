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
            //////////////////////////////////////////////////////////////////////
            // added 19th dec 2013 CL: 
            // ability to drag & drop files into the player window
            //////////////////////////////////////////////////////////////////////
            try
            {
                //////////////////////////////////////////////////////////////////////
                // get filename that was dropped onto the window 
                //////////////////////////////////////////////////////////////////////
                string filename = (string)((DataObject)e.Data).GetFileDropList()[0];

                //////////////////////////////////////////////////////////////////////
                // toDo: 
                // how to check if file dropped is in a format that we accept?
                //////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////
                // play it
                //////////////////////////////////////////////////////////////////////
                _viewModel.State.MediaPlugin.Content.OpenFileCommand.Execute(filename);

            }
            catch (Exception ex)
            {
                //////////////////////////////////////////////////////////////////////
                // didn't work - why?
                //////////////////////////////////////////////////////////////////////
                Console.WriteLine("Drag&Drop went wrong: " + ex.Message);
            }
        }


        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
