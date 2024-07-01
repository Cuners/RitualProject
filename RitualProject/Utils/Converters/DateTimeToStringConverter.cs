using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RitualProject
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime < DateTime.Today)
                {
                    if(dateTime.Year < DateTime.Today.Year)
                    {
                        return dateTime.ToString("yyyy-MM-dd HH:mm");
                    }
                    return dateTime.ToString("MMMM HH:mm");
                }
                else
                {
                    return dateTime.ToString("HH:mm");
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
