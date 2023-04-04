using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Model
{
    class AccountSystem
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Quyen { get; set; }
        public string MaBoPhan { get; set; }

        public AccountSystem() { }
        public AccountSystem(AccountSystem data)
        {
            HoTen = data.HoTen;
            MaBoPhan = data.MaBoPhan;
            MatKhau = data.MatKhau;
            Quyen = data.Quyen;
            TenDangNhap = data.TenDangNhap;
        }
    }
}
