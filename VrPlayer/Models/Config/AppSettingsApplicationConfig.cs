using System.Configuration;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : IApplicationConfig
    {
        public AppSettingsApplicationConfig()
        {
            _defaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            _customPincushionFactor = double.Parse(ConfigurationManager.AppSettings["CustomPincushionFactor"]);
            _cameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            _mouseSensitivity = int.Parse(ConfigurationManager.AppSettings["MouseSensitivity"]);
            _depthMapMaxOffset = double.Parse(ConfigurationManager.AppSettings["DepthMapMaxOffset"]);
            _colorKeyAlphaColor = ConfigurationManager.AppSettings["ColorKeyAlphaColor"];
            _colorKeyTolerance = double.Parse(ConfigurationManager.AppSettings["ColorKeyTolerance"]);
        }

        private string _defaultMediaFile;
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
        }

        private double _customPincushionFactor;
        public double CustomPincushionFactor
        {
            get { return _customPincushionFactor; }
        }

        private int _cameraFieldOfView;
        public int CameraFieldOfView
        {
            get { return _cameraFieldOfView; }
        }

        private int _mouseSensitivity;
        public int MouseSensitivity
        {
            get { return _mouseSensitivity; }
        }

        private double _depthMapMaxOffset;
        public double DepthMapMaxOffset
        {
            get { return _depthMapMaxOffset; }
        }

        private string _colorKeyAlphaColor;
        public string ColorKeyAlphaColor
        {
            get { return _colorKeyAlphaColor; }
        }

        private double _colorKeyTolerance;
        public double ColorKeyTolerance
        {
            get { return _colorKeyTolerance; }
        }
    }
}
