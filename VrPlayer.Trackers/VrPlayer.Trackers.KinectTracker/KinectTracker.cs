using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.KinectTracker
{
    [Export(typeof(ITracker))]
    public class KinectTracker : TrackerBase, ITracker
    {
        private readonly KinectSensor _kinect;

        public KinectTracker()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 1.5;

                _kinect = KinectSensor.KinectSensors[0];

                var parameters = new TransformSmoothParameters
                {
                    Smoothing = 0.3f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f
                };
                _kinect.SkeletonStream.Enable(parameters);

                _kinect.AllFramesReady += _kinect_AllFramesReady;
                _kinect.Start();

                RawRotation = Quaternion.Identity;
            }
            catch (Exception exc)
            {
                IsEnabled = false;
            }
        }

        void _kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            var skeleton = GetFirstSkeleton(e);
            if (skeleton == null)
            {
                return;
            }
            var point = skeleton.Joints[JointType.Head].Position;
            RawPosition = new Vector3D(point.X, -point.Y, point.Z);

            //Raising right hand for calibration..
            if (skeleton.Joints[JointType.HandRight].Position.Y > point.Y)
            {
                Dispatcher.Invoke((Action)(Calibrate));
            }

            Dispatcher.Invoke((Action)(UpdatePositionAndRotation));
        }

        public override void Dispose()
        {
            if (_kinect == null) return;
            if (!_kinect.IsRunning) return;
            
            //stop sensor 
            _kinect.Stop();

            //stop audio if not null
            if (_kinect.AudioSource != null)
            {
                _kinect.AudioSource.Stop();
            }
        }

        private Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            var allSkeletons = new Skeleton[6];

            using (var skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }

                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                var first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;
            }
        }
    }
}
