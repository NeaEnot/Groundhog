using Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowsDesktop.Converters
{
    internal class DateTimeConverter : IValueConverter
    {
        private string[] formats = { "dd.MM.yyyy", "dddd, dd MMMM" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString(formats[int.Parse((string)parameter)], new CultureInfo(GroundhogContext.Language.Culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.ParseExact((string)value, formats[int.Parse((string)parameter)], new CultureInfo(GroundhogContext.Language.Culture));
        }
    }
}
