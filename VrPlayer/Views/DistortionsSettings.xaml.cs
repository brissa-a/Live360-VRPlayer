using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Views
{
    public partial class DistortionsSettings : UserControl
    {
        public DistortionsSettings()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tabControl = ((TabControl)sender);
            tabControl.SelectedItem = tabControl.Items.Cast<IPlugin<DistortionBase>>().First(plugin => plugin.Panel != null);
        }
    }
}
