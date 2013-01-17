using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WPFMediaKit.DirectShow.Interop;

namespace VrPlayer.Models.Media
{
    public interface IAudioEngine
    {
        void PlayBuffer(byte[] buffer);
        void SetAudioFormat(WaveFormatEx format);
    }
}
