using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.DepthMapOverUnder
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class DepthMapOverUnderPlugin : PluginBase<EffectBase>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public DepthMapOverUnderPlugin()
        {
            Name = "Depthmap Over/Under";
            var effect = new DepthMapOverUnderEffect()
            {
                MaxOffset = ConfigHelper.ParseDouble(Config.AppSettings.Settings["MaxOffset"].Value)
            };
            Content = effect;
            Panel = new DepthMapOverUnderPanel(effect);
        }
    }
}
