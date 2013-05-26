using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.Barrel
{
    [Export(typeof(DistortionBase))]
    public class BarrelEffect : DistortionBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(BarrelEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public BarrelEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Distortions.Barrel",
                "BarrelEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
         }        
    }
}