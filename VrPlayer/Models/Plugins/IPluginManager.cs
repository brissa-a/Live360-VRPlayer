using System.Collections.Generic;

namespace VrPlayer.Models.Plugins
{
    public interface IPluginManager
    {
        List<EffectPlugin> Effects { get; }
        List<WrapperPlugin> Wrappers { get; }
        List<TrackerPlugin> Trackers { get; }
        List<ShaderPlugin> Shaders { get; }
    }
}
