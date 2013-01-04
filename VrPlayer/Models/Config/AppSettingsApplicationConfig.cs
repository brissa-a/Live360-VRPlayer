using System.Configuration;
using System.Globalization;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : IApplicationConfig
    {
        public AppSettingsApplicationConfig()
        {
            _defaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            _samplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            _customPincushionFactor = double.Parse(ConfigurationManager.AppSettings["CustomPincushionFactor"]);
            _cameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            _mouseSensitivity = int.Parse(ConfigurationManager.AppSettings["MouseSensitivity"]);
            _depthMapMaxOffset = ParseDouble(ConfigurationManager.AppSettings["DepthMapMaxOffset"]);
            _colorKeyAlphaColor = ConfigurationManager.AppSettings["ColorKeyAlphaColor"];
            _colorKeyTolerance = ParseDouble(ConfigurationManager.AppSettings["ColorKeyTolerance"]);
            _orientationRefreshRateInMS = int.Parse(ConfigurationManager.AppSettings["OrientationRefreshRateInMS"]);
            _viewportLeftRightSpacing = int.Parse(ConfigurationManager.AppSettings["ViewportLeftRightSpacing"]);
            _viewportVerticalDistance = int.Parse(ConfigurationManager.AppSettings["ViewportVerticalDistance"]);
            _viewportTopBottomSpacing = int.Parse(ConfigurationManager.AppSettings["ViewportTopBottomSpacing"]);
            _viewportHorizontalDistance = int.Parse(ConfigurationManager.AppSettings["ViewportHorizontalDistance"]);
        }

        private string _defaultMediaFile;
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
        }

        private string _samplesFolder;
        public string SamplesFolder
        {
            get { return _samplesFolder; }
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

        private int _orientationRefreshRateInMS;
        public int OrientationRefreshRateInMS
        {
            get { return _orientationRefreshRateInMS; }
        }

        private double ParseDouble(string value)
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(value, NumberStyles.Any, ci);
        }

        private int _viewportLeftRightSpacing;
        public int ViewportLeftRightSpacing
        {
            get { return _viewportLeftRightSpacing; }
        }

        private int _viewportVerticalDistance;
        public int ViewportVerticalDistance
        {
            get { return _viewportVerticalDistance; }
        }

        private int _viewportTopBottomSpacing;
        public int ViewportTopBottomSpacing
        {
            get { return _viewportTopBottomSpacing; }
        }

        private int _viewportHorizontalDistance;
        public int ViewportHorizontalDistance
        {
            get { return _viewportHorizontalDistance; }
        }
    }
}
