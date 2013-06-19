using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Config
{
    public abstract class ApplicationConfigBase : ViewModelBase, IApplicationConfig
    {
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

        private bool _positionalAudio;
        public bool PositionalAudio
        {
            get { return _positionalAudio; }
            set
            {
                _positionalAudio = value;
                OnPropertyChanged("PositionalAudio");
            }
        }

        private bool _evrRendering;
        public bool EvrRendering
        {
            get { return _evrRendering; }
            set
            {
                _evrRendering = value;
                OnPropertyChanged("EvrRendering");
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
    }
}
