using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.UnwrapFishEye
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class UnwrapFishEyePlugin : PluginBase<EffectBase>
    {
        public UnwrapFishEyePlugin()
        {
            Name = "Unwrap Fisheye";
            var effect = new UnwrapFishEyeEffect();
            Content = effect;
            Panel = null;
        }
    }
}
