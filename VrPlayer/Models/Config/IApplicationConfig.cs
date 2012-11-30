namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig
    {
        string DefaultMediaFile { get; }
        int CustomBarrelWrapFactor { get; }
        int CameraFieldOfView { get; }
        int MouseSensitivity { get; }
    }
}