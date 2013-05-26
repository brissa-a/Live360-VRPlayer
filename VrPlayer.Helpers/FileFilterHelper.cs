namespace VrPlayer.Helpers
{
    //Todo: Use generate strings from a data source like a xml file
    public class FileFilterHelper
    {
        public static string GetFilter()
        {
            return "Movies|*.avi;*.flv;*.f4v;*.mp4;*.wmv;*.mpeg;*.mkv|Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
        }
    }
}
