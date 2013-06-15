namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; set; }
        int CameraFieldOfView { get; set; }
        int ViewportsHorizontalOffset { get; set; }
        int ViewportsVerticalOffset { get; set; }
        string SamplesFolder { get; set; }
        bool MetaDataReadOnLoad { get; set; }

        bool PositionalAudio { get; set; }
        bool EvrRendering { get; set; }

        string DefaultEffect { get; set; }
        string DefaultDistortion { get; set; }
        string DefaultProjection { get; set; }
        string DefaultTracker { get; set; }
        string DefaultStabilizer { get; set; }
    }
}