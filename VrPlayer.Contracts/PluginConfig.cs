using System;
using System.Collections.Generic;
using System.Configuration;

namespace VrPlayer.Contracts
{
    public class PluginConfig
    {
        public Dictionary<string, string> Data { get; set; }

        public PluginConfig() 
        {
            Data = new Dictionary<string, string>();
        }

        public static PluginConfig FromSettings(KeyValueConfigurationCollection settings)
        {
            var config = new PluginConfig();
            foreach (var key in settings.AllKeys)
            {
                config.Data.Add(key, settings[key].Value);
            }
            return config;
        }

        public override string ToString()
        {
            var result = "";
            
            if (Data == null)
                return result;

            foreach (var val in Data)
            {
                result += val.Key + "=" + val.Value + Environment.NewLine;
            }

            return result;
        }
    }
}
