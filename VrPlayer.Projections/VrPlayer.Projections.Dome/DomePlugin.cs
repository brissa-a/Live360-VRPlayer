using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Dome
{
    [Export(typeof(IPlugin<IProjection>))]
    public class DomePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public DomePlugin()
        {
            Name = "Dome";
            var projection = new DomeProjection
                {
                    Slices = int.Parse(Config.AppSettings.Settings["Slices"].Value),
                    Stacks = int.Parse(Config.AppSettings.Settings["Stacks"].Value)
                };
            Content = projection;
            Panel = new DomePanel(projection);
        }
    }
}
