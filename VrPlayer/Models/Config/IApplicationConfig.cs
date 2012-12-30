namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; }
        double CustomPincushionFactor { get; }
        int CameraFieldOfView { get; }
        int MouseSensitivity { get; }
        double DepthMapMaxOffset { get; }
        string ColorKeyAlphaColor { get; }
        double ColorKeyTolerance { get; }
        int OrientationRefreshRateInMS { get; }
    }
}