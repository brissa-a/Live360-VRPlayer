using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Models.State;
using VrPlayer.Models.Wrappers;

namespace VrPlayer.ViewModels
{
    public class ViewPortViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        #region Fields

		public MediaUriElement Media
		{
			get
			{
				return _state.Media;
			}
		}

		public IWrapper Wrapper
		{
			get
			{
				return _state.WrapperPlugin.Wrapper;
			}
		}

		private double _fov;
        public double Fov
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                OnPropertyChanged("Fov");
            }
        }

		private Quaternion _cameraTransform;
		public Quaternion CameraTransform
		{
			get
			{
				return _cameraTransform;
			}
			set
			{
				_cameraTransform = value;
				OnPropertyChanged("CameraTransform");
			}
		}

        #endregion

        #region Commands

        private readonly ICommand _toggleNavigationCommand;
        public ICommand ToggleNavigationCommand
        {
            get { return _toggleNavigationCommand; }
        }

        #endregion

        public ViewPortViewModel(IApplicationState state, IApplicationConfig config)
        {
            _state = state;
            _config = config;

            //Default Values
            Fov = config.CameraFieldOfView;

            //Commands
            _toggleNavigationCommand = new RelayCommand(ToggleNavigation);

            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Input);
            timer.Interval = new TimeSpan(0, 0, 0, 0, config.OrientationRefreshRateInMS);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            CameraTransform = _state.TrackerPlugin.Tracker.Rotation;
        }

        #region Logic

        private void ToggleNavigation(object o)
        {
            _state.TrackerPlugin.Tracker.IsActive = !_state.TrackerPlugin.Tracker.IsActive;
        }

        #endregion
    }
}
