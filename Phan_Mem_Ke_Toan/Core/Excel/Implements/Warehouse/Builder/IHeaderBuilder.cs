using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse.Builder
{
    internal interface IHeaderBuilder<THeader> where THeader : IHeader
    {
        IHeaderBuilder<THeader> AddSoPhieu(string value);
        IHeaderBuilder<THeader> AddNguoiGiao(string value);
        IHeaderBuilder<THeader> AddNhapVaoKho(string value);
        IHeaderBuilder<THeader> AddLyDo(string value);
        IHeaderBuilder<THeader> AddNgayNhap(string value);
        IHeaderBuilder<THeader> AddNhaCungCap(string value);
        IHeaderBuilder<THeader> AddChungTuLienQuan(string value);
        IHeaderBuilder<THeader> AddTaiKhoanCo(string value);
        THeader Build();
    }
}
