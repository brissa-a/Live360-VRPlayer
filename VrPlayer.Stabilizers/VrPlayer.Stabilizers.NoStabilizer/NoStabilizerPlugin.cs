using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;

namespace VrPlayer.Stabilizers.NoStabilizer
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class NoStabilizerPlugin : PluginBase<IStabilizer>
    {
        public NoStabilizerPlugin()
        {
            Name = "None";
            Content = null;
            Panel = null;
        }
    }
}
