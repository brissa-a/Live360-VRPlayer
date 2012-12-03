namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; }
        int CustomPincushionFactor { get; }
        int CameraFieldOfView { get; }
        int MouseSensitivity { get; }
    }
}