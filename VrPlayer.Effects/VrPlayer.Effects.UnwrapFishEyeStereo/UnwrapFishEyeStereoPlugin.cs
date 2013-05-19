using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.UnwrapFishEyeStereo
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class UnwrapFishEyeStereoPlugin : PluginBase<EffectBase>
    {
        public UnwrapFishEyeStereoPlugin()
        {
            Name = "Unwrap Fisheye Stereo";
            var effect = new UnwrapFishEyeStereoEffect();
            Content = effect;
            Panel = null;
        }
    }
}
