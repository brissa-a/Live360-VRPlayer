using System;
using System.Windows.Threading;
using System.Windows.Media.Media3D;

using RazerHydraWrapper;
using System.IO;

namespace VrPlayer.Models.Trackers
{
    public class RazerHydraTracker: TrackerBase, ITracker
    {
        private const int _id = 0;
        private RazerHydra _hydra = new RazerHydra();

        public RazerHydraTracker()
        {
            try
            {
                IsEnabled = true;

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
            
            int result = _hydra.GetNewestData(_id);
            
            this.Rotation = new Quaternion(
                _hydra.Data.rot_quat.x,
                -_hydra.Data.rot_quat.y,
                _hydra.Data.rot_quat.z,
                -_hydra.Data.rot_quat.w);

            this.Position = new Vector3D(
                _hydra.Data.pos.x/1000,
                _hydra.Data.pos.y/1000,
                _hydra.Data.pos.z/1000-0.75);
        }

        public override void Dispose()
        {
            int result = _hydra.Exit();
        }
    }
}