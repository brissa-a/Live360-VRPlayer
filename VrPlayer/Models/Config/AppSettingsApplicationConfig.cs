using System.Configuration;
using System.Globalization;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : ApplicationConfigBase
    {
        public AppSettingsApplicationConfig()
        {
            DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            BarrelFactor = double.Parse(ConfigurationManager.AppSettings["BarrelFactor"]);
            PincushionFactor = double.Parse(ConfigurationManager.AppSettings["PincushionFactor"]);
            CameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            MouseSensitivity = int.Parse(ConfigurationManager.AppSettings["MouseSensitivity"]);
            DepthMapMaxOffset = ParseDouble(ConfigurationManager.AppSettings["DepthMapMaxOffset"]);
            ColorKeyAlphaColor = ConfigurationManager.AppSettings["ColorKeyAlphaColor"];
            ColorKeyTolerance = ParseDouble(ConfigurationManager.AppSettings["ColorKeyTolerance"]);
            OrientationRefreshRateInMS = int.Parse(ConfigurationManager.AppSettings["OrientationRefreshRateInMS"]);
            ViewportsHorizontalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsHorizontalOffset"]);
            HydraPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["HydraPositionScaleFactor"]);
            HydraPitchOffset = ParseDouble(ConfigurationManager.AppSettings["HydraPitchOffset"]);
            PsMovePositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["PsMovePositionScaleFactor"]);
            KinectPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["KinectPositionScaleFactor"]);
            VrpnPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["VrpnPositionScaleFactor"]);
            VrpnTrackerAddress = ConfigurationManager.AppSettings["VrpnTrackerAddress"];
            VrpnButtonAddress = ConfigurationManager.AppSettings["VrpnButtonAddress"];
            PositionalAudio = bool.Parse(ConfigurationManager.AppSettings["PositionalAudio"]);
            EvrRendering = bool.Parse(ConfigurationManager.AppSettings["EvrRendering"]);
            DomeSlices = int.Parse(ConfigurationManager.AppSettings["DomeSlices"]);
            DomeStacks = int.Parse(ConfigurationManager.AppSettings["DomeStacks"]);
            SphereSlices = int.Parse(ConfigurationManager.AppSettings["SphereSlices"]);
            SphereStacks = int.Parse(ConfigurationManager.AppSettings["SphereStacks"]);
            CylinderSlices = int.Parse(ConfigurationManager.AppSettings["CylinderSlices"]);
            CylinderStacks = int.Parse(ConfigurationManager.AppSettings["CylinderStacks"]);
        }
        
        #region Helpers

        private double ParseDouble(string value)
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(value, NumberStyles.Any, ci);
        }

        #endregion
    }
}
