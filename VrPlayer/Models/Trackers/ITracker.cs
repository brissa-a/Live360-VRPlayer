using System;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Trackers
{
    public interface ITracker: IDisposable
    {
        bool IsActive { get; set; }
        bool IsEnabled { get; set; }
        Quaternion BaseRotation { get; set; }
        Quaternion Rotation { get; set; }
        Vector3D BasePosition { get; set; }
        Vector3D Position { get; set; }
        double PositionScaleFactor { get; set; }
    }
}
