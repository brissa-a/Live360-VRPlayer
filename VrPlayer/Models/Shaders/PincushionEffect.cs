using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VrPlayer.Models.Shaders
{
	public class PincushionEffect : ShaderEffect 
	{
		public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(PincushionEffect), 0);
		public static readonly DependencyProperty BarrelFactorProperty = DependencyProperty.Register("BarrelFactor", typeof(double), typeof(PincushionEffect), new UIPropertyMetadata((0D), PixelShaderConstantCallback(0)));
		
		public PincushionEffect() 
		{
			PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Models/Shaders/PincushionEffect.ps");
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

		public double Factor 
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
