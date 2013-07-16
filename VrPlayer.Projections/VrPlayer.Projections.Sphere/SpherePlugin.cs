using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Sphere
{
    [Export(typeof(IPlugin<IProjection>))]
    public class SpherePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public SpherePlugin()
        {
            Name = "Sphere";
            var projection = new SphereProjection
                {
                    Slices = int.Parse(Config.AppSettings.Settings["Slices"].Value),
                    Stacks = int.Parse(Config.AppSettings.Settings["Stacks"].Value),
                    Width = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Width"].Value),
                    Height = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Height"].Value),
                    Depth = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Depth"].Value)
                };
            Content = projection;
            Panel = new SpherePanel(projection);
        }
    }
}