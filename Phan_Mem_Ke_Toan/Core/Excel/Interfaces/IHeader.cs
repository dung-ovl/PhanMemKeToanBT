using System.Collections.Generic;

namespace Phan_Mem_Ke_Toan.Core.Excel.Interfaces
{
    internal interface IHeader
    {
        IDictionary<string, string> MapHeader { get; }
        void AddHeader(string location, string value);
        void RemoveHeader(string location);
    }
}
