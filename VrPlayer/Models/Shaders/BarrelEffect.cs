using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Shaders
{	
	public class BarrelEffect : ShaderEffect 
    {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BarrelEffect), 0);
		public BarrelEffect() {
			PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Models/Shaders/BarrelEffect.ps");
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
	}
}
