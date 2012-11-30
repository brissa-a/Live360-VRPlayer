using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

using VrPlayer.ViewModels;

namespace VrPlayer.Views
{
    public partial class ControlPanel : UserControl
    {
        private readonly MediaViewModel _viewModel;

        public ControlPanel()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateMediaViewModel();
                DataContext = _viewModel;
            }
            catch(Exception exc)
            { 
            }
        }

        #region Events

        private void ProgressBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExecuteSeekCommand(sender, e);
        }

        private void ProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Todo: This is causing lag...
                //ExecuteSeekCommand(sender, e);
            }
        }

        #endregion

        #region Helpers

        private void ExecuteSeekCommand(object sender, MouseEventArgs e)
        {
            if (_viewModel.SeekCommand.CanExecute(null))
            {
                var seekControl = (ProgressBar)sender;
                var position = e.GetPosition(seekControl).X;
                var percent = position / seekControl.ActualWidth;
                _viewModel.SeekCommand.Execute(percent);
            }
        }

        #endregion
    }
}
