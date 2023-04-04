using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    class NotEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return (value == null || (string)value == "")
                ? new ValidationResult(false, "Vui lòng nhập thông tin")
                : ValidationResult.ValidResult;
        }
    }
}
