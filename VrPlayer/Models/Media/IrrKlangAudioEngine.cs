using System;
using System.Collections.Generic;
using System.Linq;

using IrrKlang;
using System.IO;
using WPFMediaKit.DirectShow.Interop;

namespace VrPlayer.Models.Media
{
    public class IrrKlangAudioEngine: IAudioEngine
    {
        ISoundEngine _engine;
        AudioFormat _format = new AudioFormat(); 
        Stream _stream;

        public IrrKlangAudioEngine()
        {
            _engine = new ISoundEngine();
        }

        public void PlayBuffer(byte[] buffer)
        {
            ISoundSource source = _engine.AddSoundSourceFromPCMData(buffer, "Test", _format);
            _engine.Play2D(source, false, false, false);
            //_engine.Play3D(source, -1, 0, 1, false, false, false);
        }

        public void SetAudioFormat(WaveFormatEx format)
        {
            _format.ChannelCount = format.nChannels;
            _format.SampleRate = format.nSamplesPerSec;
            if (format.wBitsPerSample == 8)
            {
                _format.Format = SampleFormat.Unsigned8Bit;
            }
            else 
            {
                _format.Format = SampleFormat.Signed16Bit;
            }
        }
    }
}
