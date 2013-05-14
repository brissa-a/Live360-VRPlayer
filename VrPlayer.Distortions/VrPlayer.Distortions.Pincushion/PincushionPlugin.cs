using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Helpers;

namespace VrPlayer.Distortions.Pincushion
{
    [Export(typeof(IPlugin<DistortionBase>))]
    public class PincushionPlugin : PluginBase<DistortionBase>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public PincushionPlugin()
        {
            Name = "Pincushion";
            var effect = new PincushionEffect()
            {
                Factor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Factor"].Value)
            };
            Content = effect;
            Panel = new PincushionPanel(effect);
        }
    }
}
