using System.ComponentModel.Composition;
using System.Configuration;
using System.Reflection;
using System.Resources;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Cylinder
{
    [Export(typeof(IPlugin<IProjection>))]
    public class CylinderPlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public CylinderPlugin()
        {
            Name = "Cylinder";
            var projection = new CylinderProjection
                {
                    Scale = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Scale"].Value),
                    Slices = int.Parse(Config.AppSettings.Settings["Slices"].Value),
                    Stacks = int.Parse(Config.AppSettings.Settings["Stacks"].Value),
                    Angle = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Angle"].Value)
                };
            Content = projection;
            Panel = new CylinderPanel(projection);
        }
    }
}
