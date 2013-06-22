using System.Windows;

namespace VrPlayer.Contracts
{
    public abstract class PluginBase<T>: IPlugin<T>
    {
        public string Name { get; set; }
        public T Content { get; set; }
        public FrameworkElement Panel { get; set; }
        public virtual void Load() { } //Todo: Use virtual to force implementation
        public virtual void Unload() { } //Todo: Use virtual to force implementation
    }
}