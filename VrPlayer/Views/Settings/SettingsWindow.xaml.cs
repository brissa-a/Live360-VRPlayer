using System;
using System.Windows;

namespace VrPlayer
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateSettingsWindowViewModel();
            }
            catch (Exception exc)
            {
            }
        }
    }
}
