using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.MediaPlayers;

namespace VrPlayer.Models.Media
{
    public class MediaGraphElement: MediaUriElement
    {
        private MediaGraphPlayer _mediaGraphPlayer;
        public MediaGraphPlayer MediaGraphPlayer
        {
            get { return _mediaGraphPlayer; }
        }

        protected override MediaPlayerBase OnRequestMediaPlayer()
        {
            _mediaGraphPlayer = new MediaGraphPlayer();
            return _mediaGraphPlayer;
        }

        protected override void OnUnloadedOverride()
        {
            if (_mediaGraphPlayer != null)
                _mediaGraphPlayer.Dispose();
            base.OnUnloadedOverride();
        }
    }
}
