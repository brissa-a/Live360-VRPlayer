using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Trackers
{
    public class MouseTracker : TrackerBase, ITracker
    {
        private FrameworkElement _viewport;
        private double _yaw;
        private double _pitch;

        #region Fields

        private double _mouseSensitivity;
        public double MouseSensitivity
        {
            get
            {
                return _mouseSensitivity;
            }
            set
            {
                _mouseSensitivity = value;
                OnPropertyChanged("MouseSensitivity");
            }
        }

        #endregion

        public MouseTracker()
        {
            //Default values
            _mouseSensitivity = 1;

            //Handlers
            Application.Current.Activated += new EventHandler(Current_Activated);
        }

        void Current_Activated(object sender, EventArgs e)
        {
            if (_viewport == null)
            {
                _viewport = Application.Current.MainWindow;
                _viewport.MouseMove += new MouseEventHandler(mouseZone_MouseMove);
            }
        }

        void mouseZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsActive)
            {
                _viewport.Cursor = Cursors.None;

                Point centerOfViewport = _viewport.PointToScreen(new Point(_viewport.ActualWidth / 2, _viewport.ActualHeight / 2));
                Point relativePos = e.MouseDevice.GetPosition(_viewport);
                Point actualRelativePos = new Point(relativePos.X - _viewport.ActualWidth / 2, _viewport.ActualHeight / 2 - relativePos.Y);
                double dx = actualRelativePos.X;
                double dy = actualRelativePos.Y;
                _yaw += dx;
                _pitch += dy;
                
                // Rotate
                Quaternion = QuaternionHelper.FromEulerAngles(_pitch * _mouseSensitivity * 0.1, _yaw * _mouseSensitivity * 0.1, 0);
                
                // Set mouse position back to the center of the viewport in screen coordinates
                MouseUtilities.SetPosition(centerOfViewport);
            }
            else
            {
                _viewport.Cursor = Cursors.Arrow;
            }
        }

        public override void Dispose()
        {
            //Todo: Remove events from main window
        }
    }
}