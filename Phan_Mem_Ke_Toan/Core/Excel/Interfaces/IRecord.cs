using System.Collections.Generic;

namespace Phan_Mem_Ke_Toan.Core.Excel.Interfaces
{
    internal interface IRecord
    {
        IDictionary<(string column, int row), string> MapRecord { get; }
        void AddRecord(string column, int row, string value);
        void AddRecord((string column, int row) cell, string value);
        void RemoveRecord(string column, int row);
    }
}
