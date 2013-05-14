using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.DepthMapSbs
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class DepthMapSbsPlugin : PluginBase<EffectBase>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public DepthMapSbsPlugin()
        {
            Name = "Depthmap Side by side";
            var effect = new DepthMapSbsEffect()
            {
                MaxOffset = ConfigHelper.ParseDouble(Config.AppSettings.Settings["MaxOffset"].Value)
            };
            Content = effect;
            Panel = new DepthMapSbsPanel(effect);
        }
    }
}
