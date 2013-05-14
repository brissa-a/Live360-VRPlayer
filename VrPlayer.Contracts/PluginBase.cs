namespace VrPlayer.Contracts
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
