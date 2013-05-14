namespace VrPlayer.Contracts.Distortions
{
    public class DistortionPlugin : PluginBase, IPlugin
    {
        private readonly DistortionBase _distortion;
        public DistortionBase Distortion
        {
            get { return _distortion; }
        }

        public DistortionPlugin(DistortionBase distortion, string name)
        {
            _distortion = distortion;
            Name = name;
        }
    }
}
