namespace Phan_Mem_Ke_Toan.Core.Excel.Interfaces
{
    internal interface IInsertExcel<THeader, TRecord> : IWriteAction
        where THeader : IHeader
        where TRecord : IRecord
    {
        void InsertRecord(TRecord[] item);

        void InsertHeader(THeader info);

        void Save(string exportPath);
    }
}
