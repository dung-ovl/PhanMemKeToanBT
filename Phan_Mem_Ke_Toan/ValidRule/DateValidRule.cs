using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    public class DateValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime result;
            if (!DateTime.TryParse((value ?? "").ToString(),
               CultureInfo.CurrentCulture,
               DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out result)) 
                return new ValidationResult(false, "Không hợp lệ");
            return ValidationResult.ValidResult;
        }
    }
}
