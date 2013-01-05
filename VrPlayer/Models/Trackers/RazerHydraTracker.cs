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
                result = _hydra.SetFilterEnabled(0);
                
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
            int result = _hydra.GetNewestData(HYDRA_ID);

            Vector3D scaledPos = new Vector3D(
                _hydra.Data.pos.x * PositionScaleFactor,
                -_hydra.Data.pos.y * PositionScaleFactor,
                _hydra.Data.pos.z * PositionScaleFactor);

            if (_hydra.Data.buttons == SIXENSE_BUTTON_START)
            {
                this.BasePosition = -scaledPos;
            }

            this.Rotation = new Quaternion(
                _hydra.Data.rot_quat.x,
                -_hydra.Data.rot_quat.y,
                _hydra.Data.rot_quat.z,
                -_hydra.Data.rot_quat.w);

            this.Position = BasePosition + scaledPos;
        }

        public override void Dispose()
        {
            int result = _hydra.Exit();
        }
    }
}