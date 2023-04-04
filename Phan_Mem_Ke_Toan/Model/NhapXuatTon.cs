using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    public class NhapXuatTon
    {
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public string TenDVT { get; set; }
        public KeyValuePair<double, decimal> TonDauKy { get; set; }
        public KeyValuePair<double, decimal> Nhap { get; set; }
        public KeyValuePair<double, decimal> Xuat { get; set; }
        public KeyValuePair<double, decimal> TonCuoiKy { get; set; }

    }
}
