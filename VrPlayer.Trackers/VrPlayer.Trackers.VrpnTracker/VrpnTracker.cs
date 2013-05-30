using System;
using System.ComponentModel.Composition;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Vrpn;

using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.VrpnTracker
{
    [Export(typeof(ITracker))]
    public class VrpnTracker : TrackerBase, ITracker
    {
        private TrackerRemote _tracker;
        private readonly ButtonRemote _button;

        private string _trackerAddress;
        public string TrackerAddress
        {
            get
            {
                return _trackerAddress;
            }
            set
            {
                _trackerAddress = value;
                OnPropertyChanged("TrackerAddress");
            }
        }

        private string _buttonAddress;
        public string ButtonAddress
        {
            get
            {
                return _buttonAddress;
            }
            set
            {
                _buttonAddress = value;
                OnPropertyChanged("ButtonAddress");
            }
        }
        
        public VrpnTracker()
        {
            const string trackerAddress = "";
            const string buttonAddress = "";
            
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.001;

                _tracker = new TrackerRemote(trackerAddress);
                _tracker.PositionChanged += PositionChanged;
                _tracker.MuteWarnings = true;
                
                _button = new ButtonRemote(buttonAddress);
                _button.ButtonChanged += ButtonChanged;
                _button.MuteWarnings = true;
                
                var timer = new DispatcherTimer(DispatcherPriority.Input);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            catch (Exception exc)
            {
                IsEnabled = false;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                _tracker.Update();
                _button.Update();
            }
            catch
            {
            }
        }

        private void PositionChanged(object sender, TrackerChangeEventArgs e)
        {
            try
            {
                //TODO: Support user defined sensor index or autodetect
                if (e.Sensor == 0)
                {
                    RawPosition = PositionScaleFactor * new Vector3D(
                        -e.Position.X,
                        -e.Position.Z,
                        e.Position.Y);

                    RawRotation = new System.Windows.Media.Media3D.Quaternion(
                        e.Orientation.Y,
                        e.Orientation.W,
                        -e.Orientation.X,
                        e.Orientation.Z);

                    UpdatePositionAndRotation();
                }
            }
            catch
            {
                //Todo: log error
            }
        }

        private void ButtonChanged(object sender, ButtonChangeEventArgs e)
        {
            if (e.Button == 0)
                Calibrate();
        }

        public override void Dispose()
        {
            _tracker = null;
        }
    }
}