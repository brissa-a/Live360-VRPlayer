using VrPlayer.Contracts.Projections;

namespace VrPlayer.Models.Plugins
{
    public class ProjectionPlugin : PluginBase, IPlugin
    {
        private readonly IProjection _projection;
        public IProjection Projection
        {
            get { return _projection; }
        }

        public ProjectionPlugin(IProjection projection, string name)
        {
            _projection = projection;
            Name = name;
        }
    }
}
