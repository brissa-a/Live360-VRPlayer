using System.Windows.Input;

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

        string DefaultMedia { get; set; }
        string DefaultEffect { get; set; }
        string DefaultDistortion { get; set; }
        string DefaultProjection { get; set; }
        string DefaultTracker { get; set; }
        string DefaultStabilizer { get; set; }

        //Todo: extract
        Key MoveLeft { get; set; }
        Key MoveRight { get; set; }
        Key MoveForward { get; set; }
        Key MoveBackward { get; set; }
    }
}