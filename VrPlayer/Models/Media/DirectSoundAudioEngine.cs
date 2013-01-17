using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VrPlayer.Models.Media
{
    public class DirectSoundAudioEngine: IAudioEngine
    {
        public void PlayBuffer(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public void SetAudioFormat(WPFMediaKit.DirectShow.Interop.WaveFormatEx format)
        {
            throw new NotImplementedException();
        }
    }
}
