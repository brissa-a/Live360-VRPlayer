using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Views
{
    public partial class EffectsSettings : UserControl
    {
        public EffectsSettings()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tabControl = ((TabControl)sender);
            tabControl.SelectedItem = tabControl.Items.Cast<IPlugin<EffectBase>>().First(plugin => plugin.Panel != null);
        }
    }
}
