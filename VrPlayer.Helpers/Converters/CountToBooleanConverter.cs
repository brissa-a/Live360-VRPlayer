using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections;

namespace VrPlayer.Helpers.Converters
{
    public class CountToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasElements = false;
            try
            {
                var list = (ICollection)value;
                hasElements = list.Count > 0;
            }
            catch (Exception)
            {
            }
            return hasElements;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}