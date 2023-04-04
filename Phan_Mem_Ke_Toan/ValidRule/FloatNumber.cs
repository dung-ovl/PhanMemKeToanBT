using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    class FloatNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((string)value)) return new ValidationResult(false, "Số không hợp lệ");
            return Regex.Match(value.ToString(), @"^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$").Success
            ? ValidationResult.ValidResult
            : new ValidationResult(false, "Số không hợp lệ");
        }
    }
}
