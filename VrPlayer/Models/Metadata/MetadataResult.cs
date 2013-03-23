//TODO: Code generation for properties.xml using attributes
namespace VrPlayer.Models.Metadata
{
    public class MetadataResult
    {
        public string FormatType { get; set; }
        public string ProjectionType { get; set; }

        public string[] Effects { get; set; }
        public int CameraFieldOfView { get; set; }
        public bool PositionalAudio { get; set; }
    }
}
