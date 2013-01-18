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
    public class X3DAudioEngine: IAudioEngine
    {
        private XAudio2 _xaudio2;
        private X3DAudio _x3dAudio;
        private WaveFormatExtensible _deviceFormat;
        private WaveFormat _format;
        private bool _isPlaying;
 
        private List<SourceVoice> _voices;
        private List<Emitter> _emitters;
        private List<Vector3> _emmiterPositions;

        private Vector3 _position;
        public Vector3 Position
        {
            get{ return _position; }
            set
            {
                _position = value;
            }
        }

        private Vector3 _orientation;
        public Vector3 Orientation
        {
            get { return _orientation; }
            set 
            {
                _orientation = value;
            }
        }

        public X3DAudioEngine()
        {
            _position = new Vector3(0, 0, 0);
            _orientation = new Vector3(0, 0, 1);
            _voices = new List<SourceVoice>();
            _emitters = new List<Emitter>();

            _xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(_xaudio2);

            _deviceFormat = _xaudio2.GetDeviceDetails(0).OutputFormat;
            _x3dAudio = new X3DAudio(_deviceFormat.ChannelMask);
            
        	_emmiterPositions = new List<Vector3>();
	        _emmiterPositions.Add(new Vector3(-1, 0, 1));	//Front Left	SPEAKER_FRONT_LEFT		0
	        _emmiterPositions.Add(new Vector3(1, 0, 1));	//Front Right	SPEAKER_FRONT_RIGHT		1
	        _emmiterPositions.Add(new Vector3(0, 0, 1));	//Front Center	SPEAKER_FRONT_CENTER	2
	        _emmiterPositions.Add(new Vector3(0, 0, 0)); 	//Sub? 									3
	        _emmiterPositions.Add(new Vector3(-1, 0, -1));  //Back Left		SPEAKER_BACK_LEFT		4
	        _emmiterPositions.Add(new Vector3(1, 0, -1));	//Back Right	SPEAKER_BACK_RIGHT		5
        }

        public void PlayBuffer(byte[] buffer)
        {
            /*
            int numberOfSamples = buffer.Length / _format.BlockAlign;
            for (int sampleIndex = 0; sampleIndex < numberOfSamples; sampleIndex++)
            {
                int bufferSize = numberOfSamples / _format.Channels;

                for (int channelIndex = 0; channelIndex < _format.Channels; channelIndex++)
                {    
                    var dataStream = new DataStream(bufferSize, true, true);

                    byte[] channelBuffer = new byte[bufferSize];
                    Array.Copy(buffer, bufferSize * channelIndex, channelBuffer, 0, bufferSize);
                    dataStream.Write(channelBuffer, 0, channelBuffer.Length);
                    dataStream.Position = 0;

                    var audioBuffer = new AudioBuffer
                    {
                        Stream = dataStream,
                        AudioBytes = bufferSize,
                        Flags = BufferFlags.EndOfStream
                    };

                    _voices[channelIndex].SubmitSourceBuffer(audioBuffer, null);
                }
            }
            */

            var dataStream = new DataStream(buffer.Length, true, true);

            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Position = 0;

            var audioBuffer = new AudioBuffer
            {
                Stream = dataStream,
                AudioBytes = buffer.Length,
                Flags = BufferFlags.EndOfStream
            };

            _voices[0].SubmitSourceBuffer(audioBuffer, null);

            UpdateMatrices();
            
            if (!_isPlaying)
                Play();
        }

        public void SetAudioFormat(WaveFormatEx format)
        {
            _format = new WaveFormat(format.nSamplesPerSec, format.wBitsPerSample, format.nChannels);
            
            for(int i = 0; i < _format.Channels; i++)
	        {
                var channelFormat = new WaveFormat(_format.SampleRate, _format.Channels);
		        SourceVoice voice = new SourceVoice(_xaudio2, channelFormat);
		        _voices.Add(voice);

		        var emitter = new Emitter
		        {
			        ChannelCount = 1,
			        CurveDistanceScaler = float.MinValue,
			        OrientFront = new Vector3(0, 0, 1),
			        OrientTop = new Vector3(0, 1, 0),
			        Position = _emmiterPositions[i],
			        Velocity = new Vector3(0, 0, 0)
		        };
                _emitters.Add(emitter);
	        }
        }

        public void Play()
        {
            _isPlaying = true;
            Thread t = new Thread(PlayMethod);
            t.Start();
        }

        private void PlayMethod()
        {
            _voices[0].Start();
            
            while (_voices[0].State.BuffersQueued > 0)
            {
                Thread.Sleep(10);
            }
            
            _voices[0].Dispose();
        }

        private void UpdateMatrices()
        {
 	        var listener = new Listener
	        {
	           OrientFront = Orientation,
	           OrientTop = new Vector3(0, 1, 0),
               Position = Position,
	           Velocity = new Vector3(0, 0, 0)
	        };

            for (int channelIndex = 0; channelIndex < _format.Channels; channelIndex++)
	        {
                var dspSettings = _x3dAudio.Calculate(listener, _emitters[channelIndex], CalculateFlags.Matrix, _format.Channels, _deviceFormat.Channels);
                _voices[channelIndex].SetOutputMatrix(_format.Channels, _deviceFormat.Channels, dspSettings.MatrixCoefficients);
	        }
        }
    }
}