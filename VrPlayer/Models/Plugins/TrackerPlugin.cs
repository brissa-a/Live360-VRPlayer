using VrPlayer.Models.Trackers;

namespace VrPlayer.Models.Plugins
{
    public class TrackerPlugin : PluginBase, IPlugin
    {
        private ITracker _tracker;
        public ITracker Tracker
        {
            get { return _tracker; }
        }

        public TrackerPlugin(ITracker tracker, string name)
        {
            _tracker = tracker;
            base.Name = name;
        }
    }
}
