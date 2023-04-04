using Phan_Mem_Ke_Toan.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    class SLPhamChatValid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var item = (value as BindingGroup).Items[0] as CT_BienBanDetail;
            return (item.SLThucTe != item.SLPhamChatTot + item.SLPhamChatKem + item.SLMatPhamChat)
                ? new ValidationResult(false, "Tổng SL phẩm chất phải bằng SL thực tế")
                : ValidationResult.ValidResult;
        }
    }
}
