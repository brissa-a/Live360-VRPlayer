using System;
using System.Globalization;
using System.Windows.Data;

namespace VrPlayer.Helpers.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ObjectToTypeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Todo: use .resx files to return correspoding label.
            return value == null ? "None" : value.GetType().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}