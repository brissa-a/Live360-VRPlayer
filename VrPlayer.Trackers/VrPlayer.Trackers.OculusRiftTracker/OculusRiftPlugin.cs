using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class OculusRiftPlugin : PluginBase<ITracker>
    {
        public OculusRiftPlugin()
        {
            Name = "Oculus Rift";
            var tracker = new OculusRiftTracker();
            Content = tracker;
            Panel = new OculusRiftPanel(tracker);
        }
    }
}
