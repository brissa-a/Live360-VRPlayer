using System.ComponentModel;
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
        EffectPlugin EffectPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        StereoMode StereoOutput { get; set; }
        ProjectionPlugin ProjectionPlugin {get; set;}
        TrackerPlugin TrackerPlugin {get; set;}
        DistortionPlugin DistortionPlugin { get; set; }
    }
}