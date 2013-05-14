using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.PsMoveTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class PsMoveTrackerPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public PsMoveTrackerPlugin()
        {
            Name = "Playstation Move";
            var tracker = new PsMoveTracker()
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value)
                };
            Content = tracker;
            Panel = new PsMovePanel(tracker);
        }
    }
}
