namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; set; }
        string SamplesFolder { get; set; }
        double CustomPincushionFactor { get; set; }
        int CameraFieldOfView { get; set; }
        int MouseSensitivity { get; set; }
        double DepthMapMaxOffset { get; set; }
        string ColorKeyAlphaColor { get; set; }
        double ColorKeyTolerance { get; set; }
        int OrientationRefreshRateInMS { get; set; }
        int ViewportLeftRightSpacing { get; set; }
        int ViewportVerticalDistance { get; set; }
        int ViewportTopBottomSpacing { get; set; }
        int ViewportHorizontalDistance { get; set; }
        double HydraPositionScaleFactor { get; set; }
        double PsMovePositionScaleFactor { get; set; }
    }
}