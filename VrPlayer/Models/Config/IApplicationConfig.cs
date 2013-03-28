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
        int ViewportsHorizontalOffset { get; set; }
        double HydraPositionScaleFactor { get; set; }
        double HydraPitchOffset { get; set; }
        double PsMovePositionScaleFactor { get; set; }
        double KinectPositionScaleFactor { get; set; }
        double VrpnPositionScaleFactor { get; set; }
        string VrpnTrackerAddress { get; set; }
        string VrpnButtonAddress { get; set; }
        bool PositionalAudio { get; set; }
        bool EvrRendering { get; set; }
    }
}