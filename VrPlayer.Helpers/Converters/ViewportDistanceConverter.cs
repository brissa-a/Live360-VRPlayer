using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VrPlayer.Helpers.Converters
{
    public class ViewportDistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            Thickness thickness = new Thickness();
            try
            {
                if (val > 0)
                {
                    switch (parameter.ToString())
                    {
                        case "Left":
                            thickness.Left = -val;
                            break;
                        case "Right":
                            thickness.Right = -val;
                            break;
                        case "Top":
                            thickness.Top = -val;
                            break;
                        case "Bottom":
                            thickness.Bottom = -val;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (parameter.ToString())
                    {
                        case "Left":
                            thickness.Right = val;
                            break;
                        case "Right":
                            thickness.Left = val;
                            break;
                        case "Top":
                            thickness.Bottom = val;
                            break;
                        case "Bottom":
                            thickness.Left = val;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}