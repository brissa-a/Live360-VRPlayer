using System;
using System.Reflection;
using System.Windows;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Contracts
{
    public abstract class PluginBase<T>: ViewModelBase, IPlugin<T> where T: ILoadable
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

        public void UpdateConfig()
        {
            foreach (var val in _config.Data)
            {
                var prop = Content.GetType().GetProperty(val.Key, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null || !prop.CanWrite) continue;
                var obj = Convert.ChangeType(val.Value, prop.PropertyType);
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