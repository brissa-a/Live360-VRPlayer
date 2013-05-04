using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VrPlayer.Distortions.Pincushion
{
    [Export(typeof(ShaderEffect))]
    public class PincushionEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(PincushionEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }
        
        public static readonly DependencyProperty FactorProperty =
            DependencyProperty.Register("Factor", typeof(double), typeof(PincushionEffect), new UIPropertyMetadata(5D, PixelShaderConstantCallback(0)));
        public double Factor
        {
            get { return ((double)(GetValue(FactorProperty))); }
            set { SetValue(FactorProperty, value); }
        }

        public PincushionEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Distortions.Pincushion",
                "PincushionEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FactorProperty);
        }
    }
}