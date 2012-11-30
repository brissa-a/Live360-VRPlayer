using System;
using System.Windows;
using System.Windows.Input;

using VrPlayer.ViewModels;

namespace VrPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateMainWindowViewModel();
            }
            catch (Exception exc)
            {
            }
        }

        #region Fullscreen

        private bool _inStateChange;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                WindowState = WindowState.Normal;
            }
            base.OnKeyDown(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Maximized && !_inStateChange)
            {
                _inStateChange = true;
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                _inStateChange = false;
            }
            else if (WindowState == WindowState.Normal && !_inStateChange)
            {
                _inStateChange = true;
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                _inStateChange = false;
            }
            base.OnStateChanged(e);
        }

        #endregion
    }
}
