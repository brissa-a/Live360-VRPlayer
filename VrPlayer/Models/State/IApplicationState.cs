using System.ComponentModel;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Trackers;
using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Plugins;

namespace VrPlayer.Models.State
{
    public interface IApplicationState : INotifyPropertyChanged
    {
        MediaUriElement MediaPlayer { get; }
        IPlugin<EffectBase> EffectPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        StereoMode StereoOutput { get; set; }
        IPlugin<IProjection> ProjectionPlugin { get; set; }
        IPlugin<ITracker> TrackerPlugin { get; set; }
        IPlugin<DistortionBase> DistortionPlugin { get; set; }
    }
}