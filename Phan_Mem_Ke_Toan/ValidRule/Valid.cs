using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Phan_Mem_Ke_Toan.ValidRule
{
    public class Valid
    {
        public static bool IsValid(DependencyObject obj)
        {
            if (Validation.GetHasError(obj))
                return false;

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(obj); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (!IsValid(child)) { return false; }
            }
            return true;
        }
        public static bool isPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(0\d{9})").Success;
        }
        public static bool isMaSoThue(string MaSo)
        {
            return Regex.Match(MaSo, @"^(\d{2} \d{8}-\d{3})").Success;
        }
    }
}
