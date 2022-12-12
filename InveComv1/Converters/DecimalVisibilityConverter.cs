using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace InveComv1.Converters
{
    public class DecimalVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string visible = (string)value;
            try
            {
                Decimal.Parse(visible);
                return Visibility.Collapsed;
            }
            catch (System.FormatException ex)
            {
                return Visibility.Visible;
            }
            catch (System.ArgumentNullException ex)
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
