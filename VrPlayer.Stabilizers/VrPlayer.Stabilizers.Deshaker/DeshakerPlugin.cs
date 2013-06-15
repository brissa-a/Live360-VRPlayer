using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Deshaker
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class DeshakerPlugin : PluginBase<IStabilizer>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public DeshakerPlugin()
        {
            Name = "Deshaker";
            var stabilizer = new DeshakerStabilizer
            {
                TranslationFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["TranslationFactor"].Value),
                RotationFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["RotationFactor"].Value),
                ZoomFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["ZoomFactor"].Value)
            };
            Content = stabilizer;
            Panel = new DeshakerPanel(stabilizer);
        }
    }
}