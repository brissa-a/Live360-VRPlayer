using System.Windows.Media.Media3D;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Stabilization
{
    public class Deshaker: ViewModelBase
    {
        private Vector3D _translation;
        public Vector3D Translation
        {
            get
            {
                return _translation;
            }
            set
            {
                _translation = value;
                OnPropertyChanged("Translation");
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
    }
}
