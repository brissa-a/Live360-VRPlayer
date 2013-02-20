using System;
using System.Linq;
using System.Windows.Media.Media3D;

using Microsoft.Kinect;

namespace VrPlayer.Models.Trackers
{
    public class KinectTracker : TrackerBase, ITracker
    {
        private KinectSensor _kinect;

        public KinectTracker()
        {
            try
            {
                this.IsEnabled = true;
                this.PositionScaleFactor = 1.5;

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

                _kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_kinect_AllFramesReady);
                _kinect.Start();

                _rawRotation = Quaternion.Identity;
            }
            catch (Exception exc)
            {
                IsEnabled = false;
            }
        }

        void _kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            Skeleton skeleton = GetFirstSkeleton(e);
            if (skeleton == null)
            {
                return;
            }
            SkeletonPoint point = skeleton.Joints[JointType.Head].Position;
            _rawPosition = PositionScaleFactor * new Vector3D(point.X, -point.Y, point.Z);

            //Raising right hand for calibration..
            if (skeleton.Joints[JointType.HandRight].Position.Y > point.Y)
            {
                Calibrate();
            }

            UpdatePositionAndRotation();
        }

        public override void Dispose()
        {
            if (_kinect != null)
            {
                if (_kinect.IsRunning)
                {
                    //stop sensor 
                    _kinect.Stop();

                    //stop audio if not null
                    if (_kinect.AudioSource != null)
                    {
                        _kinect.AudioSource.Stop();
                    }
                }
            }
        }

        private Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            Skeleton[] allSkeletons = new Skeleton[6];

            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }

                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;
            }
        }
    }
}
