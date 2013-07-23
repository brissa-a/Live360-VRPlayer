using System;
using System.Reflection;
using System.Windows;
using VrPlayer.Helpers;

namespace VrPlayer.Contracts
{
    public abstract class PluginBase<T>: IPlugin<T> where T: ILoadable
    {
        public string Name { get; set; }
        public T Content { get; set; }
        public FrameworkElement Panel { get; set; }

        private PluginConfig _config;
        public PluginConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;
                UpdateConfig();
            }
        }

        //Todo: Extract to util
        public void UpdateConfig()
        {
            foreach (var val in _config.Data)
            {
                var prop = Content.GetType().GetProperty(val.Key, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null || !prop.CanWrite) continue;

                object obj = val.Value;

                if (prop.PropertyType == typeof(int))
                    obj = int.Parse(val.Value);
                else if (prop.PropertyType == typeof(double))
                    obj = ConfigHelper.ParseDouble(val.Value);
                else if (prop.PropertyType == typeof(bool))
                    obj = bool.Parse(val.Value);

                prop.SetValue(Content, obj, null);
            }
        }

        public void Load()
        {
            if (Content != null)
                Content.Load();
        }

        public void Unload()
        {
            if (Content != null)
                Content.Unload();
        }
    }
}