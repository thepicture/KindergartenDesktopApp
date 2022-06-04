﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace KindergartenDesktopApp.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return false;
            }
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return false;
            }
            return !(bool)value;
        }
    }
}
