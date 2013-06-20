using System;
using System.Windows;
using System.Windows.Input;

namespace VrPlayer.Contracts.Medias
{
    public interface IMedia
    {
        FrameworkElement Media { get; }
        
        ICommand OpenFileCommand { get; }
        ICommand OpenDiscCommand { get; }
        ICommand OpenStreamCommand { get; }
        ICommand OpenDeviceCommand { get; }

        ICommand PlayCommand { get; }
        ICommand PauseCommand { get; }
        ICommand StopCommand { get; }
        ICommand SeekCommand { get; }
        ICommand LoopCommand { get; }

        TimeSpan Position { get; set; }
        TimeSpan Duration { get; set; }
        bool IsPlaying { get; set; }
        bool HasDuration { get; }//Todo:Remove and use duration only
        double Progress { get; }//Todo: Remove and use position on duration
    }
}
