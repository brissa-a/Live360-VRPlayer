using System.ComponentModel;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;
using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Contracts.Projections;

namespace VrPlayer.Models.State
{
    public interface IApplicationState : INotifyPropertyChanged
    {
        MediaUriElement MediaPlayer { get; }
        StereoMode StereoInput { get; set; }
        LayoutMode StereoOutput { get; set; }
        IPlugin<EffectBase> EffectPlugin { get; set; }
        IPlugin<IProjection> ProjectionPlugin { get; set; }
        IPlugin<ITracker> TrackerPlugin { get; set; }
        IPlugin<DistortionBase> DistortionPlugin { get; set; }
        IPlugin<IStabilizer> StabilizerPlugin { get; set; }
    }
}