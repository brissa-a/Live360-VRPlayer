using System;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.State;

namespace VrPlayer.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;

        public IApplicationState State
        {
            get { return _state; }
        }

        public MainWindowViewModel(IApplicationState state)
        {
            _state = state;
        }
    }
}