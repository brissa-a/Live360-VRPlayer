using System.Windows.Input;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Config
{
    public abstract class ApplicationConfigBase : ViewModelBase, IApplicationConfig
    {
        protected ApplicationConfigBase()
        {
            KeyPlayPause = Key.Space;
            KeyStop = Key.S;
            KeyPrevious = Key.A;
            KeyNext = Key.D;
            KeyLoop = Key.F;
            KeyMoveLeft = Key.Left;
            KeyMoveRight = Key.Right;
            KeyMoveForward = Key.Up;
            KeyMoveBackward = Key.Down;
            KeyMoveUp = Key.PageUp;
            KeyMoveDown = Key.PageDown;
            KeyTrackerCalibrate = Key.F1;
            KeyTrackerReset = Key.F2;
            KeyFieldOfViewMinus = Key.O;
            KeyFieldOfViewPlus = Key.P;
            KeyHorizontalOffsetMinus = Key.K;
            KeyHorizontalOffsetPlus = Key.L;
            KeyVerticalOffsetMinus = Key.N;
            KeyVerticalOffsetPlus = Key.M;
        }

        private string _defaultMediaFile;
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
            set
            {
                _defaultMediaFile = value;
                OnPropertyChanged("DefaultMediaFile");
            }
        }

        private string _samplesFolder;
        public string SamplesFolder
        {
            get { return _samplesFolder; }
            set 
            { 
                _samplesFolder = value;
                OnPropertyChanged("SamplesFolder");
            }
        }
        
        private int _cameraFieldOfView;
        public int CameraFieldOfView
        {
            get { return _cameraFieldOfView; }
            set
            {
                _cameraFieldOfView = value;
                OnPropertyChanged("CameraFieldOfView");
            }
        }

        private int _viewportsHorizontalOffset;
        public int ViewportsHorizontalOffset
        {
            get { return _viewportsHorizontalOffset; }
            set
            {
                _viewportsHorizontalOffset = value;
                OnPropertyChanged("ViewportsHorizontalOffset");
            }
        }

        private int _viewportsVerticalOffset;
        public int ViewportsVerticalOffset
        {
            get { return _viewportsVerticalOffset; }
            set
            {
                _viewportsVerticalOffset = value;
                OnPropertyChanged("ViewportsVerticalOffset");
            }
        }

        private bool _metaDataReadOnLoad;
        public bool MetaDataReadOnLoad
        {
            get { return _metaDataReadOnLoad; }
            set
            {
                _metaDataReadOnLoad = value;
                OnPropertyChanged("MetaDataReadOnLoad");
            }
        }

        private string _defaultMedia;
        public string DefaultMedia
        {
            get { return _defaultMedia; }
            set
            {
                _defaultMedia = value;
                OnPropertyChanged("DefaultMedia");
            }
        }

        private string _defaultEffect;
        public string DefaultEffect
        {
            get { return _defaultEffect; }
            set
            {
                _defaultEffect = value;
                OnPropertyChanged("DefaultEffect");
            }
        }

        private string _defaultDistortion;
        public string DefaultDistortion
        {
            get { return _defaultDistortion; }
            set
            {
                _defaultDistortion = value;
                OnPropertyChanged("DefaultDistortion");
            }
        }

        private string _defaultProjection;
        public string DefaultProjection
        {
            get { return _defaultProjection; }
            set
            {
                _defaultProjection = value;
                OnPropertyChanged("DefaultProjection");
            }
        }
        
        private string _defaultTracker;
        public string DefaultTracker
        {
            get { return _defaultTracker; }
            set
            {
                _defaultTracker = value;
                OnPropertyChanged("DefaultTracker");
            }
        }

        private string _defaultStabilizer;
        public string DefaultStabilizer
        {
            get { return _defaultStabilizer; }
            set
            {
                _defaultStabilizer = value;
                OnPropertyChanged("DefaultStabilizer");
            }
        }

        public Key KeyPlayPause { get; set; }
        public Key KeyStop { get; set; }
        public Key KeyNext { get; set; }
        public Key KeyPrevious { get; set; }
        public Key KeyLoop { get; set; }
        public Key KeyMoveLeft { get; set; }
        public Key KeyMoveRight { get; set; }
        public Key KeyMoveForward { get; set; }
        public Key KeyMoveBackward { get; set; }
        public Key KeyMoveUp { get; set; }
        public Key KeyMoveDown { get; set; }
        public Key KeyTrackerCalibrate { get; set; }
        public Key KeyTrackerReset { get; set; }
        public Key KeyFieldOfViewMinus { get; set; }
        public Key KeyFieldOfViewPlus { get; set; }
        public Key KeyHorizontalOffsetMinus { get; set; }
        public Key KeyHorizontalOffsetPlus { get; set; }
        public Key KeyVerticalOffsetMinus { get; set; }
        public Key KeyVerticalOffsetPlus { get; set; }
    }
}
