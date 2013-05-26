namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; set; }
        string SamplesFolder { get; set; }
        int CameraFieldOfView { get; set; }
        int ViewportsHorizontalOffset { get; set; }
        bool PositionalAudio { get; set; }
        bool EvrRendering { get; set; }
        bool MetaDataReadOnLoad { get; set; }
    }
}