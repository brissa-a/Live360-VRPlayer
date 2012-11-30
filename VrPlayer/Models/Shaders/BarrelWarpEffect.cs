using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VrPlayer.Models.Shaders
{
	public class BarrelWarpEffect : ShaderEffect 
	{
		public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(BarrelWarpEffect), 0);
		public static readonly DependencyProperty BarrelFactorProperty = DependencyProperty.Register("BarrelFactor", typeof(double), typeof(BarrelWarpEffect), new UIPropertyMetadata((0D), PixelShaderConstantCallback(0)));
		
		public BarrelWarpEffect() 
		{
			PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Models/Shaders/BarrelWarpEffect.ps");
			PixelShader = pixelShader;
			UpdateShaderValue(InputProperty);
			UpdateShaderValue(BarrelFactorProperty);
		}

		public Brush Input 
		{
			get 
			{
				return ((Brush)(GetValue(InputProperty)));
			}
			set 
			{
				SetValue(InputProperty, value);
			}
		}

		public double BarrelFactor 
		{
			get 
			{
				return ((double)(GetValue(BarrelFactorProperty)));
			}
			set 
			{
				SetValue(BarrelFactorProperty, value);
			}
		}
	}
}
