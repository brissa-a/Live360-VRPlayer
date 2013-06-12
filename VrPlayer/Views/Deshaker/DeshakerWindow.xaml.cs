using System;
using System.Linq;
using System.Windows;
using Application = System.Windows.Application;

namespace VrPlayer.Views.Deshaker
{
    public partial class DeshakerWindow : Window
    {
        public DeshakerWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateDeshakerViewModel();
            }
            catch (Exception exc)
            {
            }
        }

        public static void ShowSingle()
        {
            var window = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.GetType() == typeof(DeshakerWindow));
            if (window != null)
            {
                window.Activate();
            }
            else
            {
                window = new DeshakerWindow();
                window.Show();
            }
        }
    }
}