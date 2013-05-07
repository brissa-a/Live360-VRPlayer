using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace VrPlayer.Helpers.Converters
{
    public class Vector3DToQuaternionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vector = (Vector3D)value;
            return QuaternionHelper.QuaternionFromEulerAngles(vector);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}