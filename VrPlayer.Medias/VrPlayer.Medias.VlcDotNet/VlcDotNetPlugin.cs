using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;

namespace VrPlayer.Medias.VlcDotNet
{
    [Export(typeof(IPlugin<IMedia>))]
    public class VlcDotNetPlugin : PluginBase<IMedia>
    {
        public VlcDotNetPlugin()
        {
            Name = "Vlc.net";
            var media = new VlcDotNetMedia();
            Content = media;
            Panel = null;
        }
    }
}