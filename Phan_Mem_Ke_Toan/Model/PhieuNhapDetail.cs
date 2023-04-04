using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    public class PhieuNhapDetail
    {
        public string SoPhieu { get; set; }
        public DateTime NgayNhap { get; set; }
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string MaNguoiGiao { get; set; }
        public string TenNguoiGiao { get; set; }
        public string MaKho { get; set; }
        public string TenKho { get; set; }
        public string DiaChi { get; set; }
        public string LyDo { get; set; }
        public string TKCo { get; set; }
        public decimal TongTien { get; set; }
        public string ChungTuLQ { get; set; }
    }
}
