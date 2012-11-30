using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VrPlayer.Models.Plugins
{
    public abstract class PluginBase
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
