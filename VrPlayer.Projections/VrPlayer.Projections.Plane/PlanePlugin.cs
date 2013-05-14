using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Plane
{
    [Export(typeof(IPlugin<IProjection>))]
    public class PlanePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
       
        public PlanePlugin()
        {
            Name = "Plane";
            var projection = new PlaneProjection
                {
                    Ratio = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Ratio"].Value)
                };
            Content = projection;
            Panel = new PlanePanel(projection);
        }
    }
}
