using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp1.Converters
{
    class MultiplierConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
            double multiplier = Convert.ToDouble(parameter);   
            return (double)value * multiplier;
            
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double multiplier = Convert.ToDouble(parameter);
            return (double)value / multiplier;
        }
    }
}
