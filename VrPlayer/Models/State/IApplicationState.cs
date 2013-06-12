using System.ComponentModel;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Models.Stabilization;
using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Contracts.Projections;

namespace VrPlayer.Models.State
{
    public interface IApplicationState : INotifyPropertyChanged
    {
        MediaUriElement MediaPlayer { get; }
        IPlugin<EffectBase> EffectPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        LayoutMode StereoOutput { get; set; }
        IPlugin<IProjection> ProjectionPlugin { get; set; }
        IPlugin<ITracker> TrackerPlugin { get; set; }
        IPlugin<DistortionBase> DistortionPlugin { get; set; }
        Deshaker Deshaker { get; set; }
    }
}