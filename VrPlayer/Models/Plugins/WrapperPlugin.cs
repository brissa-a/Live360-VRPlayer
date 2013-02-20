using VrPlayer.Models.Wrappers;

namespace VrPlayer.Models.Plugins
{
    public class WrapperPlugin : PluginBase, IPlugin
    {
        private IWrapper _wrapper;
        public IWrapper Wrapper
        {
            get { return _wrapper; }
        }

        public WrapperPlugin(IWrapper wrapper, string name)
        {
            _wrapper = wrapper;
            base.Name = name;
        }
    }
}
