using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace test_projet
{
    public class ExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList<string> extensions && parameter is string extension)
            {
                return extensions.Contains(extension);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}