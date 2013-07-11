using System.ComponentModel.Composition;
using System.Configuration;
using System.Reflection;
using System.Resources;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Sphere
{
    [Export(typeof(IPlugin<IProjection>))]
    public class SpherePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        private static readonly ResourceManager RessourcesManager = new ResourceManager("Resource", Assembly.GetExecutingAssembly());

        public SpherePlugin()
        {
            Name = "Sphere";// ressourcesManager.GetString("Name", CultureInfo.InvariantCulture);
            var projection = new SphereProjection
                {
                    Slices = int.Parse(Config.AppSettings.Settings["Slices"].Value),
                    Stacks = int.Parse(Config.AppSettings.Settings["Stacks"].Value),
                    Angle = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Angle"].Value)
                };
            Content = projection;
            Panel = new SpherePanel(projection);
        }
    }
}
