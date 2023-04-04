using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    public class BienBanDetail
    {
        public string SoBienBan { get; set; }
        public DateTime NgayLap { get; set; }
        public string MaKho { get; set; }
        public string TenKho { get; set; }

        public string TruongBan { get; set; }
        public string TenTruongBan { get; set; }
        public string UyVien1 { get; set; }
        public string TenUyVien1 { get; set; }
        public string UyVien2 { get; set; }
        public string TenUyVien2 { get; set; }
    }
}
