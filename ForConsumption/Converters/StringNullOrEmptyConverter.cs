
using System;
using System.Globalization;

using Xamarin.Forms;

namespace ForConsumption.Converters
{
    public class StringNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string desc = parameter?.ToString() ?? string.Empty;
            if (value == null)
            {
                return desc;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
