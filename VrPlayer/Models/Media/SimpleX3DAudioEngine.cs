using System;

using SharpDX;
using SharpDX.XAudio2;
using SharpDX.X3DAudio;
using SharpDX.Multimedia;

using WPFMediaKit.DirectShow.Interop;
using System.Threading;
using System.Collections.Generic;

namespace VrPlayer.Models.Media
{
    public class SimpleX3DAudioEngine: IAudioEngine
    {
        private XAudio2 _xaudio2;
        private X3DAudio _x3dAudio;
        private WaveFormatExtensible _deviceFormat;
        private WaveFormatExtensible _format;
        private bool _isPlaying;
 
        private SourceVoice _voice;

        public SimpleX3DAudioEngine()
        {
            _xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(_xaudio2);
            _xaudio2.StartEngine();

            _deviceFormat = _xaudio2.GetDeviceDetails(0).OutputFormat;
            _x3dAudio = new X3DAudio(_deviceFormat.ChannelMask);
        }

        public void PlayBuffer(byte[] buffer)
        {
            var dataStream = new DataStream(buffer.Length, true, true);
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Position = 0;

            var audioBuffer = new AudioBuffer
            {
                Stream = dataStream,
                AudioBytes = buffer.Length,
                Flags = BufferFlags.EndOfStream
            };

            _voice.SubmitSourceBuffer(audioBuffer, null);

            //UpdateMatrices();
            
            if (!_isPlaying)
                Play();
        }

        public void SetAudioFormat(WaveFormatEx format)
        {
            _format = new WaveFormatExtensible(format.nSamplesPerSec, format.wBitsPerSample, format.nChannels);
            _voice = new SourceVoice(_xaudio2, _format);
        }

        public void Play()
        {
            _isPlaying = true;
            _voice.Start();
        }

        private void UpdateMatrices()
        {
 	        var listener = new Listener
	        {
	           OrientFront = new Vector3(0, 0, 1),
	           OrientTop = new Vector3(0, 1, 0),
               Position = new Vector3(0, 0, 0),
	           Velocity = new Vector3(0, 0, 0)
	        };

            var emitter = new Emitter
            {
                ChannelCount = 1,
                CurveDistanceScaler = float.MinValue,
                OrientFront = new Vector3(0, 0, 1),
                OrientTop = new Vector3(0, 1, 0),
                Position = new Vector3(0, 0, 0),
                Velocity = new Vector3(0, 0, 0)
            };

            var dspSettings = _x3dAudio.Calculate(listener, emitter, CalculateFlags.Matrix, _format.Channels, _deviceFormat.Channels);
            _voice.SetOutputMatrix(_format.Channels, _deviceFormat.Channels, dspSettings.MatrixCoefficients);
	        
            //_xaudio2.CommitChanges();
        }

        public void Dispose()
        {
            _voice.Stop();
            if (_xaudio2 != null)
                _xaudio2.Dispose();
        }
    }
}