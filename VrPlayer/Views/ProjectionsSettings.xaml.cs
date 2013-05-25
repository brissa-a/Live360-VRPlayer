using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;

namespace VrPlayer.Views
{
    public partial class ProjectionsSettings : UserControl
    {
        public ProjectionsSettings()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tabControl = ((TabControl)sender);
            tabControl.SelectedItem = tabControl.Items.Cast<IPlugin<IProjection>>().First(plugin => plugin.Panel != null);
        }
    }
}
