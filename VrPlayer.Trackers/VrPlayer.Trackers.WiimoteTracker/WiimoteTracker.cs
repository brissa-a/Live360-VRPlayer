using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Media.Media3D;
using WiimoteLib;

using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.WiimoteTracker
{
    [Export(typeof(ITracker))]
    public class WiimoteTracker : TrackerBase, ITracker
    {
        private Wiimote _wiimote;

        public WiimoteTracker()
        {
            IsEnabled = true;
                
            try
            {
                _wiimote = new Wiimote();
                _wiimote.Connect();
                _wiimote.InitializeMotionPlus();
                _wiimote.WiimoteChanged += wiimote_WiimoteChanged;

                _wiimote.SetRumble(true);
                _wiimote.SetLEDs(true, false, false, true);
                Thread.Sleep(40);
                _wiimote.SetRumble(false);

                RawPosition = new Vector3D();
            }
            catch
            {
                try
                {
                    _wiimote.SetLEDs(false, false, false, false);
                }
                catch
                {
                }
                IsEnabled = false;
            }
        }

        void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            RawRotation = QuaternionHelper.EulerAnglesInDegToQuaternion(
            e.WiimoteState.MotionPlusState.Values.Y,
            e.WiimoteState.MotionPlusState.Values.X,
            e.WiimoteState.MotionPlusState.Values.Z);

            if (e.WiimoteState.ButtonState.Plus)
            {
                Dispatcher.Invoke((Action)(Calibrate));
            }

            Dispatcher.Invoke((Action)(UpdatePositionAndRotation));
        }

        public override void Dispose()
        {
            try
            {
                _wiimote.SetLEDs(false, false, false, false);
                _wiimote.Disconnect();
            }
            catch
            {
                
            }
            _wiimote = null;
        }
    }
}
