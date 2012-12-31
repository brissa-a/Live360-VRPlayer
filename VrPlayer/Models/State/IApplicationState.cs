using System.Windows.Controls;
using System.Windows.Media.Effects;

using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Models.Trackers;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Wrappers;

namespace VrPlayer.Models.State
{
    public interface IApplicationState
    {
        MediaUriElement Media { get; }
        EffectPlugin EffectPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        StereoMode StereoOutput { get; set; }
        WrapperPlugin WrapperPlugin {get; set;}
        TrackerPlugin TrackerPlugin {get; set;}
        ShaderPlugin ShaderPlugin { get; set; }
    }
}
