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
    class PhoneNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
               return Regex.Match(value.ToString(), @"^(0\d{9})").Success
               ? ValidationResult.ValidResult
               : new ValidationResult(false, "Số điện thoại không hợp lệ");
            return ValidationResult.ValidResult;
        }
    }
}
