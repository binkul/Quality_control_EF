﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Quality_Control_EF.Converters
{
    public class BoolToNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
