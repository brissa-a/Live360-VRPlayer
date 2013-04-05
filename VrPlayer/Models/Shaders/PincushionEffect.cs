using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Shaders
{
	public class PincushionEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(PincushionEffect), 0);
		public static readonly DependencyProperty FactorProperty = DependencyProperty.Register("Factor", typeof(double), typeof(PincushionEffect), new UIPropertyMetadata(((double)(5D)), PixelShaderConstantCallback(0)));
		public PincushionEffect() {
			PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Models/Shaders/PincushionEffect.ps"); this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(FactorProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		public double Factor {
			get {
				return ((double)(this.GetValue(FactorProperty)));
			}
			set {
				this.SetValue(FactorProperty, value);
			}
		}
	}
}
