using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace VrPlayer.Helpers.Converters
{
    public class Vector3DToCoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vector = (Vector3D)value;
            //Todo: support Y and Z via parameter
            return vector.X;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coord = (double)value;
            return new Vector3D(coord, 0, 0);
        }
    }
}