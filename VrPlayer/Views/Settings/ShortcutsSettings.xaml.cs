using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VrPlayer.Views.Dialogs;

namespace VrPlayer.Views.Settings
{
    public partial class ShortcutsSettings : UserControl
    {
        public ShortcutsSettings()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = Brushes.Yellow;
            var dialog = new KeyInputDialog();
            if (dialog.ShowDialog() == true)
            {
                textBox.Text = dialog.Key.ToString();
                textBox.Background = Brushes.White;
            }
        }
    }
}
