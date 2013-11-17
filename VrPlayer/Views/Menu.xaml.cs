using System;
using System.Windows;
using System.Windows.Forms;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;

namespace VrPlayer.Views
{
    public partial class Menu : UserControl
    {
        private readonly MenuViewModel _viewModel;

        public Menu()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateMenuViewModel();
                DataContext = _viewModel;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing Menu view.", exc);
            }
        }

        private void SavePresetMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"VR Player Presets|*.json";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _viewModel.SaveMediaPresetCommand.Execute(saveFileDialog.FileName);
            }
        }

        private void LoadPresetMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"VR Player Presets|*.json";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _viewModel.LoadMediaPresetCommand.Execute(openFileDialog.FileName);
            }
        }
    }
}
