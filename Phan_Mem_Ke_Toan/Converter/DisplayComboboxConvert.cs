using Phan_Mem_Ke_Toan.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Phan_Mem_Ke_Toan.Converter
{
    class DisplayComboboxConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value.GetType().Name;
            if (type.Equals("VatTuDetail"))
            {
                VatTuDetail model = value as VatTuDetail;
                return model.MaVT + " - " + model.TenVT;
            }
            if (type.Equals("Kho"))
            {
                Kho model = value as Kho;
                return model.MaKho + " - " + model.TenKho;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
