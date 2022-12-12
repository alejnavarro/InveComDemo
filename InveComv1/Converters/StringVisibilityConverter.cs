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
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string visible = (string)value;
            if (String.IsNullOrEmpty(visible)||String.Equals(visible, "No se pudo obtener la tasa, favor insertar manualmente")|| String.Equals(visible,"Ingrese un valor de cambio valido para continuar"))
            {
                return Visibility.Visible;
            }
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
