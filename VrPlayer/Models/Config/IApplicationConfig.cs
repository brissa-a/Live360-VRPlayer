namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; }
        string SamplesFolder { get; }
        double CustomPincushionFactor { get; }
        int CameraFieldOfView { get; }
        int MouseSensitivity { get; }
        double DepthMapMaxOffset { get; }
        string ColorKeyAlphaColor { get; }
        double ColorKeyTolerance { get; }
        int OrientationRefreshRateInMS { get; }
        int ViewportLeftRightSpacing { get; }
        int ViewportVerticalDistance { get; }
        int ViewportTopBottomSpacing { get; }
        int ViewportHorizontalDistance { get; }
    }
}