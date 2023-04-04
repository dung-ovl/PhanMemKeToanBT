using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Phan_Mem_Ke_Toan.Converter
{
    class MoneyVNConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((decimal)value == 0) return value;
            return String.Format("{0:#,#}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string money = value.ToString().Replace(",", "");
            return decimal.Parse(money);
        }
    }
}
