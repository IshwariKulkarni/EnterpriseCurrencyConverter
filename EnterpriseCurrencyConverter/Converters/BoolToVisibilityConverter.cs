using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EnterpriseCurrencyConverter.Converters
{
    /// <summary>
    /// VALUE CONVERTER: Transforms bool → Visibility enum.
    /// 
    /// XAML cannot directly use a bool to show/hide elements.
    /// This converter sits between the ViewModel and the View.
    /// 
    /// true  → Visibility.Visible   (show the element)
    /// false → Visibility.Collapsed (hide AND collapse the element)
    /// 
    /// Implements IValueConverter — required interface for all XAML converters.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        // Called when data flows FROM ViewModel TO View
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        // Called when data flows FROM View TO ViewModel (not needed here)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
