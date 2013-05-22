using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.TrackIrTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class TrackIrPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public TrackIrPlugin()
        {
            Name = "NaturalPoint TrackIR";
            var tracker = new TrackIrTracker
            {
                PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value)
            };
            Content = tracker;
            Panel = new TrackIrPanel(tracker);
        }
    }
}
