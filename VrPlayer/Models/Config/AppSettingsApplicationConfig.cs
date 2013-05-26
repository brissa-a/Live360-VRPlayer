using System.Configuration;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : ApplicationConfigBase
    {
        public AppSettingsApplicationConfig()
        {
            DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            CameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            ViewportsHorizontalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsHorizontalOffset"]);
            PositionalAudio = bool.Parse(ConfigurationManager.AppSettings["PositionalAudio"]);
            EvrRendering = bool.Parse(ConfigurationManager.AppSettings["EvrRendering"]);
            MetaDataReadOnLoad = bool.Parse(ConfigurationManager.AppSettings["MetaDataReadOnLoad"]);
        }
    }
}
