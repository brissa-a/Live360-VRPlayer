using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;

namespace VrPlayer.Medias.NoMedia
{
    [Export(typeof(IPlugin<IMedia>))]
    public class NoMediaPlugin : PluginBase<IMedia>
    {
        public NoMediaPlugin()
        {
            Name = "None";
            Content = null;
            Panel = null;
        }
    }
}
