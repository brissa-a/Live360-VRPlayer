using System.Configuration;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : ApplicationConfigBase
    {
        public AppSettingsApplicationConfig()
        {
            DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            CameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            ViewportsHorizontalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsHorizontalOffset"]);
            ViewportsVerticalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsVerticalOffset"]);
            SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            MetaDataReadOnLoad = bool.Parse(ConfigurationManager.AppSettings["MetaDataReadOnLoad"]);
            
            PositionalAudio = bool.Parse(ConfigurationManager.AppSettings["PositionalAudio"]);
            EvrRendering = bool.Parse(ConfigurationManager.AppSettings["EvrRendering"]);

            DefaultEffect = ConfigurationManager.AppSettings["DefaultEffect"];
            DefaultDistortion = ConfigurationManager.AppSettings["DefaultDistortion"];
            DefaultProjection = ConfigurationManager.AppSettings["DefaultProjection"];
            DefaultTracker = ConfigurationManager.AppSettings["DefaultTracker"];
        }
    }
}
