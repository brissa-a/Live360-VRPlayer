using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.NoDistortion
{
    [Export(typeof(IPlugin<DistortionBase>))]
    public class NoDistortionPlugin : PluginBase<DistortionBase>
    {
        public NoDistortionPlugin()
        {
            Name = "None";
            Content = null;
            Panel = null;
        }
    }
}
