using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.MouseTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class OculusRiftPlugin : PluginBase<ITracker>
    {
        public OculusRiftPlugin()
        {
            Name = "Mouse";
            var tracker = new MouseTracker();
            Content = tracker;
            Panel = new MousePanel(tracker);
        }
    }
}
