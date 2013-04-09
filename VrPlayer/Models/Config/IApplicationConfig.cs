namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; set; }
        string SamplesFolder { get; set; }
        double BarrelFactor { get; set; }
        double PincushionFactor { get; set; }
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
        double VrpnPitchOffset { get; set; }
        double LeapPositionScaleFactor { get; set; }
        double LeapRotationFactor { get; set; }
        bool PositionalAudio { get; set; }
        bool EvrRendering { get; set; }
        int DomeSlices { get; set; }
        int DomeStacks { get; set; }
        int SphereSlices { get; set; }
        int SphereStacks { get; set; }
        int CylinderSlices { get; set; }
        int CylinderStacks { get; set; }
    }
}