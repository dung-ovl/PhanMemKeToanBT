using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords.Builder
{
    internal interface IHeaderBuilder<THeader> where THeader : IHeader
    {
        IHeaderBuilder<THeader> AddTruongBan(string value);
        IHeaderBuilder<THeader> AddUyVien1(string value);
        IHeaderBuilder<THeader> AddUyVien2(string value);
        IHeaderBuilder<THeader> AddSoBienBan(string value);
        IHeaderBuilder<THeader> AddNgayLap(string value);
        THeader Build();
    }
}
