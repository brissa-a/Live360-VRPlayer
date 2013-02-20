using System.Windows.Media.Effects;

namespace VrPlayer.Models.Plugins
{
    public class EffectPlugin : PluginBase, IPlugin
    {
        private readonly ShaderEffect _effect;
        public ShaderEffect Effect
        {
            get { return _effect; }
        }

        public EffectPlugin(ShaderEffect effect, string name)
        {
            _effect = effect;
            base.Name = name;
        }
    }
}
