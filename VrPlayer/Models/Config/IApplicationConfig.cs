namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; }
        double CustomPincushionFactor { get; }
        int CameraFieldOfView { get; }
        int MouseSensitivity { get; }
    }
}