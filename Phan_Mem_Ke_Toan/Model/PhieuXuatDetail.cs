using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    public class PhieuXuatDetail
    {
        public string SoPhieu { get; set; }
        public DateTime NgayXuat { get; set; }
        public string MaCongTrinh { get; set; }
        public string TenCongTrinh { get; set; }
        public string DiaChiCT { get; set; }
        public string MaNguoiNhan { get; set; }
        public string TenNguoiNhan { get; set; }
        public string MaKho { get; set; }
        public string TenKho { get; set; }
        public string DiaChiKho { get; set; }
        public string LyDo { get; set; }
        public string TKNo { get; set; }
        public decimal TongTien { get; set; }
        public string ChungTuLQ { get; set; }
    }
}
