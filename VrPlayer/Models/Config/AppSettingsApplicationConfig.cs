using System.Configuration;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Helpers;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : ApplicationConfigBase
    {
        public AppSettingsApplicationConfig()
        {
            DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
            SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
            BarrelFactor = ParseDouble(ConfigurationManager.AppSettings["BarrelFactor"]);
            PincushionFactor = ParseDouble(ConfigurationManager.AppSettings["PincushionFactor"]);
            CameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
            MouseSensitivity = int.Parse(ConfigurationManager.AppSettings["MouseSensitivity"]);
            DepthMapMaxOffset = ParseDouble(ConfigurationManager.AppSettings["DepthMapMaxOffset"]);
            ColorKeyAlphaColor = ParseColor(ConfigurationManager.AppSettings["ColorKeyAlphaColor"]);
            ColorKeyTolerance = ParseDouble(ConfigurationManager.AppSettings["ColorKeyTolerance"]);
            OrientationRefreshRateInMS = int.Parse(ConfigurationManager.AppSettings["OrientationRefreshRateInMS"]);
            ViewportsHorizontalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsHorizontalOffset"]);
            HydraPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["HydraPositionScaleFactor"]);
            HydraRotationOffset = ParseVector3D(ConfigurationManager.AppSettings["HydraRotationOffset"]);
            PsMovePositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["PsMovePositionScaleFactor"]);
            KinectPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["KinectPositionScaleFactor"]);
            VrpnPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["VrpnPositionScaleFactor"]);
            VrpnTrackerAddress = ConfigurationManager.AppSettings["VrpnTrackerAddress"];
            VrpnButtonAddress = ConfigurationManager.AppSettings["VrpnButtonAddress"];
            VrpnRotationOffset = ParseVector3D(ConfigurationManager.AppSettings["VrpnRotationOffset"]);
            LeapPositionScaleFactor = ParseDouble(ConfigurationManager.AppSettings["LeapPositionScaleFactor"]);
            LeapRotationFactor = ParseDouble(ConfigurationManager.AppSettings["LeapRotationFactor"]);
            PositionalAudio = bool.Parse(ConfigurationManager.AppSettings["PositionalAudio"]);
            EvrRendering = bool.Parse(ConfigurationManager.AppSettings["EvrRendering"]);
            MetaDataReadOnLoad = bool.Parse(ConfigurationManager.AppSettings["MetaDataReadOnLoad"]);
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
            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(value, NumberStyles.Any, ci);
        }

        private Color ParseColor(string value)
        {
            var color = ColorConverter.ConvertFromString(value);
            if (color == null) 
                return new Color();

            return (Color)color; 
        }

        private Vector3D ParseVector3D(string value)
        {
            var coords = value.Split(',');

            if(coords.Length != 3)
                return new Vector3D();

            var pitch = ParseDouble(coords[0]);
            var yaw = ParseDouble(coords[1]);
            var roll = ParseDouble(coords[2]);
            return new Vector3D(pitch,yaw,roll);
        }

        #endregion
    }
}
