using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.MouseTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class OculusRiftPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public OculusRiftPlugin()
        {
            Name = "Mouse";
            var tracker = new MouseTracker
                {
                    Sensitivity = ConfigHelper.ParseDouble(Config.AppSettings.Settings["Sensitivity"].Value)
                };
            Content = tracker;
            Panel = new MousePanel(tracker);
        }
    }
}
