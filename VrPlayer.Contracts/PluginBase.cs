using System.Windows;

namespace VrPlayer.Contracts
{
    public abstract class PluginBase<T>: IPlugin<T> where T: ILoadable
    {
        public string Name { get; set; }
        public T Content { get; set; }
        public FrameworkElement Panel { get; set; }

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