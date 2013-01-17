using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMediaKit.DirectShow.Controls;
using System.Windows;
using WPFMediaKit.DirectShow.MediaPlayers;

namespace VrPlayer.Models.Media
{
    /// <summary>
    /// The MediaUriElement is a WPF control that plays media of a given
    /// Uri. The Uri can be a file path or a Url to media.  The MediaUriElement
    /// inherits from the MediaSeekingElement, so where available, seeking is
    /// also supported.
    /// </summary>
    public class GraphPlayerElement : MediaSeekingElement
    {
        private IAudioEngine _audioEngine;

        /// <summary>
        /// The current GraphPlayer
        /// </summary>
        protected GraphPlayer GraphPlayer
        {
            get
            {
                return MediaPlayerBase as GraphPlayer;
            }
        }

        #region VideoRenderer

        public static readonly DependencyProperty VideoRendererProperty =
            DependencyProperty.Register("VideoRenderer", typeof(VideoRendererType), typeof(GraphPlayerElement),
                new FrameworkPropertyMetadata(VideoRendererType.VideoMixingRenderer9,
                    new PropertyChangedCallback(OnVideoRendererChanged)));

        public VideoRendererType VideoRenderer
        {
            get { return (VideoRendererType)GetValue(VideoRendererProperty); }
            set { SetValue(VideoRendererProperty, value); }
        }

        private static void OnVideoRendererChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GraphPlayerElement)d).OnVideoRendererChanged(e);
        }

        protected virtual void OnVideoRendererChanged(DependencyPropertyChangedEventArgs e)
        {
            if (HasInitialized)
                PlayerSetVideoRenderer();
        }

        private void PlayerSetVideoRenderer()
        {
            var videoRendererType = VideoRenderer;
            GraphPlayer.Dispatcher.BeginInvoke((Action)delegate
            {
                GraphPlayer.VideoRenderer = videoRendererType;
            });
        }

        #endregion
        
        #region Source

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(GraphPlayerElement),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// The Uri source to the media.  This can be a file path or a
        /// URL source
        /// </summary>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GraphPlayerElement)d).OnSourceChanged(e);
        }

        protected void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if(HasInitialized)
                PlayerSetSource();
        }

        private void PlayerSetSource()
        {
            var source = Source;
            var rendererType = VideoRenderer;

            MediaPlayerBase.Dispatcher.BeginInvoke((Action) delegate
            {
                /* Set the renderer type */
                GraphPlayer.VideoRenderer = rendererType;

                /* Set the source type */
                GraphPlayer.Source = source;

                Dispatcher.BeginInvoke((Action) delegate
                {
                    if (IsLoaded)
                        ExecuteMediaState(LoadedBehavior);
                    //else
                    //    ExecuteMediaState(UnloadedBehavior);
                });
            });
        }
        #endregion

        #region Loop

        public static readonly DependencyProperty LoopProperty =
            DependencyProperty.Register("Loop", typeof(bool), typeof(GraphPlayerElement),
                new FrameworkPropertyMetadata(false,
                    new PropertyChangedCallback(OnLoopChanged)));

        /// <summary>
        /// Gets or sets whether the media should return to the begining
        /// once the end has reached
        /// </summary>
        public bool Loop
        {
            get { return (bool)GetValue(LoopProperty); }
            set { SetValue(LoopProperty, value); }
        }

        private static void OnLoopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GraphPlayerElement)d).OnLoopChanged(e);
        }

        protected virtual void OnLoopChanged(DependencyPropertyChangedEventArgs e)
        {
            if (HasInitialized)
                PlayerSetLoop();
        }

        private void PlayerSetLoop()
        {
            var loop = Loop;
            MediaPlayerBase.Dispatcher.BeginInvoke((Action)delegate
            {
                GraphPlayer.Loop = loop;
            });
        }
        #endregion

        public override void EndInit()
        {
            PlayerSetVideoRenderer();
            PlayerSetLoop();
            PlayerSetSource();
            base.EndInit();
        }

        /// <summary>
        /// The Play method is overrided so we can
        /// set the source to the media
        /// </summary>
        public override void Play()
        {
            EnsurePlayerThread();
            base.Play();
        }

        /// <summary>
        /// The Pause method is overrided so we can
        /// set the source to the media
        /// </summary>
        public override void Pause()
        {
            EnsurePlayerThread();

            base.Pause();
        }

        /// <summary>
        /// Gets the instance of the media player to initialize
        /// our base classes with
        /// </summary>
        protected override MediaPlayerBase OnRequestMediaPlayer()
        {
            //Todo: extract audio  engine creation
            IAudioEngine audioEngine = new IrrKlangAudioEngine();
            var player = new GraphPlayer(audioEngine);
            return player;
        }
    }
}
