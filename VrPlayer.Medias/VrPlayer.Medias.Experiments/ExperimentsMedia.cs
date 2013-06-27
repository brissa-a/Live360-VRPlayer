using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VrPlayer.Contracts.Medias;
using Brush = System.Windows.Media.Brush;
using Image = System.Windows.Controls.Image;

namespace VrPlayer.Medias.Experiments
{
    public class ExperimentsMedia : MediaBase
    {
        private readonly Image _media;
        private readonly DispatcherTimer _timer;

        public override FrameworkElement Media
        {
            get
            {
                return _media;
            }
        }

        public static readonly DependencyProperty ProcessProperty =
            DependencyProperty.Register("Process", typeof(Process),
            typeof(ExperimentsMedia), new FrameworkPropertyMetadata(null));
        public Process Process
        {
            get { return (Process)GetValue(ProcessProperty); }
            set { SetValue(ProcessProperty, value); }
        }

        public IEnumerable<Process> Processes
        {
            get
            {
                var processlist = Process.GetProcesses();
                return processlist.Where(process => !String.IsNullOrEmpty(process.MainWindowTitle)).ToList();
            }
        }

        public static Brush CreateBrushFromBitmap(Bitmap bmp)
        {
            return new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
        }

        public ExperimentsMedia()
        {
            _media = new Image();
            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            _timer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (Process == null || Process.MainWindowHandle == IntPtr.Zero) return;

            try
            {
                _media.Source = WindowsGrabber.PrintWindow(Process.MainWindowHandle).ToImageSource();
            }
            catch (Exception exc)
            {
            }
            OnPropertyChanged("Media");
        }

        public override void Load()
        {
            _timer.Start();
        }

        public override void Unload()
        {
            _timer.Stop();
        }
    }

    public static class BitmapExtensions
    {
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
            var hbitmap = bitmap.GetHbitmap();
            try
            {
                var imageSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

                return imageSource;
            }
            finally
            {
                NativeMethods.DeleteObject(hbitmap);
            }
        }
    }

    public static class NativeMethods
    {
        [DllImport("gdi32")]
        public static extern int DeleteObject(IntPtr hObject);
    }
}
