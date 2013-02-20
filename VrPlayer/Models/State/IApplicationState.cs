using System.ComponentModel;

using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Models.Plugins;
using VrPlayer.Models.Wrappers;

namespace VrPlayer.Models.State
{
    public interface IApplicationState : INotifyPropertyChanged
    {
        MediaUriElement MediaPlayer { get; }
        EffectPlugin EffectPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        StereoMode StereoOutput { get; set; }
        WrapperPlugin WrapperPlugin {get; set;}
        TrackerPlugin TrackerPlugin {get; set;}
        ShaderPlugin ShaderPlugin { get; set; }
    }
}
