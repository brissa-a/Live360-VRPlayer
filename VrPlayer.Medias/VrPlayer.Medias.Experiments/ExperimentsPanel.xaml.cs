using System;
using System.Windows.Controls;

namespace VrPlayer.Medias.Experiments
{
    public partial class ExperimentsPanel : UserControl
    {
        public ExperimentsPanel(ExperimentsMedia media)
        {
            InitializeComponent();
            try
            {
                DataContext = media;
            }
            catch (Exception exc)
            {
            }
        }
    }
}
