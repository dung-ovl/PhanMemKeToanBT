using Microsoft.Office.Interop.Excel;
using Phan_Mem_Ke_Toan.Core.Excel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.Core.Excel.Implements.Warehouse
{
    internal class WarehouseExcel :
        BaseExcel,
        IInsertExcel<WarehouseHeader, WarehouseRecord>,
        IReadExcel<WarehouseHeader, WarehouseRecord>
    {
        public const int H_OFFSET = 8; //Row index start records
        public const int V_OFFSET = 8; //Column index end records

        public WarehouseExcel(string openFile = "")
        {
            if (string.IsNullOrEmpty(openFile))
                template = TemplateFile.TemplateWarehouse;
            else
            {
                FileInfo fileInfo = new FileInfo(openFile);
                if (fileInfo.Exists && (fileInfo.Extension == ".xlsx" || fileInfo.Extension == ".xls"))
                {
                    template = fileInfo.FullName;
                }
                else throw new ArgumentException("File not supprted");
            }
            InitExcel();
        }

        public void InsertHeader(WarehouseHeader info)
        {
            foreach (var item in info.MapHeader)
            {
                sheet.get_Range(item.Key).Value = item.Value;
            }
        }

        public void InsertRecord(WarehouseRecord[] item)
        {
            int countRecords = item.Count();
            var startCell = (Range)sheet.Cells[H_OFFSET, 1];
            var endCell = (Range)sheet.Cells[countRecords + H_OFFSET - 1, V_OFFSET];
            var writeRange = sheet.get_Range(startCell, endCell);
            string[,] convert = new string[countRecords, V_OFFSET];
            Parallel.For(0, countRecords, v_index =>
            {
                for (int h_index = 0; h_index < V_OFFSET; h_index++)
                {
                    if (item[v_index].MapRecord.TryGetValue((GetExcelColumnName(h_index + 1), H_OFFSET + v_index), out string value))
                        convert[v_index, h_index] = value;
                    else convert[v_index, h_index] = string.Empty;
                }
            });
            writeRange.set_Value(Type.Missing, convert);
        }

        public object Read()
        {
            throw new NotImplementedException();
        }

        public WarehouseHeader ReadHeader()
        {
            return new WarehouseHeaderBuilder(false)
                .AddSoPhieu(sheet.get_Range(WarehouseHeaderIndex.IndexSoPhieu).Value?.ToString())
                .AddNguoiGiao(sheet.get_Range(WarehouseHeaderIndex.IndexNguoiGiao).Value?.ToString())
                .AddNhapVaoKho(sheet.get_Range(WarehouseHeaderIndex.IndexNhapVaoKho).Value?.ToString())
                .AddLyDo(sheet.get_Range(WarehouseHeaderIndex.IndexLyDo).Value?.ToString())
                .AddNgayNhap(sheet.get_Range(WarehouseHeaderIndex.IndexNgayNhap).Value?.ToString())
                .AddNhaCungCap(sheet.get_Range(WarehouseHeaderIndex.IndexNhaCungCap).Value?.ToString())
                .AddChungTuLienQuan(sheet.get_Range(WarehouseHeaderIndex.IndexChungTuLienQuan).Value?.ToString())
                .AddTaiKhoanCo(sheet.get_Range(WarehouseHeaderIndex.IndexTaiKhoanCo).Value?.ToString())
                .Build();
        }

        public WarehouseRecord[] ReadRecords()
        {
            int lastRow = FindLastRowUsed();
            var startCell = (Range)sheet.Cells[H_OFFSET, 1];
            var endCell = (Range)sheet.Cells[lastRow, V_OFFSET];
            var readRange = sheet.get_Range(startCell, endCell);
            var rows = readRange.get_Value(XlRangeValueDataType.xlRangeValueDefault) as object[,];
            var countRecords = rows.GetLength(0);
            WarehouseRecord[] warehouses = new WarehouseRecord[countRecords];
            Parallel.For(0, countRecords, v_index =>
            {
                WarehouseRecord record = new WarehouseRecord();
                int rowIndex = v_index + 1;
                int rowRecord = H_OFFSET + v_index;
                for (int h_index = 1; h_index <= V_OFFSET; h_index++)
                {
                    record.AddRecord(GetExcelColumnName(h_index), rowRecord, rows[rowIndex, h_index]?.ToString());
                }
                warehouses[v_index] = record;
            });
            return warehouses;
        }

        public void Save(string exportPath)
        {
            if (string.IsNullOrWhiteSpace(exportPath)) throw new Exception();
            exportPath = Path.GetFullPath(exportPath);
            if (File.Exists(exportPath))
            {
                File.Delete(exportPath);
            }

            workbook.SaveAs(exportPath);
        }

        public void Write()
        {
            throw new NotImplementedException();
        }
    }
}
