using System;
using System.Linq;
using System.Windows;

namespace VrPlayer.Views.Settings
{
    public partial class PluginsWindow : Window
    {
        public PluginsWindow()
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

        public static void ShowSingle()
        {
            var window = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.GetType() == typeof(PluginsWindow));
            if (window != null)
            {
                window.Activate();
            }
            else
            {
                window = new PluginsWindow();
                window.Show();
            }
        }
    }
}
