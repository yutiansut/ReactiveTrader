using System;
using Windows.UI.Xaml.Data;

namespace Adaptive.ReactiveTrader.Client.Converters
{
    public class EnumAsStringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum)
            {
                return value.ToString() == (string)parameter;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
