using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Models.Trackers
{
    public class MouseTracker : TrackerBase, ITracker
    {
        private FrameworkElement _viewport;
        private double _yaw;
        private double _pitch;

        #region Fields

        public static readonly DependencyProperty MouseSensitivityProperty =
            DependencyProperty.Register("MouseSensitivityProperty", typeof(double),
            typeof(MouseTracker), new FrameworkPropertyMetadata(1D));

        public double MouseSensitivity
        {
            get { return (double)GetValue(MouseSensitivityProperty); }
            set { SetValue(MouseSensitivityProperty, value); }
        }

        #endregion

        public MouseTracker()
        {
            //Handlers
            Application.Current.Activated += Current_Activated;

            IsEnabled = true;
        }

        void Current_Activated(object sender, EventArgs e)
        {
            if (_viewport == null)
            {
                _viewport = Application.Current.MainWindow;
                _viewport.MouseMove += mouseZone_MouseMove;
                _viewport.KeyDown += _viewport_KeyDown;
            }
        }

        void _viewport_KeyDown(object sender, KeyEventArgs e)
        {
            const double moveFactor = 0.01;
            var moveVector = new Vector3D();
            switch (e.Key)
            { 
                case Key.Left:
                    moveVector = new Vector3D(-moveFactor,0,0);
                    break;
                case Key.Right:
                    moveVector = new Vector3D(moveFactor, 0, 0);
                    break;
                case Key.Up:
                    moveVector = new Vector3D(0, 0, -moveFactor);
                    break;
                case Key.Down:
                    moveVector = new Vector3D(0, 0, moveFactor);
                    break;
                case Key.PageUp:
                    moveVector = new Vector3D(0, -moveFactor, 0);
                    break;
                case Key.PageDown:
                    moveVector = new Vector3D(0, moveFactor, 0);
                    break;
            }
            var m = Matrix3D.Identity;
            m.Rotate(Rotation);
            m.Translate(moveVector);
            m.Rotate(Rotation);
            Position = Position + new Vector3D(m.OffsetX, m.OffsetY, m.OffsetZ);
        }

        void mouseZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsActive)
            {
                _viewport.Cursor = Cursors.None;

                var centerOfViewport = _viewport.PointToScreen(new Point(_viewport.ActualWidth / 2, _viewport.ActualHeight / 2));
                var relativePos = e.MouseDevice.GetPosition(_viewport);
                var actualRelativePos = new Point(relativePos.X - _viewport.ActualWidth / 2, _viewport.ActualHeight / 2 - relativePos.Y);
                var dx = actualRelativePos.X;
                var dy = actualRelativePos.Y;
                _yaw += dx;
                _pitch += dy;
                
                // Rotate
                Rotation = QuaternionHelper.QuaternionFromEulerAngles(_pitch * MouseSensitivity * 0.1, _yaw * MouseSensitivity * 0.1, 0);
                
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