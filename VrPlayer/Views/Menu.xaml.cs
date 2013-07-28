using System;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Helpers;

namespace VrPlayer.Views
{
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateMenuViewModel();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing Menu view.", exc);
            }
        }
    }
}
