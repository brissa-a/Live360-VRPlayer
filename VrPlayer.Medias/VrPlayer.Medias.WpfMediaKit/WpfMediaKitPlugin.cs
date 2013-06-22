using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.WpfMediaKit
{
    [Export(typeof(IPlugin<IMedia>))]
    public class WpfMediaKitPlugin : PluginBase<IMedia>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public WpfMediaKitPlugin()
        {
            Name = "WPF Media Kit";
            var media = new WpfMediaKitMedia
            {
                PositionalAudio = bool.Parse(Config.AppSettings.Settings["PositionalAudio"].Value),
                EvrRendering = bool.Parse(Config.AppSettings.Settings["EvrRendering"].Value)
            };
            Content = media;
            Panel = new WpfMediaKitPanel(media);
        }

        public override void Load()
        {
            if (Content != null)
                Content.Load();
        }

        public override void Unload()
        {
            if (Content != null)
                Content.Unload();
        }
    }
}
