using System;

using SharpDX;
using SharpDX.XAudio2;
using SharpDX.X3DAudio;
using SharpDX.Multimedia;

using WPFMediaKit.DirectShow.Interop;
using System.Threading;

namespace VrPlayer.Models.Media
{
    public class X3DAudioEngine: IAudioEngine
    {
        XAudio2 _xaudio2;
        WaveFormat _format;
        SourceVoice _sourceVoice;
        DataStream _dataStream; 

        public X3DAudioEngine()
        {
            _xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(_xaudio2);
        }

        public void PlayBuffer(byte[] buffer)
        {
            _dataStream = new DataStream(buffer.Length, true, true);
            _dataStream.Write(buffer, 0, buffer.Length);
            _dataStream.Position = 0;

            var audioBuffer = new AudioBuffer
            {
                Stream = _dataStream,
                Flags = BufferFlags.EndOfStream,
                AudioBytes = buffer.Length
            };
            _sourceVoice.SubmitSourceBuffer(audioBuffer, null);
            if (!_isPlaying)
                Play();
        }

        public void SetAudioFormat(WaveFormatEx format)
        {
            _format = new WaveFormat(format.nSamplesPerSec, format.wBitsPerSample, format.nChannels);
            _sourceVoice = new SourceVoice(_xaudio2, _format);

            /*
            // Instantiate X3DAudio
            var deviceFormat = _xaudio2.GetDeviceDetails(0).OutputFormat;
            var x3dAudio = new X3DAudio(deviceFormat.ChannelMask);

            var emitter = new Emitter
            {
                ChannelCount = 1,
                CurveDistanceScaler = float.MinValue,
                OrientFront = new Vector3(0, 0, 1),
                OrientTop = new Vector3(0, 1, 0),
                Position = new Vector3(0, 0, 0),
                Velocity = new Vector3(0, 0, 0)
            };

            var listener = new Listener
            {
                OrientFront = new Vector3(0, 0, 1),
                OrientTop = new Vector3(0, 1, 0),
                Position = new Vector3(0, 0, 0),
                Velocity = new Vector3(0, 0, 0)
            };
                
            Console.WriteLine("Play a sound rotating around the listener");
            for (int i = 0; i < 1200; i++)
            {
                // Rotates the emitter
                var rotateEmitter = Matrix.RotationY(i / 5.0f);
                var newPosition = Vector3.Transform(new Vector3(0, 0, 1000), rotateEmitter);
                var newPositionVector3 = new Vector3(newPosition.X, newPosition.Y, newPosition.Z);
                emitter.Velocity = (newPositionVector3 - emitter.Position) / 0.05f;
                emitter.Position = newPositionVector3;

                // Calculate X3DAudio settings
                var dspSettings = x3dAudio.Calculate(listener, emitter, CalculateFlags.Matrix | CalculateFlags.Doppler, 1, deviceFormat.Channels);

                // Modify XAudio2 source voice settings
                sourceVoice.SetOutputMatrix(1, deviceFormat.Channels, dspSettings.MatrixCoefficients);
                sourceVoice.SetFrequencyRatio(dspSettings.DopplerFactor);
            }
            */
        }

        private bool _isPlaying;
        public void Play()
        {
            _isPlaying = true;
            Thread t = new Thread(PlayMethod);
            t.Start();
        }

        private void PlayMethod()
        {
            _sourceVoice.Start();
            while (_sourceVoice.State.BuffersQueued > 0)
            {
                Thread.Sleep(10);
            }
            _sourceVoice.Dispose();
        }
    }
}
