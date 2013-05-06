using System;
using System.Collections.Generic;

namespace VrPlayer.Models.Plugins
{
    public interface IPluginManager: IDisposable
    {
        List<EffectPlugin> Effects { get; }
        List<ProjectionPlugin> Projections { get; }
        List<TrackerPlugin> Trackers { get; }
        List<ShaderPlugin> Shaders { get; }
    }
}
