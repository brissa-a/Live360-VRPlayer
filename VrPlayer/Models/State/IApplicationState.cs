using System.Windows.Controls;
using System.Windows.Media.Effects;

using VrPlayer.Models.Trackers;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Wrappers;

namespace VrPlayer.Models.State
{
    public interface IApplicationState
    {
        MediaElement Media { get; }
        StereoMode StereoMode { get; set; }
        WrapperPlugin WrapperPlugin {get; set;}
        TrackerPlugin TrackerPlugin {get; set;}
        ShaderPlugin ShaderPlugin { get; set; }
    }
}
