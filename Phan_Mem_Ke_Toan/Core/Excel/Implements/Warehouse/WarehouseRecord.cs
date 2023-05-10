using Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse.Builder;
using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse
{
    internal class WarehouseRecordIndex
    {
        public static (string column, int row) IndexSTT(int row) => ("A", row);
        public static (string column, int row) IndexMaVatTu(int row) => ("B", row);
        public static (string column, int row) IndexTenVatTu(int row) => ("C", row);
        public static (string column, int row) IndexDonViTinh(int row) => ("D", row);
        public static (string column, int row) IndexTaiKhoanNo(int row) => ("E", row);
        public static (string column, int row) IndexSoLuongSoSach(int row) => ("F", row);
        public static (string column, int row) IndexSoLuongThucTe(int row) => ("G", row);
        public static (string column, int row) IndexDonGia(int row) => ("H", row);
    }

    internal class WarehouseRecord : IRecord
    {
        public IDictionary<(string column, int row), string> MapRecord { get; private set; }

        public WarehouseRecord()
        {
            MapRecord = new Dictionary<(string, int), string>();
        }

        public void AddRecord((string column, int row) cell, string value)
        {
            if (!MapRecord.ContainsKey(cell))
            {
                MapRecord.Add(cell, value);
            }
            else MapRecord[cell] = value;
        }

        public void RemoveRecord(string column, int row)
        {
            if (MapRecord.ContainsKey((column, row)))
            {
                MapRecord.Remove((column, row));
            }
        }

        public void AddRecord(string column, int row, string value)
        {
            AddRecord((column, row), value);
        }
    }

    internal class WarehouseRecordModel
    {
        private readonly IRecord record;
        public int RowIndex { get; private set; }

        public WarehouseRecordModel(IRecord record)
        {
            this.record = record;
            RowIndex = this.record.MapRecord.First().Key.row;
        }

        /// <summary>
        /// Caution!!! 1 record has only 1 rowIndex. Only use when record empty
        /// </summary>
        /// <param name="rowIndex"></param>
        public void SetRowIndex(int rowIndex)
        {
            RowIndex = rowIndex;
        }

        public string STT
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexSTT(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexSTT(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexSTT(RowIndex), value);
            }
        }

        public string MaVatTu
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexMaVatTu(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexMaVatTu(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexMaVatTu(RowIndex), value);
            }
        }

        public string TenVatTu
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexTenVatTu(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexTenVatTu(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexTenVatTu(RowIndex), value);
            }
        }

        public string DonViTinh
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexDonViTinh(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexDonViTinh(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexDonViTinh(RowIndex), value);
            }
        }

        public string TaiKhoanNo
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexTaiKhoanNo(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexTaiKhoanNo(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexTaiKhoanNo(RowIndex), value);
            }
        }

        public string SoLuongSoSach
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexSoLuongSoSach(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexSoLuongSoSach(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexSoLuongSoSach(RowIndex), value);
            }
        }

        public string SoLuongThucTe
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexSoLuongThucTe(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexSoLuongThucTe(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexSoLuongThucTe(RowIndex), value);
            }
        }

        public string DonGia
        {
            get
            {
                if (record.MapRecord.ContainsKey(WarehouseRecordIndex.IndexDonGia(RowIndex)))
                    return record.MapRecord[WarehouseRecordIndex.IndexDonGia(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(WarehouseRecordIndex.IndexDonGia(RowIndex), value);
            }
        }
    }

    internal class WarehouseRecordBuilder : IRecordBuilder<WarehouseRecord>
    {
        private IList<IRecord> records;
        private const int OFFSET = WarehouseExcel.H_OFFSET;

        /// <summary>
        /// Is STT in Excel
        /// </summary>
        public int Index
        {
            get => records.Count;
        }

        public WarehouseRecordBuilder()
        {
            Reset();
        }

        public IRecordBuilder<WarehouseRecord> AddDonGia(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexDonGia(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddDonViTinh(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexDonViTinh(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddMaVatTu(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexMaVatTu(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddSoLuongSoSach(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexSoLuongSoSach(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddSoLuongThucTe(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexSoLuongThucTe(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddTaiKhoanNo(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexTaiKhoanNo(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<WarehouseRecord> AddTenVatTu(string value)
        {
            records.Last().AddRecord(WarehouseRecordIndex.IndexTenVatTu(OFFSET + Index - 1), value);
            return this;
        }

        public void Reset()
        {
            if (records == null)
                records = new List<IRecord>();
            else
                records.Clear();
            NewRecord();
        }

        public IRecordBuilder<WarehouseRecord> NewRecord()
        {
            IRecord record = new WarehouseRecord();
            records.Add(record);
            int index = Index;
            record.AddRecord(WarehouseRecordIndex.IndexSTT(OFFSET + index - 1), index.ToString());
            return this;
        }

        public WarehouseRecord[] Build()
        {
            return records.Cast<WarehouseRecord>().ToArray();
        }
    }
}
