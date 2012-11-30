using System.Configuration;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : IApplicationConfig
    {
        public AppSettingsApplicationConfig()
        {
            _defaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            _customBarrelWrapFactor = int.Parse(ConfigurationManager.AppSettings["CustomBarrelWrapFactor"]);
            _cameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            _mouseSensitivity = int.Parse(ConfigurationManager.AppSettings["MouseSensitivity"]);
        }

        private string _defaultMediaFile;
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
        }

        private int _customBarrelWrapFactor;
        public int CustomBarrelWrapFactor
        {
            get { return _customBarrelWrapFactor; }
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
    }
}
