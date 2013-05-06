using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.ColorKeyAlpha
{
    [Export(typeof(EffectBase))]
    public class ColorKeyAlphaEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyAlphaEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty ColorKeyProperty =
            DependencyProperty.Register("ColorKey", typeof(Color), typeof(ColorKeyAlphaEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 128, 0), PixelShaderConstantCallback(0)));
        public Color ColorKey
        {
            get { return ((Color)(GetValue(ColorKeyProperty))); }
            set { SetValue(ColorKeyProperty, value); }
        }

        public static readonly DependencyProperty ToleranceProperty =
            DependencyProperty.Register("Tolerance", typeof(double), typeof(ColorKeyAlphaEffect), new UIPropertyMetadata(0.3D, PixelShaderConstantCallback(1)));
        public double Tolerance
        {
            get { return ((double)(GetValue(ToleranceProperty))); }
            set { SetValue(ToleranceProperty, value); }
        }

        public ColorKeyAlphaEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.ColorKeyAlpha",
                "ColorKeyAlphaEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ColorKeyProperty);
            UpdateShaderValue(ToleranceProperty);
        }
    }
}
