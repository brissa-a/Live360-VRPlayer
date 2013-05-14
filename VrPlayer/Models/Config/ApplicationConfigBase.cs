using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Config
{
    public abstract class ApplicationConfigBase : ViewModelBase, IApplicationConfig
    {
        private string _defaultMediaFile;
        private string _samplesFolder;
        private int _cameraFieldOfView;
        private int _orientationRefreshRateInMs;
        private int _viewportsHorizontalOffset;
        private bool _positionalAudio;
        private bool _evrRendering;
        private bool _metaDataReadOnLoad;
        
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
            set
            {
                _defaultMediaFile = value;
                OnPropertyChanged("DefaultMediaFile");
            }
        }

        public string SamplesFolder
        {
            get { return _samplesFolder; }
            set 
            { 
                _samplesFolder = value;
                OnPropertyChanged("SamplesFolder");
            }
        }
        
        public int CameraFieldOfView
        {
            get { return _cameraFieldOfView; }
            set
            {
                _cameraFieldOfView = value;
                OnPropertyChanged("CameraFieldOfView");
            }
        }

        public int OrientationRefreshRateInMs
        {
            get { return _orientationRefreshRateInMs; }
            set
            {
                _orientationRefreshRateInMs = value;
                OnPropertyChanged("OrientationRefreshRateInMs");
            }
        }

        public int ViewportsHorizontalOffset
        {
            get { return _viewportsHorizontalOffset; }
            set
            {
                _viewportsHorizontalOffset = value;
                OnPropertyChanged("ViewportsHorizontalOffset");
            }
        }

        public bool PositionalAudio
        {
            get { return _positionalAudio; }
            set
            {
                _positionalAudio = value;
                OnPropertyChanged("PositionalAudio");
            }
        }

        public bool EvrRendering
        {
            get { return _evrRendering; }
            set
            {
                _evrRendering = value;
                OnPropertyChanged("EvrRendering");
            }
        }

        public bool MetaDataReadOnLoad
        {
            get { return _metaDataReadOnLoad; }
            set
            {
                _metaDataReadOnLoad = value;
                OnPropertyChanged("MetaDataReadOnLoad");
            }
        }
    }
}
