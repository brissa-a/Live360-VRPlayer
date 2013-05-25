using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Views
{
    public partial class TrackersSettings : UserControl
    {
        public TrackersSettings()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tabControl = ((TabControl)sender);
            tabControl.SelectedItem = tabControl.Items.Cast<IPlugin<ITracker>>().First(plugin => plugin.Panel != null);
        }
    }
}
