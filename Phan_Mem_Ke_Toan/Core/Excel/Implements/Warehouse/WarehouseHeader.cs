using Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse.Builder;
using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;
using System.Collections.Generic;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse
{
    internal class WarehouseHeaderIndex
    {
        public const string IndexHeader = "A1";
        public const string IndexSoPhieu = "C2";
        public const string IndexNguoiGiao = "C3";
        public const string IndexNhapVaoKho = "C4";
        public const string IndexLyDo = "C5";
        public const string IndexNgayNhap = "E2";
        public const string IndexNhaCungCap = "E3";
        public const string IndexChungTuLienQuan = "E4";
        public const string IndexTaiKhoanCo = "E5";
    }

    internal class WarehouseHeader : IHeader
    {
        public IDictionary<string, string> MapHeader { get; private set; }

        public WarehouseHeader()
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

    internal class WarehouseHeaderModel
    {
        private readonly IHeader header;

        public WarehouseHeaderModel(IHeader header)
        {
            this.header = header;
        }

        public string Header
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexHeader))
                    return header.MapHeader[WarehouseHeaderIndex.IndexHeader];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexHeader, value);
            }
        }

        public string SoPhieu
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexSoPhieu))
                    return header.MapHeader[WarehouseHeaderIndex.IndexSoPhieu];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexSoPhieu, value);
            }
        }

        public string NguoiGiao
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexNguoiGiao))
                    return header.MapHeader[WarehouseHeaderIndex.IndexNguoiGiao];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexNguoiGiao, value);
            }
        }

        public string NhapVaoKho
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexNhapVaoKho))
                    return header.MapHeader[WarehouseHeaderIndex.IndexNhapVaoKho];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexNhapVaoKho, value);
            }
        }

        public string LyDo
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexLyDo))
                    return header.MapHeader[WarehouseHeaderIndex.IndexLyDo];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexLyDo, value);
            }
        }

        public string NgayNhap
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexNgayNhap))
                    return header.MapHeader[WarehouseHeaderIndex.IndexNgayNhap];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexNgayNhap, value);
            }
        }

        public string NhaCungCap
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexNhaCungCap))
                    return header.MapHeader[WarehouseHeaderIndex.IndexNhaCungCap];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexNhaCungCap, value);
            }
        }

        public string ChungTuLienQuan
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexChungTuLienQuan))
                    return header.MapHeader[WarehouseHeaderIndex.IndexChungTuLienQuan];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexChungTuLienQuan, value);
            }
        }

        public string TaiKhoanCo
        {
            get
            {
                if (header.MapHeader.ContainsKey(WarehouseHeaderIndex.IndexTaiKhoanCo))
                    return header.MapHeader[WarehouseHeaderIndex.IndexTaiKhoanCo];
                return string.Empty;
            }
            set
            {
                header.AddHeader(WarehouseHeaderIndex.IndexTaiKhoanCo, value);
            }
        }
    }

    internal class WarehouseHeaderBuilder : IHeaderBuilder<WarehouseHeader>
    {
        private WarehouseHeader header;

        public WarehouseHeaderBuilder(bool isImport = true)
        {
            Reset(isImport);
        }

        public IHeaderBuilder<WarehouseHeader> AddSoPhieu(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexSoPhieu, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddNguoiGiao(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexNguoiGiao, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddNhapVaoKho(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexNhapVaoKho, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddLyDo(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexLyDo, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddNgayNhap(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexNgayNhap, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddNhaCungCap(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexNhaCungCap, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddChungTuLienQuan(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexChungTuLienQuan, value);
            return this;
        }

        public IHeaderBuilder<WarehouseHeader> AddTaiKhoanCo(string value)
        {
            header.AddHeader(WarehouseHeaderIndex.IndexTaiKhoanCo, value);
            return this;
        }

        public WarehouseHeader Build()
        {
            return header;
        }

        public void Reset(bool isImport = true)
        {
            header = new WarehouseHeader();
            header.AddHeader(WarehouseHeaderIndex.IndexHeader, isImport ? "PHIẾU NHẬP KHO" : "PHIẾU XUẤT KHO");
        }
    }
}
