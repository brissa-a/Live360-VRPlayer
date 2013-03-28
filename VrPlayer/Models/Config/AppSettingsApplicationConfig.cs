using System.Configuration;
using System.Globalization;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : IApplicationConfig
    {
        public AppSettingsApplicationConfig()
        {
            DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            CustomPincushionFactor = double.Parse(ConfigurationManager.AppSettings["CustomPincushionFactor"]);
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
        }

        public string DefaultMediaFile { get; set; }
        public string SamplesFolder { get; set; }
        public double CustomPincushionFactor { get; set; }
        public int CameraFieldOfView { get; set; }
        public int MouseSensitivity { get; set; }
        public double DepthMapMaxOffset { get; set; }
        public string ColorKeyAlphaColor { get; set; }
        public double ColorKeyTolerance { get; set; }
        public int OrientationRefreshRateInMS { get; set; }
        public int ViewportsHorizontalOffset { get; set; }
        public double HydraPositionScaleFactor { get; set; }
        public double HydraPitchOffset { get; set; }
        public double PsMovePositionScaleFactor { get; set; }
        public double KinectPositionScaleFactor { get; set; }
        public double VrpnPositionScaleFactor { get; set; }
        public string VrpnTrackerAddress { get; set; }
        public string VrpnButtonAddress { get; set; }
        public bool PositionalAudio { get; set; }
        public bool EvrRendering { get; set; }

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
