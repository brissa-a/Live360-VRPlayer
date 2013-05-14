namespace VrPlayer.Contracts.Trackers
{
    public class TrackerPlugin : PluginBase, IPlugin
    {
        private readonly ITracker _tracker;
        public ITracker Tracker
        {
            get { return _tracker; }
        }

        public TrackerPlugin(ITracker tracker, string name)
        {
            _tracker = tracker;
            Name = name;
        }
    }
}
