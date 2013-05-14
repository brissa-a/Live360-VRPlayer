using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.ColorKeyAlpha
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class ColorKeyAlphaPlugin : PluginBase<EffectBase>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public ColorKeyAlphaPlugin()
        {
            Name = "ColorKey Alpha";
            var effect = new ColorKeyAlphaEffect()
            {
                ColorKey = ConfigHelper.ParseColor(Config.AppSettings.Settings["AlphaColor"].Value),
                Tolerance = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Tolerance"].Value)
            };
            Content = effect;
            Panel = new ColorKeyAlphaPanel(effect);
        }
    }
}
