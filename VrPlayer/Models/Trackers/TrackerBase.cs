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
        private Vector3D _position;
        public Vector3D Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                OnPropertyChanged("Rotation");
            }
        }

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

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private double _positionScaleFactor;
        public double PositionScaleFactor
        {
            get
            {
                return _positionScaleFactor;
            }
            set
            {
                _positionScaleFactor = value;
                OnPropertyChanged("PositionScaleFactor");
            }
        }

        private Vector3D _basePosition;
        public Vector3D BasePosition
        {
            get
            {
                return _basePosition;
            }
            set
            {
                _basePosition = value;
                OnPropertyChanged("BasePosition");
            }
        }

        private Quaternion _baseRotation;
        public Quaternion BaseRotation
        {
            get
            {
                return _baseRotation;
            }
            set
            {
                _baseRotation = value;
                OnPropertyChanged("BaseRotation");
            }
        }

        public abstract void Dispose();
    }
}
