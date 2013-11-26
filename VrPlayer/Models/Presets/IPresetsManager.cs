namespace VrPlayer.Models.Presets
{
    public interface IPresetsManager
    {
        void SaveMediaToFile(string fileName);
        void LoadFromUri(string path);
    }
}