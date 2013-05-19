using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.NoTracker
{
    public class NoTrackerPlugin
    {
        [Export(typeof(IPlugin<ITracker>))]
        public class KinectPlugin : PluginBase<ITracker>
        {
            public KinectPlugin()
            {
                Name = "None";
                Content = null;
                Panel = null;
            }
        }
    }
}
