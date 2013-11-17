namespace VrPlayer.Models.Presets
{
    public interface IPresetsManager
    {
        void SaveMediaToFile(string fileName);
        /*void Reset();
        void SaveAllToSettings();
        void SaveDeviceToFile(string fileName);
        void SaveConfigToFile(string fileName);
        void SaveAllToFile(string fileName);
        void LoadFromSettings();
        void LoadFromFile(string path);
        void LoadFromMetadata(string path);
        void Load(string json);*/
    }
}