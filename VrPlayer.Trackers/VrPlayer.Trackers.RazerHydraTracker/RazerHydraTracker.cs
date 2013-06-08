﻿using System;
﻿using System.ComponentModel.Composition;
﻿using System.Windows;
﻿using System.Windows.Threading;
using System.Windows.Media.Media3D;

using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.RazerHydraTracker
{
    [Export(typeof(ITracker))]
    public class RazerHydraTracker: TrackerBase, ITracker
    {
        private const int HYDRA_ID = 0;
        private const int SIXENSE_BUTTON_START = 1;

        private readonly RazerHydraWrapper _hydra = new RazerHydraWrapper();

        public static readonly DependencyProperty FilterEnabledProperty =
            DependencyProperty.Register("FilterEnabledFilterEnabled", typeof(bool),
            typeof(RazerHydraTracker), new FrameworkPropertyMetadata(false));
        public bool FilterEnabled
        {
            get { return (bool)GetValue(FilterEnabledProperty); }
            set { SetValue(FilterEnabledProperty, value); }
        }

        public RazerHydraTracker()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.002;

                var result = _hydra.Init();
                ThrowErrorOnResult(result, "Error while initializing the Razer Hydra");

                var filter = FilterEnabled ? 1 : 0;
                result = _hydra.SetFilterEnabled(filter);
                ThrowErrorOnResult(result, "Error while settings the Razer Hydra filter");

                var timer = new DispatcherTimer(DispatcherPriority.Input);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            catch
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

                RawPosition = new Vector3D(
                    _hydra.Data.pos.x,
                    -_hydra.Data.pos.y,
                    _hydra.Data.pos.z);

                RawRotation = new Quaternion(
                    _hydra.Data.rot_quat.x,
                    -_hydra.Data.rot_quat.y,
                    _hydra.Data.rot_quat.z,
                    -_hydra.Data.rot_quat.w);

                if (_hydra.Data.buttons == SIXENSE_BUTTON_START)
                {
                    Calibrate();
                }

                UpdatePositionAndRotation();
            }
            catch
            {
                //Todo: log error
            }
        }

        public override void Dispose()
        {
            try
            {
                int result = _hydra.Exit();
                ThrowErrorOnResult(result, "Error shutting down the Razer Hydra");
            }
            catch
            {
            }
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