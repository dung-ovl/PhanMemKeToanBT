namespace Phan_Mem_Ke_Toan.Core.Excel.Interfaces
{
    internal interface IReadExcel<THeader, TRecord> : IReadAction
        where THeader : IHeader
        where TRecord : IRecord
    {
        TRecord[] ReadRecords();

        THeader ReadHeader();
    }
}
