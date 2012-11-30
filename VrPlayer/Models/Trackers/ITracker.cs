using System;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Trackers
{
    public interface ITracker: IDisposable
    {
        Quaternion Quaternion { get; set; }
        bool IsActive { get; set; }
    }
}
