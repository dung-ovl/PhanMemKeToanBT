using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse.Builder
{
    internal interface IRecordBuilder<TRecord> where TRecord : IRecord
    {
        IRecordBuilder<TRecord> NewRecord();
        IRecordBuilder<TRecord> AddMaVatTu(string value);
        IRecordBuilder<TRecord> AddTenVatTu(string value);
        IRecordBuilder<TRecord> AddDonViTinh(string value);
        IRecordBuilder<TRecord> AddTaiKhoanNo(string value);
        IRecordBuilder<TRecord> AddSoLuongSoSach(string value);
        IRecordBuilder<TRecord> AddSoLuongThucTe(string value);
        IRecordBuilder<TRecord> AddDonGia(string value);
        TRecord[] Build();
    }
}
