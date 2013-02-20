using System.Windows.Media.Effects;

namespace VrPlayer.Models.Plugins
{
    public class ShaderPlugin : PluginBase, IPlugin
    {
        private ShaderEffect _shader;
        public ShaderEffect Shader
        {
            get { return _shader; }
        }

        public ShaderPlugin(ShaderEffect shader, string name)
        {
            _shader = shader;
            base.Name = name;
        }
    }
}
