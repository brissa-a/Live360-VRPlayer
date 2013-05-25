using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VrPlayer.Helpers.Converters
{
    public class CompareStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Visible;
            try
            {
                visibility = (value.ToString() != parameter.ToString())? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception)
            {
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}