using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.VrpnTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class VrpnPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public VrpnPlugin()
        {
            Name = "VRPN";
            var tracker = new VrpnTracker()
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value),
                    RotationOffset = QuaternionHelper.EulerAnglesInDegToQuaternion(ConfigHelper.ParseVector3D(Config.AppSettings.Settings["RotationOffset"].Value)),
                    TrackerAddress = Config.AppSettings.Settings["TrackerAddress"].Value,
                    ButtonAddress = Config.AppSettings.Settings["ButtonAddress"].Value
                };
            Content = tracker;
            Panel = new VrpnPanel(tracker);
        }
    }
}
