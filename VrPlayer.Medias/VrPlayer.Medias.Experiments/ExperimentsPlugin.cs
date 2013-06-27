using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.Experiments
{
    [Export(typeof (IPlugin<IMedia>))]
    public class ExperimentsPlugin : PluginBase<IMedia>
    {
        public ExperimentsPlugin()
        {
            Name = "Experiments";
            var media = new ExperimentsMedia();
            Content = media;
            Panel = new ExperimentsPanel(media);
        }
    }
}
