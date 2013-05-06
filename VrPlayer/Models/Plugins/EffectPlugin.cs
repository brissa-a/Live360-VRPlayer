using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Models.Plugins
{
    public class EffectPlugin : PluginBase, IPlugin
    {
        private readonly EffectBase _effect;
        public EffectBase Effect
        {
            get { return _effect; }
        }

        public EffectPlugin(EffectBase effect, string name)
        {
            _effect = effect;
            base.Name = name;
        }
    }
}
