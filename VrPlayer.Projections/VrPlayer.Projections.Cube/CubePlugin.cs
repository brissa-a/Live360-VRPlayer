using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Cube
{
    [Export(typeof(IPlugin<IProjection>))]
    public class CubePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public CubePlugin()
        {
            Name = "Cube";
            var projection = new CubeProjection();
            Content = projection;
            Panel = null;
        }
    }
}
