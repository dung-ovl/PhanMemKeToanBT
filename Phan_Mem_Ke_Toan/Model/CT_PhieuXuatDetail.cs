using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    public class CT_PhieuXuatDetail
    {
        public int MaSo { get; set; }
        public string SoPhieu { get; set; }
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public string TenDVT { get; set; }
        public string MaTK { get; set; }
        public double SLSoSach { get; set; }
        public double SLThucTe { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
