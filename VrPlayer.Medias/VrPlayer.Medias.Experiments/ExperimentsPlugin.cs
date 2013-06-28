using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;

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
