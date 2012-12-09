using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;

namespace VrPlayer.Models.Plugins
{
    public class EffectPlugin : PluginBase, IPlugin
    {
        private ShaderEffect _effect;
        public ShaderEffect Effect
        {
            get { return _effect; }
        }

        public EffectPlugin(ShaderEffect effect, string name)
        {
            _effect = effect;
            base.Name = name;
        }
    }
}
