using System;
using System.IO;

namespace Phan_Mem_Ke_Toan.Core.Excel
{
    internal class TemplateFile
    {
        private static readonly string folderFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template"));
        private static readonly string warehouse = "Warehouse_Template.xlsx";
        private static readonly string inventory_records = "Inventory_Records_Template.xlsx";

        public static string TemplateWarehouse
        {
            get { return Path.Combine(folderFile, warehouse); }
        }

        public static string TemplateInventoryRecords
        {
            get { return Path.Combine(folderFile, inventory_records); }
        }
    }
}
