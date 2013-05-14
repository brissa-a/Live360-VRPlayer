using System;
using System.Collections.Generic;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Models.Plugins
{
    public interface IPluginManager: IDisposable
    {
        List<EffectPlugin> Effects { get; }
        List<ProjectionPlugin> Projections { get; }
        List<TrackerPlugin> Trackers { get; }
        List<DistortionPlugin> Distortions { get; }
    }
}
