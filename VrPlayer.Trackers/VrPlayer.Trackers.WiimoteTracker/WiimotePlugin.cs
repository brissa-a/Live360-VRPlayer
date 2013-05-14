using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.WiimoteTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class WiimotePlugin : PluginBase<ITracker>
    {
        public WiimotePlugin()
        {
            Name = "Nintendo Wiimote";
            var tracker = new WiimoteTracker();
            Content = tracker;
            Panel = new WiimotePanel(tracker);
        }
    }
}
