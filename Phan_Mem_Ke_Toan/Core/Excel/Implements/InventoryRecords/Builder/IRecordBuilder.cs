using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords.Builder
{
    internal interface IRecordBuilder<TRecord> where TRecord : IRecord
    {
        IRecordBuilder<TRecord> AddMaVatTu(string value);
        IRecordBuilder<TRecord> AddTenVatTu(string value);
        IRecordBuilder<TRecord> AddDonViTinh(string value);
        IRecordBuilder<TRecord> AddSoLuongSoSach(string value);
        IRecordBuilder<TRecord> AddSoLuongThucTe(string value);
        IRecordBuilder<TRecord> AddSoLuongSanPhamTot(string value);
        IRecordBuilder<TRecord> AddSoLuongMatSanPham(string value);
        IRecordBuilder<TRecord> NewRecord();
        TRecord[] Build();
    }
}
