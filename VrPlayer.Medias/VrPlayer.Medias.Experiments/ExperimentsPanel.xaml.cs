using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace VrPlayer.Medias.Experiments
{
    public partial class ExperimentsPanel : UserControl
    {
        private readonly ExperimentsMedia _media;

        public ExperimentsPanel(ExperimentsMedia media)
        {
            _media = media;

            InitializeComponent();
            try
            {
                DataContext = media;
            }
            catch (Exception exc)
            {
            }
        }

        private void ComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            _media.Load();
        }
    }
}
