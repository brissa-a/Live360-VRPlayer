using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMediaKit.DirectShow.MediaPlayers;
using System.Runtime.InteropServices;
using WPFMediaKit.DirectShow.Interop;
using System.IO;

namespace VrPlayer.Helpers.Media
{
    [ComImport, Guid("04FE9017-F873-410E-871E-AB91661A4EF7")]
    internal class FFDShow
    {

    }
    /// <summary>
    /// The MediaUriPlayer plays media files from a given Uri.
    /// </summary>
    public class GraphPlayer : MediaSeekingPlayer, ISampleGrabberCB
    {
        /// <summary>
        /// The name of the default audio render.  This is the
        /// same on all versions of windows
        /// </summary>
        private const string DEFAULT_AUDIO_RENDERER_NAME = "Default DirectSound Device";

        /// <summary>
        /// Set the default audio renderer property backing
        /// </summary>
        private string m_audioRenderer = DEFAULT_AUDIO_RENDERER_NAME;

        /// <summary>
        /// Used to view the graph in graphedit
        /// </summary>
        private DsROTEntry m_dsRotEntry;

        /// <summary>
        /// The DirectShow graph interface.  In this example
        /// We keep reference to this so we can dispose 
        /// of it later.
        /// </summary>
        private IGraphBuilder m_graph;

        /// <summary>
        /// The media Uri
        /// </summary>
        private Uri m_sourceUri;

        /// <summary>
        /// Gets or sets the Uri source of the media
        /// </summary>
        public Uri Source
        {
            get
            {
                VerifyAccess();
                return m_sourceUri;
            }
            set
            {
                VerifyAccess();
                m_sourceUri = value;

                OpenSource();
            }
        }

        /// <summary>
        /// The renderer type to use when
        /// rendering video
        /// </summary>
        public VideoRendererType VideoRenderer
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the audio renderer device
        /// </summary>
        public string AudioRenderer
        {
            get
            {
                VerifyAccess();
                return m_audioRenderer;
            }
            set
            {
                VerifyAccess();

                if (string.IsNullOrEmpty(value))
                {
                    value = DEFAULT_AUDIO_RENDERER_NAME;
                }

                m_audioRenderer = value;
            }
        }

        /// <summary>
        /// Gets or sets if the media should play in loop
        /// or if it should just stop when the media is complete
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// Is ran everytime a new media event occurs on the graph
        /// </summary>
        /// <param name="code">The Event code that occured</param>
        /// <param name="lparam1">The first event parameter sent by the graph</param>
        /// <param name="lparam2">The second event parameter sent by the graph</param>
        protected override void OnMediaEvent(EventCode code, IntPtr lparam1, IntPtr lparam2)
        {
            if (Loop)
            {
                switch (code)
                {
                    case EventCode.Complete:
                        MediaPosition = 0;
                        break;
                }
            }
            else
                /* Only run the base when we don't loop
                 * otherwise the default behavior is to
                 * fire a media ended event */
                base.OnMediaEvent(code, lparam1, lparam2);
        }

        /// <summary>
        /// Opens the media by initializing the DirectShow graph
        /// </summary>
        protected virtual void OpenSource()
        {
            /* Make sure we clean up any remaining mess */
            FreeResources();

            if (m_sourceUri == null)
                return;

            string fileSource = m_sourceUri.OriginalString;

            if (string.IsNullOrEmpty(fileSource))
                return;

            try
            {
                CreateGraph(fileSource);
            }
            catch (Exception ex)
            {
                /* This exection will happen usually if the media does
                 * not exist or could not open due to not having the
                 * proper filters installed */
                FreeResources();

                /* Fire our failed event */
                InvokeMediaFailed(new MediaFailedEventArgs(ex.Message, ex));
            }

            InvokeMediaOpened();
        }

        private void CreateGraph(string fileSource)
        {
            /* Creates the GraphBuilder COM object */
            m_graph = new FilterGraphNoThread() as IGraphBuilder;

            if (m_graph == null)
                throw new Exception("Could not create a graph");

            /* Add our prefered audio renderer */
            //InsertAudioRenderer(AudioRenderer);
           
            IBaseFilter renderer = CreateVideoRenderer(VideoRenderer, m_graph, 2);
            
            var filterGraph = m_graph as IFilterGraph2;

            if (filterGraph == null)
                throw new Exception("Could not QueryInterface for the IFilterGraph2");

            IBaseFilter sourceFilter;

            /* Have DirectShow find the correct source filter for the Uri */
            int hr = filterGraph.AddSourceFilter(fileSource, fileSource, out sourceFilter);
            DsError.ThrowExceptionForHR(hr);

            /* We will want to enum all the pins on the source filter */
            IEnumPins pinEnum;

            hr = sourceFilter.EnumPins(out pinEnum);
            DsError.ThrowExceptionForHR(hr);

            IntPtr fetched = IntPtr.Zero;
            IPin[] pins = { null };

            /* Counter for how many pins successfully rendered */
            int pinsRendered = 0;

            if (VideoRenderer == VideoRendererType.VideoMixingRenderer9)
            {
                var mixer = renderer as IVMRMixerControl9;

                if (mixer != null)
                {
                    VMR9MixerPrefs dwPrefs;
                    mixer.GetMixingPrefs(out dwPrefs);
                    dwPrefs &= ~VMR9MixerPrefs.RenderTargetMask;
                    dwPrefs |= VMR9MixerPrefs.RenderTargetRGB;
                    //mixer.SetMixingPrefs(dwPrefs);
                }
            }

            /* Test using FFDShow Video Decoder Filter
            var ffdshow = new FFDShow() as IBaseFilter;

            if (ffdshow != null)
                m_graph.AddFilter(ffdshow, "ffdshow");
            */

            /* Loop over each pin of the source filter */
            while (pinEnum.Next(pins.Length, pins, fetched) == 0)
            {
                if (filterGraph.RenderEx(pins[0],
                                            AMRenderExFlags.RenderToExistingRenderers,
                                            IntPtr.Zero) >= 0)
                    pinsRendered++;

                Marshal.ReleaseComObject(pins[0]);
            }

            Marshal.ReleaseComObject(pinEnum);
            Marshal.ReleaseComObject(sourceFilter);

            if (pinsRendered == 0)
                throw new Exception("Could not render any streams from the source Uri");

            /* Adds the GB to the ROT so we can view
                * it in graphedit */
            m_dsRotEntry = new DsROTEntry(m_graph);
                
            /* Configure the graph in the base class */
            SetupFilterGraph(m_graph);

            HasVideo = true;
            /* Sets the NaturalVideoWidth/Height */
            //SetNativePixelSizes(renderer);

            RenderAudio();
            
            //Debug:
            GraphUtil.SaveGraphFile(m_graph, @"c:\temp\graph.grf");
        }

        /// <summary>
        /// Inserts the audio renderer by the name of
        /// the audio renderer that is passed
        /// </summary>
        protected virtual void InsertAudioRenderer(string audioDeviceName)
        {
            if (m_graph == null)
                return;

            AddFilterByName(m_graph, FilterCategory.AudioRendererCategory, audioDeviceName);
        }

        /// <summary>
        /// Inserts the audio stream renderer
        /// </summary>
        protected virtual void InsertDirectSoundRenderer()
        {
            int hr;

            if (m_graph == null)
                return;

            var devices = DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);

            var deviceList = (from d in devices
                              where d.Name == DEFAULT_AUDIO_RENDERER_NAME
                              select d);
            DsDevice device = null;
            if (deviceList.Count() > 0)
                device = deviceList.Take(1).Single();

            foreach (var item in deviceList)
            {
                if (item != device)
                    item.Dispose();
            }

            if (m_graph == null)
                throw new ArgumentNullException("graphBuilder");

            var filterGraph = m_graph as IFilterGraph2;

            if (filterGraph == null)
                return;

            IBaseFilter filter = null;
            if (device != null)
            {
                hr = filterGraph.AddSourceFilterForMoniker(device.Mon, null, device.Name, out filter);
                DsError.ThrowExceptionForHR(hr);
            }

            //IntPtr test;
            //bool b;
            //hr = ((IAMDirectSound)filter).GetFocusWindow(out test, out b);
            //DsError.ThrowExceptionForHR(hr);
        }

        protected virtual void RenderAudio()
        { 
            int hr;

            IEnumFilters enumFilters;
            hr = m_graph.EnumFilters(out enumFilters);
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter[] filters = new IBaseFilter[1];
            IntPtr fetched = new IntPtr();

            while (enumFilters.Next(1, filters, fetched) == 0)
            {
                IBaseFilter filter = filters[0] as IBaseFilter;
                IPin unconnectedPin = DsFindPin.ByConnectionStatus((IBaseFilter)filter, PinConnectedStatus.Unconnected, 0);
                if (unconnectedPin != null)
                { 
                    PinDirection direction;
                    hr = unconnectedPin.QueryDirection(out direction);
                    DsError.ThrowExceptionForHR(hr);

                    if (direction == PinDirection.Output)
                    {
                        hr = m_graph.Render(unconnectedPin);
                        DsError.ThrowExceptionForHR(hr);

                        InsertAudioStreamRenderer();
                    }
                }
            }
        }

        private int avgBytesPerSec = 0;
        private int audioBlockAlign = 0;
        private int audioChannels = 0;
        private int audioSamplesPerSec = 0;
        private int audioBitsPerSample = 0;
        
        /// <summary>
        /// Inserts the audio stream renderer
        /// </summary>
        protected virtual void InsertAudioStreamRenderer()
        {
            if (m_graph == null)
                return;

            int hr;

            //Get directsound filter
            IBaseFilter directSoundFilter;
            hr = m_graph.FindFilterByName(DEFAULT_AUDIO_RENDERER_NAME, out directSoundFilter);
            DsError.ThrowExceptionForHR(hr);

            IPin rendererPinIn = DsFindPin.ByConnectionStatus(directSoundFilter, PinConnectedStatus.Connected, 0);

            if (rendererPinIn != null)
            {
                IPin audioPinOut;
                hr = rendererPinIn.ConnectedTo(out audioPinOut);
                DsError.ThrowExceptionForHR(hr);

                if (audioPinOut != null)
                {
                    //Disconect audio decoder to directsound renderer
                    hr = audioPinOut.Disconnect();
                    DsError.ThrowExceptionForHR(hr);

                    hr = m_graph.RemoveFilter(directSoundFilter);
                    DsError.ThrowExceptionForHR(hr);

                    //Add Sample Grabber
                    ISampleGrabber sampleGrabber = new SampleGrabber() as ISampleGrabber;
                    hr = sampleGrabber.SetCallback(this, 1);
                    DsError.ThrowExceptionForHR(hr);
                    //////////////////
                    AMMediaType media;
                    media = new AMMediaType();
                    media.majorType = MediaType.Audio;
                    media.subType = MediaSubType.PCM;
                    media.formatType = FormatType.WaveEx;
                    hr = sampleGrabber.SetMediaType(media);
                    DsError.ThrowExceptionForHR(hr);
                    //////////////////
                    /*
                    if ((media.formatType != FormatType.WaveEx) || (media.formatPtr == IntPtr.Zero))
                    {
                        throw new NotSupportedException("Unknown Grabber Media Format");
                    }
                    WaveFormatEx wav = new WaveFormatEx();
                    wav = (WaveFormatEx)Marshal.PtrToStructure(media.formatPtr, typeof(WaveFormatEx));
                    this.avgBytesPerSec = wav.nAvgBytesPerSec;
                    this.audioBlockAlign = wav.nBlockAlign;
                    this.audioChannels = wav.nChannels;
                    this.audioSamplesPerSec = wav.nSamplesPerSec;
                    this.audioBitsPerSample = wav.wBitsPerSample;
                    Marshal.FreeCoTaskMem(media.formatPtr);
                    media.formatPtr = IntPtr.Zero;
                    */
                    //////////////////
                    IPin sampleGrabberPinIn = DsFindPin.ByDirection((IBaseFilter)sampleGrabber, PinDirection.Input, 0);
                    IPin sampleGrabberPinOut = DsFindPin.ByDirection((IBaseFilter)sampleGrabber, PinDirection.Output, 0);
                    hr = m_graph.AddFilter((IBaseFilter)sampleGrabber, "SampleGrabber");
                    DsError.ThrowExceptionForHR(hr);

                    PinInfo pinInfo;
                    hr = audioPinOut.QueryPinInfo(out pinInfo);
                    DsError.ThrowExceptionForHR(hr);

                    FilterInfo filterInfo;
                    hr = pinInfo.filter.QueryFilterInfo(out filterInfo);
                    DsError.ThrowExceptionForHR(hr);

                    hr = m_graph.Connect(audioPinOut, sampleGrabberPinIn);
                    DsError.ThrowExceptionForHR(hr);

                    //Add null renderer
                    NullRenderer nullRenderer = new NullRenderer();
                    hr = m_graph.AddFilter((IBaseFilter)nullRenderer, "NullRenderer");
                    DsError.ThrowExceptionForHR(hr);

                    IPin nullRendererPinIn = DsFindPin.ByDirection((IBaseFilter)nullRenderer, PinDirection.Input, 0);
                    hr = m_graph.Connect(sampleGrabberPinOut, nullRendererPinIn);
                    DsError.ThrowExceptionForHR(hr);
                }
            }
        }

        /// <summary>
        /// Frees all unmanaged memory and resets the object back
        /// to its initial state
        /// </summary>
        protected override void FreeResources()
        {
#if DEBUG
            /* Remove us from the ROT */
            if (m_dsRotEntry != null)
            {
                m_dsRotEntry.Dispose();
                m_dsRotEntry = null;
            }
#endif

            /* We run the StopInternal() to avoid any 
             * Dispatcher VeryifyAccess() issues because
             * this may be called from the GC */
            StopInternal();

            /* Let's clean up the base 
             * class's stuff first */
            base.FreeResources();

            if (m_graph != null)
            {
                Marshal.ReleaseComObject(m_graph);
                m_graph = null;

                /* Only run the media closed if we have an
                 * initialized filter graph */
                InvokeMediaClosed(new EventArgs());
            }
        }

        #region Grabber

        public int SampleCB(double sampleTime, IMediaSample mediaSample)
        {
            Console.WriteLine("SampleCB Callback");
            Console.WriteLine(mediaSample.IsSyncPoint() + " ");

            /* other code */
            Marshal.ReleaseComObject(mediaSample);
            return 0;
        }

        public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            byte[] buffer = new byte[BufferLen];
            Marshal.Copy(pBuffer, buffer, 0, BufferLen);
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(@"C:\temp\directshowoutput.pcm", FileMode.Append)))
            {
                binWriter.Write(buffer);
            }
            return 0;
        }

        #endregion
    }
}
