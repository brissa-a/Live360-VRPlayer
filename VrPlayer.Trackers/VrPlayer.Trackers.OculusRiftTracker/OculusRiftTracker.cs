using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Threading;
using System.Windows.Media.Media3D;

using RiftDotNet;
using VrPlayer.Contracts.Trackers;
using log4net;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [Export(typeof(ITracker))]
    public class OculusRiftTracker: TrackerBase, ITracker
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IHMDDevice _rift;
        private readonly ISensorFusion _sensor;

        public OculusRiftTracker()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 1;

                var manager = Factory.CreateDeviceManager();

                var hmds = manager.HMDDevices;
                _rift = hmds[0].CreateDevice();

                var sensors = manager.SensorDevices;
                var sensor = sensors[0].CreateDevice();
                _sensor = Factory.CreateSensorFusion(sensor);
                
                var timer = new DispatcherTimer(DispatcherPriority.Send);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                IsEnabled = false;
            }  
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                RawRotation = new Quaternion(
                    _sensor.Orientation.X,
                    -_sensor.Orientation.Y,
                    _sensor.Orientation.Z,
                    -_sensor.Orientation.W);

                UpdatePositionAndRotation();
            }
            catch(Exception exc)
            {
                Log.Error(exc.Message);
            }
        }

        public override void Dispose()
        {
            if(_rift != null)
                _rift.Dispose();
        }
    }
}