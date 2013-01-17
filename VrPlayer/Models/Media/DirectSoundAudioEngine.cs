using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using SharpDX.DirectSound;
using SharpDX.Multimedia;
using System.Windows.Interop;
using SharpDX;

namespace VrPlayer.Models.Media
{
    public class DirectSoundAudioEngine: IAudioEngine
    {
        DirectSound _directSound;
        WaveFormat _format;
        SecondarySoundBuffer _channel1;

        public DirectSoundAudioEngine()
        {
            _directSound = new DirectSound();

            //Todo: Use parameter instead
            //IntPtr windowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            // Set Cooperative Level to PRIORITY (priority level can call the SetFormat and Compact methods)
            //_directSound.SetCooperativeLevel(windowHandle, CooperativeLevel.Priority);

            // Create PrimarySoundBuffer
            var primaryBufferDesc = new SoundBufferDescription();
            primaryBufferDesc.Flags = BufferFlags.PrimaryBuffer;
            primaryBufferDesc.AlgorithmFor3D = Guid.Empty;

            var primarySoundBuffer = new PrimarySoundBuffer(_directSound, primaryBufferDesc);

            // Play the PrimarySound Buffer
            primarySoundBuffer.Play(0, PlayFlags.Looping);


            //////////////////////

            /*
            SoundBufferDescription desc = new SoundBufferDescription();
            desc.Format = _format;
            desc.Flags = BufferFlags.Control3D;

            _channel1 = new SecondarySoundBuffer(_directSound, desc);
            _channel1.Play(0, PlayFlags.None);


            //_soundBuffer = new SoundBuffer(_directSound.NativePointer);
            //SoundBuffer3D soundBuffer3D = new SoundBuffer3D(_soundBuffer.NativePointer);
            //soundBuffer3D.Position = new SharpDX.Vector3(-1, 0, 1);
            */

            ///////////////////////

            
        }

        public void PlayBuffer(byte[] buffer)
        {
            //_channel1.Write<byte>(buffer, 0, LockFlags.None);
        }

        public void SetAudioFormat(WPFMediaKit.DirectShow.Interop.WaveFormatEx format)
        {
            _format = new WaveFormat(format.nSamplesPerSec, format.nChannels);

            /////////////////////3

            // Create SecondarySoundBuffer
            var secondaryBufferDesc = new SoundBufferDescription();
            secondaryBufferDesc.BufferBytes = _format.ConvertLatencyToByteSize(60000);
            secondaryBufferDesc.Format = _format;
            secondaryBufferDesc.Flags = BufferFlags.GetCurrentPosition2 | BufferFlags.ControlPositionNotify | BufferFlags.GlobalFocus |
                                        BufferFlags.ControlVolume | BufferFlags.StickyFocus;
            secondaryBufferDesc.AlgorithmFor3D = Guid.Empty;
            var secondarySoundBuffer = new SecondarySoundBuffer(_directSound, secondaryBufferDesc);

            // Get Capabilties from secondary sound buffer
            var capabilities = secondarySoundBuffer.Capabilities;

            // Lock the buffer
            DataStream dataPart2;
            var dataPart1 = secondarySoundBuffer.Lock(0, capabilities.BufferBytes, LockFlags.EntireBuffer, out dataPart2);

            // Fill the buffer with some sound
            int numberOfSamples = capabilities.BufferBytes / _format.BlockAlign;
            for (int i = 0; i < numberOfSamples; i++)
            {
                double vibrato = Math.Cos(2 * Math.PI * 10.0 * i / _format.SampleRate);
                short value = (short)(Math.Cos(2 * Math.PI * (220.0 + 4.0 * vibrato) * i / _format.SampleRate) * 16384); // Not too loud
                dataPart1.Write(value);
                dataPart1.Write(value);
            }

            // Unlock the buffer
            secondarySoundBuffer.Unlock(dataPart1, dataPart2);

            // Play the song
            secondarySoundBuffer.Play(0, PlayFlags.Looping);
           
        }
    }
}
