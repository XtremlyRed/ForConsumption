using System;
using System.Globalization;

using Xamarin.Forms;

namespace ForConsumption.Converters
{
    public class GroupTitleMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool target || target)
            {
                return new Thickness(0, 0, 0, 0);
            }
            int margin = (int)System.Convert.ChangeType(parameter, typeof(int));

            return new Thickness(margin, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
