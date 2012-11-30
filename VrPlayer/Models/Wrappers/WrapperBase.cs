using System.Windows.Media;
using System.Windows.Media.Media3D;

using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Wrappers
{
    public abstract class WrapperBase: ViewModelBase
    {
        private StereoMode _stereoMode;
        private Vector3D _cameraLeftPosition;
        private Vector3D _cameraRightPosition;

        public StereoMode StereoMode
        {
            get { return _stereoMode; }
            set 
            {
                _stereoMode = value;
                OnPropertyChanged("StereoMode");
                OnPropertyChanged("Geometry");
            }
        }
        
        public WrapperBase()
        {
            _stereoMode = StereoMode.Mono;
            Vector3D defaultCameraPosition = new Vector3D(0, 0, 0);
            _cameraLeftPosition = defaultCameraPosition;
            _cameraRightPosition = defaultCameraPosition;
        }

        public MeshGeometry3D Geometry 
        {
            get
            {
                MeshGeometry3D geometry = new MeshGeometry3D();
                geometry.Positions = Positions;
                geometry.TriangleIndices = TriangleIndices;
                switch (_stereoMode)
                {
                    case StereoMode.OverUnder:
                        geometry.TextureCoordinates = OverUnderTextureCoordinates;
                        break;
                    case StereoMode.SideBySide:
                        geometry.TextureCoordinates = SideBySideTextureCoordinates;
                        break;
                    default:
                        geometry.TextureCoordinates = MonoTextureCoordinates;
                        break;
                }
                return geometry;
            }
        }

        public Vector3D CameraLeftPosition
        {
            get { return _cameraLeftPosition; }
        }

        public Vector3D CameraRightPosition
        {
            get { return _cameraRightPosition; }
        }

        public abstract Point3DCollection Positions { get; }
        public abstract Int32Collection TriangleIndices { get; }
        public abstract PointCollection MonoTextureCoordinates { get; }
        public abstract PointCollection OverUnderTextureCoordinates { get; }
        public abstract PointCollection SideBySideTextureCoordinates { get; }
    }
}
