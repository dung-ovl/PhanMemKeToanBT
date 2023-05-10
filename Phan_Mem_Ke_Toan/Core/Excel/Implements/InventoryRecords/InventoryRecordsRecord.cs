using Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords.Builder;
using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords
{
    internal class InventoryRecordsRecordIndex
    {
        public static (string column, int row) IndexSTT(int row) => ("A", row);
        public static (string column, int row) IndexMaVatTu(int row) => ("B", row);
        public static (string column, int row) IndexTenVatTu(int row) => ("C", row);
        public static (string column, int row) IndexDonViTinh(int row) => ("D", row);
        public static (string column, int row) IndexSoLuongSoSach(int row) => ("E", row);
        public static (string column, int row) IndexSoLuongThucTe(int row) => ("F", row);
        public static (string column, int row) IndexSoLuongSanPhamTot(int row) => ("G", row);
        public static (string column, int row) IndexSoLuongMatSanPham(int row) => ("H", row);
    }

    internal class InventoryRecordsRecord : IRecord
    {
        public IDictionary<(string column, int row), string> MapRecord { get; private set; }

        public InventoryRecordsRecord()
        {
            MapRecord = new Dictionary<(string, int), string>();
        }

        public void AddRecord(string column, int row, string value)
        {
            AddRecord((column, row), value);
        }

        public void RemoveRecord(string column, int row)
        {
            if (MapRecord.ContainsKey((column, row)))
            {
                MapRecord.Remove((column, row));
            }
        }

        public void AddRecord((string column, int row) cell, string value)
        {
            if (!MapRecord.ContainsKey(cell))
            {
                MapRecord.Add(cell, value);
            }
            else MapRecord[cell] = value;
        }
    }

    internal class InventoryRecordsRecordModel
    {
        private readonly IRecord record;
        public int RowIndex { get; private set; }

        public InventoryRecordsRecordModel(IRecord record)
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
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexSTT(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexSTT(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexSTT(RowIndex), value);
            }
        }

        public string MaVatTu
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexMaVatTu(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexMaVatTu(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexMaVatTu(RowIndex), value);
            }
        }

        public string TenVatTu
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexTenVatTu(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexTenVatTu(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexTenVatTu(RowIndex), value);
            }
        }

        public string DonViTinh
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexDonViTinh(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexDonViTinh(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexDonViTinh(RowIndex), value);
            }
        }

        public string SoLuongSoSach
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexSoLuongSoSach(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexSoLuongSoSach(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexSoLuongSoSach(RowIndex), value);
            }
        }

        public string SoLuongThucTe
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexSoLuongThucTe(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexSoLuongThucTe(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexSoLuongThucTe(RowIndex), value);
            }
        }

        public string SoLuongSanPhamTot
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexSoLuongSanPhamTot(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexSoLuongSanPhamTot(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexSoLuongSanPhamTot(RowIndex), value);
            }
        }

        public string SoLuongMatSanPham
        {
            get
            {
                if (record.MapRecord.ContainsKey(InventoryRecordsRecordIndex.IndexSoLuongMatSanPham(RowIndex)))
                    return record.MapRecord[InventoryRecordsRecordIndex.IndexSoLuongMatSanPham(RowIndex)];
                return string.Empty;
            }
            set
            {
                record.AddRecord(InventoryRecordsRecordIndex.IndexSoLuongMatSanPham(RowIndex), value);
            }
        }
    }

    internal class InventoryRecordsRecordBuilder : IRecordBuilder<InventoryRecordsRecord>
    {
        private IList<IRecord> records;
        private const int OFFSET = InventoryRecordsExcel.H_OFFSET;

        /// <summary>
        /// Is STT in Excel
        /// </summary>
        public int Index
        {
            get => records.Count;
        }

        public InventoryRecordsRecordBuilder()
        {
            Reset();
        }

        public IRecordBuilder<InventoryRecordsRecord> AddDonViTinh(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexDonViTinh(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddMaVatTu(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexMaVatTu(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddSoLuongMatSanPham(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexSoLuongMatSanPham(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddSoLuongSanPhamTot(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexSoLuongSanPhamTot(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddSoLuongSoSach(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexSoLuongSoSach(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddSoLuongThucTe(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexSoLuongThucTe(OFFSET + Index - 1), value);
            return this;
        }

        public IRecordBuilder<InventoryRecordsRecord> AddTenVatTu(string value)
        {
            records.Last().AddRecord(InventoryRecordsRecordIndex.IndexTenVatTu(OFFSET + Index - 1), value);
            return this;
        }

        public InventoryRecordsRecord[] Build()
        {
            return records.Cast<InventoryRecordsRecord>().ToArray();
        }

        public IRecordBuilder<InventoryRecordsRecord> NewRecord()
        {
            IRecord record = new InventoryRecordsRecord();
            records.Add(record);
            int index = Index;
            record.AddRecord(InventoryRecordsRecordIndex.IndexSTT(OFFSET + index - 1), index.ToString());
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
    }
}
