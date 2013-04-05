using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;

namespace VrPlayer.ViewModels
{
    public class SettingsWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        private readonly IApplicationConfig _config;

        public IApplicationState State
        {
            get { return _state; }
        }

        public IApplicationConfig Config
        {
            get { return _config; }
        }

        public SettingsWindowViewModel(IApplicationState state, IApplicationConfig config)
        {
            _state = state;
            _config = config;
        }
    }
}