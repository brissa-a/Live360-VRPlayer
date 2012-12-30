using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Trackers
{
    public abstract class TrackerBase: ViewModelBase
    {
        public Quaternion Quaternion { get; set; }
        public Vector3D Position { get; set; }
        
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public abstract void Dispose();
    }
}
