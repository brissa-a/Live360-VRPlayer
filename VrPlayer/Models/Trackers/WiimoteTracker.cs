using System;
using System.Windows.Media.Media3D;

using WiimoteLib;

using VrPlayer.Helpers;

namespace VrPlayer.Models.Trackers
{
    public class WiimoteTracker : TrackerBase, ITracker
    {
        private Wiimote _wiimote = new Wiimote();

        public WiimoteTracker()
        {
            //Todo: Use a connecion method in the Tracker Interface
            try
            {
                _wiimote.Connect();
                _wiimote.InitializeMotionPlus();
                _wiimote.WiimoteChanged += new EventHandler<WiimoteChangedEventArgs>(wiimote_WiimoteChanged);
            }
            catch (Exception exc)
            {
                //Todo: Error handling
                //throw new Exception("Error while initializing Wiimote tracker.", exc);
            }
        }

        void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            Quaternion = QuaternionHelper.FromEulerAngles(
                e.WiimoteState.MotionPlusState.Values.Y,
                e.WiimoteState.MotionPlusState.Values.X,
                e.WiimoteState.MotionPlusState.Values.Z);
        }

        public override void Dispose()
        {
            _wiimote.Disconnect();
            _wiimote = null;
        }
    }
}
