using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.KinectTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class KinectPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public KinectPlugin()
        {
            Name = "Microsoft Kinect";
            var tracker = new KinectTracker
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value)
                };
            Content = tracker;
            Panel = new KinectPanel(tracker);
        }
    }
}
