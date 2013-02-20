using System;
using System.Windows;
using System.Windows.Controls;

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
            }
        }
    }
}
