using System.Windows.Media.Effects;
using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Models.Plugins
{
    public class ShaderPlugin : PluginBase, IPlugin
    {
        private DistortionBase _shader;
        public DistortionBase Shader
        {
            get { return _shader; }
        }

        public ShaderPlugin(DistortionBase shader, string name)
        {
            _shader = shader;
            Name = name;
        }
    }
}
