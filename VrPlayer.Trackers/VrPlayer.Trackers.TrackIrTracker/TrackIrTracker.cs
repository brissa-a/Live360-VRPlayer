using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.TrackIrTracker
{
    unsafe public class TrackIrTracker : TrackerBase, ITracker
    {
        [DllImport(@"TrackIrWrapper.dll")]
        static extern int TIR_Init();

        [DllImport(@"TrackIrWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int TIR_Exit();

        [DllImport(@"TrackIrWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int TIR_Update(float* x, float* y, float* z, float* pitch, float* yaw, float* roll);

        [DllImport(@"TrackIrWrapper.dll")]
        static extern int TIR_ReCenter();

        private const double UnitsByDeg = Int16.MaxValue / 180;

        public TrackIrTracker()
        {
            Logger.Instance.Info("Initializing Track IR", null);
            IsEnabled = false;
            var timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void Init()
        {
            if (Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero)
            {
                try
                {
                    var result = TIR_Init();
                    ThrowErrorOnResult(result, "Error while initializing Track IR");
                    IsEnabled = true;
                }
                catch (Exception exc)
                {
                    Logger.Instance.Error(exc.Message, exc);
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!IsEnabled)
            {
                Init();
            }
            else
            {
                try
                {
                    float x, y, z, pitch, yaw, roll;
                    var result = TIR_Update(&x, &y, &z, &pitch, &yaw, &roll);
                    ThrowErrorOnResult(result, "Error while getting data from the Track IR");

                    RawPosition = new Vector3D(-x,-y,z);
                    RawRotation = QuaternionHelper.EulerAnglesInDegToQuaternion(
                        -(yaw - Int16.MaxValue) / UnitsByDeg,
                        -(pitch - Int16.MaxValue) / UnitsByDeg,
                        (roll - Int16.MaxValue) / UnitsByDeg);

                    UpdatePositionAndRotation();
                }
                catch(Exception exc)
                {
                    Logger.Instance.Error(exc.Message, exc);
                    Init();
                }    
            }
        }

        public override void Calibrate()
        {
            var result = TIR_ReCenter();
            ThrowErrorOnResult(result, "Error while re-centering Track IR");
            base.Calibrate();
        }

        public override void Dispose()
        {
            var result = TIR_Exit();
            ThrowErrorOnResult(result, "Error while shuting down Track IR");
        }

        private void ThrowErrorOnResult(int result, string message)
        {
            if (result != 0)
            {
                throw new Exception(message);
            }
        }
    }
}
