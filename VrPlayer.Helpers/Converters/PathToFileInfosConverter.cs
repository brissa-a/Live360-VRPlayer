using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace VrPlayer.Helpers.Converters
{
    //Todo: Create a data provider instead of a converter.
    [ValueConversion(typeof(string), typeof(IEnumerable<FileInfo>))]
    public class PathToFileInfosConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dir = new DirectoryInfo(value.ToString());
            var filter = FileFilterHelper.GetFilter();
            return dir.GetFiles().Where(info => filter.Contains(info.Extension));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}