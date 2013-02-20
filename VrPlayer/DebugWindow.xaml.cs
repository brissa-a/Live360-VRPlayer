using System;
using System.Windows;

namespace VrPlayer
{
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateDebugWindowViewModel();
            }
            catch (Exception exc)
            {
            }
        }
    }
}
