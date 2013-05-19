using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.NoEffect
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class NoEffectPlugin : PluginBase<EffectBase>
    {
        public NoEffectPlugin()
        {
            Name = "None";
            Content = null;
            Panel = null;
        }
    }
}
