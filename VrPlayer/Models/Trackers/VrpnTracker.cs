using System;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Vrpn;

namespace VrPlayer.Models.Trackers
{
    public class VrpnTracker : TrackerBase, ITracker
    {
        private TrackerRemote _tracker;
        private readonly ButtonRemote _button;

        public VrpnTracker(string trackerAddress, string buttonAddress)
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.001;

                _tracker = new TrackerRemote(trackerAddress);
                _tracker.PositionChanged += new TrackerChangeEventHandler(PositionChanged);
                _tracker.MuteWarnings = true;
                
                _button = new ButtonRemote(buttonAddress);
                _button.ButtonChanged += new ButtonChangeEventHandler(ButtonChanged);
                _button.MuteWarnings = true;
                
                DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Input);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += new EventHandler(timer_Tick);
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

        private
                    void PositionChanged(object sender, TrackerChangeEventArgs e)
        {
            try
            {
                //TODO: Support user defined sensor index
                if (e.Sensor == 0)
                {
                    RawPosition = PositionScaleFactor * new Vector3D(
                        e.Position.X,
                        -e.Position.Y,
                        e.Position.Z);

                    RawRotation = new System.Windows.Media.Media3D.Quaternion(
                        e.Orientation.X,
                        -e.Orientation.Y,
                        e.Orientation.Z,
                        -e.Orientation.W);

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