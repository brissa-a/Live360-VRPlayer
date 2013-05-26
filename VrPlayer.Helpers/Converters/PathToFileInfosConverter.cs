using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            return dir.GetFiles();   
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}