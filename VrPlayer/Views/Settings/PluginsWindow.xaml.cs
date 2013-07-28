using System;
using System.Linq;
using System.Windows;
using VrPlayer.Helpers;

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
                Logger.Instance.Error("Error while initilizing Plugins window.", exc);
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
