using System;
using System.Windows.Data;

namespace VrPlayer.Helpers.Converters
{
    public class CompareStringParameterConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.ToString() == parameter.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
    }
}