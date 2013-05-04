using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VrPlayer.Distortions.Barrel
{
    [Export(typeof(ShaderEffect))]
    public class BarrelEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(BarrelEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty FactorProperty =
            DependencyProperty.Register("Factor", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(1.45D, PixelShaderConstantCallback(0)));
        public double Factor
        {
            get { return ((double)(GetValue(FactorProperty))); }
            set { SetValue(FactorProperty, value); }
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
            UpdateShaderValue(FactorProperty);
        }        
    }
}