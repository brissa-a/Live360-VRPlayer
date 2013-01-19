using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WPFMediaKit.DirectShow.Interop;
using SharpDX;

namespace VrPlayer.Models.Media
{
    public interface IAudioEngine: IDisposable
    {
        void PlayBuffer(byte[] buffer);
        void SetAudioFormat(WaveFormatEx format);
        Vector3 Position { get; set; }
        Vector3 Orientation { get; set; }
    }
}
