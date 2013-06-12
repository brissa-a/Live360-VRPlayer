using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Stabilization;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;

namespace VrPlayer.ViewModels
{
    public class DeshakerViewModel: ViewModelBase
	{
        private IEnumerable<DeshakerFrame> _deshakerData;
        private int _currentFrame;

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

        private readonly ICommand _setDeshakerFileCommand;
        public ICommand SetDeshakerFileCommand
        {
            get { return _setDeshakerFileCommand; }
        }

        public DeshakerViewModel(IApplicationState state, IApplicationConfig config)
        {
            _state = state;
            _config = config;

            _setDeshakerFileCommand = new DelegateCommand(SetDeshakerFile);

            var timer = new DispatcherTimer(DispatcherPriority.DataBind);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void SetDeshakerFile(object o)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Deshaker Files|*.log|All Files|*";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _deshakerData = DeshakerParser.Parse(dialog.FileName);
            }
        }

        private const double TranslationFactor = 0.01;
        private const double RotationFactor = 1;
        private const double ZoomFactor = 0.01;

        void timer_Tick(object sender, EventArgs e)
        {
            if (_state.Deshaker == null || _deshakerData == null) return;
            if (_state.MediaPlayer.MediaDuration <= 0) return;

            var progress = _state.MediaPlayer.MediaPosition / (double)_state.MediaPlayer.MediaDuration * 100;

            _currentFrame = (int)Math.Round((_deshakerData.Count() * progress) / 100);
            var frame = _deshakerData.FirstOrDefault(data => data.FrameNumber == _currentFrame);
            if (frame == null) return;

            _state.Deshaker.Translation = new Vector3D(-frame.PanX * TranslationFactor, -frame.PanY * TranslationFactor, -frame.Zoom * ZoomFactor);
            //var eulerRotation = QuaternionHelper.QuaternionToEulerAnglesInDeg(_state.Deshaker.Rotation);
            //_state.Deshaker.Rotation = QuaternionHelper.EulerAnglesInDegToQuaternion(0, 0, eulerRotation.Z - frame.Rotation * RotationFactor);
        }
    }
}