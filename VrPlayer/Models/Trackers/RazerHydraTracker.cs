using System;
using System.Windows.Threading;
using System.Windows.Media.Media3D;

using RazerHydraWrapper;
using System.IO;

namespace VrPlayer.Models.Trackers
{
    public class RazerHydraTracker: TrackerBase, ITracker
    {
        private const int HYDRA_ID = 0;
        private const int SIXENSE_BUTTON_START = 1;

        private RazerHydra _hydra = new RazerHydra();

        public RazerHydraTracker()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.002;

                int result = _hydra.Init();
                ThrowErrorOnResult(result, "Error while initializing the Razer Hydra");

                result = _hydra.SetFilterEnabled(0);
                ThrowErrorOnResult(result, "Error while enabling the Razer Hydra filter");

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

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                int result = _hydra.GetNewestData(HYDRA_ID);
                ThrowErrorOnResult(result, "Error while getting data from the Razer Hydra");

                Vector3D scaledPos = new Vector3D(
                    _hydra.Data.pos.x * PositionScaleFactor,
                    -_hydra.Data.pos.y * PositionScaleFactor,
                    _hydra.Data.pos.z * PositionScaleFactor);

                Quaternion rotation = new Quaternion(
                    _hydra.Data.rot_quat.x,
                    -_hydra.Data.rot_quat.y,
                    _hydra.Data.rot_quat.z,
                    -_hydra.Data.rot_quat.w);

                if (_hydra.Data.buttons == SIXENSE_BUTTON_START)
                {
                    Quaternion conjugate = new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
                    conjugate.Conjugate();
                    this.BaseRotation = conjugate;
                    this.BasePosition = -scaledPos;
                }

                this.Rotation = this.BaseRotation * rotation;
                this.Position = this.BasePosition + scaledPos;
            }
            catch
            {
                //Todo: log error
            }
        }

        public override void Dispose()
        {
            int result = _hydra.Exit();
            ThrowErrorOnResult(result, "Error shutting down the Razer Hydra");
        }

        private void ThrowErrorOnResult(int result, string message)
        {
            if (result == -1)
            {
                throw new Exception(message);
            }
        }
    }
}