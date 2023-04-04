using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    [ContentProperty("password")]
    class MatchPassword : ValidationRule
    {
        public Password password { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = password.Value;
            if (text == null || text == "") return new ValidationResult(false, "Vui lòng nhập mật khẩu");

            return (!password.Value.Equals((string)value))
                ? new ValidationResult(false, "Mật khẩu không trùng khớp")
                : ValidationResult.ValidResult;
        }
    }

    public class Password : DependencyObject
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(Password),
            new PropertyMetadata(default(string)));
    }
}
