﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Quality_Control_EF.Converters
{
    public class EmptyStringToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : string.IsNullOrEmpty(value.ToString()) ? null : value;
        }
    }
}
