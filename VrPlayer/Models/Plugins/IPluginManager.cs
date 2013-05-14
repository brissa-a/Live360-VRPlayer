using System;
using System.Collections.Generic;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Models.Plugins
{
    public interface IPluginManager: IDisposable
    {
        IEnumerable<IPlugin<EffectBase>> Effects { get; }
        IEnumerable<IPlugin<IProjection>> Projections { get; }
        IEnumerable<IPlugin<ITracker>> Trackers { get; }
        IEnumerable<IPlugin<DistortionBase>> Distortions { get; }
    }
}
