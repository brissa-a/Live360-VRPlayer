using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.VlcDotNet
{
    [Export(typeof(IPlugin<IMedia>))]
    public class VlcDotNetPlugin : PluginBase<IMedia>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
  
        public VlcDotNetPlugin()
        {
            Name = "Vlc.net";
            var media = new VlcDotNetMedia()
            {
                DebugMode = bool.Parse(Config.AppSettings.Settings["DebugMode"].Value),
                LibVlcDllsPath = Config.AppSettings.Settings["LibVlcDllsPath"].Value,
                LibVlcPluginsPath = Config.AppSettings.Settings["LibVlcPluginsPath"].Value
            };
            Content = media;
            Panel = new VlcDotNetPanel(media);
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