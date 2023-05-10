using Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords.Builder;
using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;
using System.Collections.Generic;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.InventoryRecords
{
    internal class InventoryRecordsHeaderIndex
    {
        public const string IndexHeader = "A1";
        public const string IndexTruongBan = "C2";
        public const string IndexUyVien1 = "C3";
        public const string IndexUyVien2 = "C4";
        public const string IndexSoBienBan = "E2";
        public const string IndexNgayLap = "E3";
    }

    internal class InventoryRecordsHeader : IHeader
    {
        public IDictionary<string, string> MapHeader { get; private set; }

        public InventoryRecordsHeader()
        {
            MapHeader = new Dictionary<string, string>();
        }

        public void AddHeader(string location, string value)
        {
            if (!MapHeader.ContainsKey(location))
            {
                MapHeader.Add(location, value);
            }
            else MapHeader[location] = value;
        }

        public void RemoveHeader(string location)
        {
            if (MapHeader.ContainsKey(location))
            {
                MapHeader.Remove(location);
            }
        }
    }

    internal class InventoryRecordsHeaderModel
    {
        private readonly IHeader header;

        public InventoryRecordsHeaderModel(IHeader header)
        {
            this.header = header;
        }

        public string Header
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexHeader))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexHeader];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexHeader, value);
            }
        }

        public string TruongBan
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexTruongBan))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexTruongBan];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexTruongBan, value);
            }
        }

        public string UyVien1
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexUyVien1))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexUyVien1];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexUyVien1, value);
            }
        }

        public string UyVien2
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexUyVien2))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexUyVien2];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexUyVien2, value);
            }
        }

        public string SoBienBan
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexSoBienBan))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexSoBienBan];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexSoBienBan, value);
            }
        }

        public string NgayLap
        {
            get
            {
                if (header.MapHeader.ContainsKey(InventoryRecordsHeaderIndex.IndexNgayLap))
                    return header.MapHeader[InventoryRecordsHeaderIndex.IndexNgayLap];
                return string.Empty;
            }
            set
            {
                header.AddHeader(InventoryRecordsHeaderIndex.IndexNgayLap, value);
            }
        }
    }

    internal class InventoryRecordsHeaderBuilder : IHeaderBuilder<InventoryRecordsHeader>
    {
        private InventoryRecordsHeader header;

        public InventoryRecordsHeaderBuilder()
        {
            Reset();
        }

        public IHeaderBuilder<InventoryRecordsHeader> AddNgayLap(string value)
        {
            header.AddHeader(InventoryRecordsHeaderIndex.IndexNgayLap, value);
            return this;
        }

        public IHeaderBuilder<InventoryRecordsHeader> AddSoBienBan(string value)
        {
            header.AddHeader(InventoryRecordsHeaderIndex.IndexSoBienBan, value);
            return this;
        }

        public IHeaderBuilder<InventoryRecordsHeader> AddTruongBan(string value)
        {
            header.AddHeader(InventoryRecordsHeaderIndex.IndexTruongBan, value);
            return this;
        }

        public IHeaderBuilder<InventoryRecordsHeader> AddUyVien1(string value)
        {
            header.AddHeader(InventoryRecordsHeaderIndex.IndexUyVien1, value);
            return this;
        }

        public IHeaderBuilder<InventoryRecordsHeader> AddUyVien2(string value)
        {
            header.AddHeader(InventoryRecordsHeaderIndex.IndexUyVien2, value);
            return this;
        }

        public InventoryRecordsHeader Build()
        {
            return header;
        }

        public void Reset()
        {
            header = new InventoryRecordsHeader();
            header.AddHeader(InventoryRecordsHeaderIndex.IndexHeader, "BIÊN BẢN KIỂM KÊ");
        }
    }
}
