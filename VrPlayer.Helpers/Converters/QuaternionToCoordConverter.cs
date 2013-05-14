using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace VrPlayer.Helpers.Converters
{
    public class QuaternionToCoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var quaternion = (Quaternion)value;
            //Todo: temp - return Vector3D X
            //Todo: support Y and Z via parameter
            return quaternion.X;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coord = (double)value;
            return new Vector3D(coord, 0, 0);
        }
    }
}