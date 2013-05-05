using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VrPlayer.Effects.UnwrapFishEyeStereo
{
    [Export(typeof(ShaderEffect))]
    public class UnwrapFishEyeStereoEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(UnwrapFishEyeStereoEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty SampleInputParamProperty =
            DependencyProperty.Register("SampleInputParam", typeof(double), typeof(UnwrapFishEyeStereoEffect), new UIPropertyMetadata(3.5D, PixelShaderConstantCallback(0)));
        public double SampleInputParam
        {
            get { return ((double)(GetValue(SampleInputParamProperty))); }
            set { SetValue(SampleInputParamProperty, value); }
        }

        public UnwrapFishEyeStereoEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.UnwrapFishEyeStereo",
                "UnwrapFishEyeStereoEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(SampleInputParamProperty);
        }
    }
}
