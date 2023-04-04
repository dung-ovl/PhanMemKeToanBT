
using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.ValidRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using MessageBox = System.Windows.MessageBox;
using System.Diagnostics;
using System.Globalization;


namespace Phan_Mem_Ke_Toan.ViewModel
{
    public class LapBaoCaoNXTViewModel : BaseViewModel
    {
        private int _selectedMonth;
        public int selectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }
        private int _selectedYear;
        public int selectedYear
        {
            get => _selectedYear;
            set => SetProperty(ref _selectedYear, value);
        }

        private string _selectedMaKho;
        public string selectedMaKho
        {
            get => _selectedMaKho;
            set => SetProperty(ref _selectedMaKho, value);
        }
        public ObservableCollection<VatTuDetail> ListVT { get; set; }
        public ObservableCollection<Kho> ListKho { get; set; }
        public ObservableCollection<int> ListMonth { get; set; }
        public ObservableCollection<int> ListYear { get; set; }
        public ICommand ExportCommand { get; set; }
        public LapBaoCaoNXTViewModel()
        {
            selectedMaKho = string.Empty;
            GetListVatTu();
            GetListKho();
            ListMonth = new ObservableCollection<int>();
            for (int i = 1; i <= 12; i++)
                ListMonth.Add(i);
            ListYear = new ObservableCollection<int>();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year; i++)
                ListYear.Add(i);

            selectedMonth = DateTime.Now.Month;
            selectedYear = DateTime.Now.Year;
            ExportCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                ExportBaoCaoNXT();
            });
        }
        public void GetListVatTu()
        {
            string data = CRUD.GetJoinTableData("VatTu");
            ListVT = JsonConvert.DeserializeObject<ObservableCollection<VatTuDetail>>(data);
        }
        public void GetListKho()
        {
            string data = CRUD.GetJsonData("Kho");
            ListKho = JsonConvert.DeserializeObject<ObservableCollection<Kho>>(data);
        }
        public string GetTenKho()
        {
            foreach (var item in ListKho)
            {
                if (item.MaKho == selectedMaKho)
                    return item.TenKho;
            }
            return null;
        }
        public VatTuDetail GetDetailVT(string MaVT)
        {
            foreach (var item in ListVT)
            {
                if (item.MaVT == MaVT)
                    return item;
            }
            return null;
        }
        public List<DuDauVatTu> GetTonKho(string MaKho, int Thang, int Nam)
        {
            string url = "dudauvattu/alltonkhothang?MaKho=" + MaKho + "&Thang=" + Thang + "&Nam=" + Nam;
            string data = CRUD.GetJsonData(url);
            return JsonConvert.DeserializeObject<List<DuDauVatTu>>(data).OrderBy(item => item.Ngay).ToList();
        }
        public List<DataNXT> GetListNhap(string MaKho, int Thang, int Nam)
        {
            string url = "?MaKho=" + MaKho + "&Thang=" + Thang + "&Nam=" + Nam;
            string dataNhap = CRUD.GetJsonData("ct_phieunhap/allctpnthang" + url);
            return JsonConvert.DeserializeObject<List<DataNXT>>(dataNhap);
        }
        public List<DataNXT> GetListXuat(string MaKho, int Thang, int Nam)
        {
            string url = "?MaKho=" + MaKho + "&Thang=" + Thang + "&Nam=" + Nam;
            string dataXuat = CRUD.GetJsonData("ct_phieuxuat/allctpxthang" + url);
            return JsonConvert.DeserializeObject<List<DataNXT>>(dataXuat);
        }
        public List<NhapXuatTon> GetListNXT(string MaKho, int Thang, int Nam)
        {
            var listTonKho = GetTonKho(MaKho, Thang, Nam);
            var listNhap = GetListNhap(MaKho, Thang, Nam);
            var listXuat = GetListXuat(MaKho, Thang, Nam);
            //Lấy ra tất cả vật tư có trong tháng cần báo cáo
            Dictionary<string, VatTuDetail> listNXT = new Dictionary<string, VatTuDetail>();
            foreach (var item in listTonKho)
                if (!listNXT.ContainsKey(item.MaVT))
                    listNXT.Add(item.MaVT, GetDetailVT(item.MaVT));
            foreach (var item in listNhap)
                if (!listNXT.ContainsKey(item.MaVT))
                    listNXT.Add(item.MaVT, GetDetailVT(item.MaVT));
            foreach (var item in listXuat)
                if (!listNXT.ContainsKey(item.MaVT))
                    listNXT.Add(item.MaVT, GetDetailVT(item.MaVT));
            var list = new List<NhapXuatTon>();
            foreach(var item in listNXT)
            {
                var row = new NhapXuatTon();
                row.MaVT = item.Key;
                row.TenVT = item.Value.TenVT;
                row.TenDVT = item.Value.TenDVT;
                //get tonkho
                foreach(var tonkho in listTonKho)
                {
                    if (tonkho.MaVT == item.Key)
                    {
                        row.TonDauKy = new KeyValuePair<double, decimal>(tonkho.SoLuong, tonkho.ThanhTien);
                        break;
                    }
                }
                if (row.TonDauKy.Equals(default(KeyValuePair<double, decimal>)))
                    row.TonDauKy = new KeyValuePair<double, decimal>(0, 0);
                foreach (var nhap in listNhap)
                {
                    if (nhap.MaVT == item.Key)
                    {
                        row.Nhap = new KeyValuePair<double, decimal>(nhap.TongSL, nhap.TongTT);
                        break;
                    }
                }
                if (row.Nhap.Equals(default(KeyValuePair<double, decimal>)))
                    row.Nhap = new KeyValuePair<double, decimal>(0, 0);
                foreach (var xuat in listXuat)
                {
                    if (xuat.MaVT == item.Key)
                    {
                        row.Xuat = new KeyValuePair<double, decimal>(xuat.TongSL, xuat.TongTT);
                        break;
                    }
                }
                if (row.Xuat.Equals(default(KeyValuePair<double, decimal>)))
                    row.Xuat = new KeyValuePair<double, decimal>(0, 0);
                double SLTonCK = row.TonDauKy.Key + row.Nhap.Key - row.Xuat.Key;
                decimal TTTonCK = row.TonDauKy.Value + row.Nhap.Value - row.Xuat.Value;
                row.TonCuoiKy = new KeyValuePair<double, decimal>(SLTonCK, TTTonCK);
                list.Add(row);
            }
            return list;
        }
        public void ExportBaoCaoNXT()
        {
            var data = GetListNXT(selectedMaKho, selectedMonth, selectedYear);
            using (SaveFileDialog sfd = new SaveFileDialog() { FileName = "Báo cáo nhập xuất tồn", Filter = "Word Document | *.docx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Application app = new Application();
                    app.Visible = false;
                    object missing = System.Reflection.Missing.Value;
                    object oEndOfDoc = "\\endofdoc";
                    Document document = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                    document.Content.Font.Name = "Times New Roman";
                    document.Content.Font.Size = 11;
                    document.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
                    document.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
                    document.PageSetup.TopMargin = app.InchesToPoints(0.5f);
                    document.PageSetup.BottomMargin = app.InchesToPoints(0.5f);
                    document.PageSetup.LeftMargin = app.InchesToPoints(0.5f);
                    document.PageSetup.RightMargin = app.InchesToPoints(0.5f);

                    float PageWidth = document.PageSetup.PageWidth - document.PageSetup.LeftMargin - document.PageSetup.RightMargin;

                    Range wordRange = document.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    Table HeaderTable = document.Tables.Add(wordRange, 1, 2);
                    HeaderTable.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;
                    HeaderTable.Columns[1].Width = 0.62f * PageWidth;
                    HeaderTable.Columns[2].Width = PageWidth - HeaderTable.Columns[1].Width;

                    Range col1Range = HeaderTable.Cell(1, 1).Range;
                    col1Range.Text = "Đơn vị:......\vĐịa chỉ:......";
                    col1Range.Font.Bold = 1;

                    object oCollapseStart = WdCollapseDirection.wdCollapseStart;
                    object oCollapseEnd = WdCollapseDirection.wdCollapseEnd;
                    Range col2Range = HeaderTable.Cell(1, 2).Range;
                    col2Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    col2Range.Collapse(ref oCollapseStart);
                    col2Range.Text = "Mẫu số B07 - H\v";
                    col2Range.Font.Bold = 1;
                    col2Range.Collapse(oCollapseEnd);

                    Paragraph Title = document.Content.Paragraphs.Add(ref missing);
                    Title.Range.Text = "BÁO CÁO NHẬP XUẤT TỒN";
                    Title.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    Title.Range.Font.Size = 13;
                    Title.Range.Font.Bold = 1;
                    Title.SpaceAfter = 0;
                    Title.Range.InsertParagraphAfter();

                    Paragraph Date = document.Content.Paragraphs.Add(ref missing);
                    Date.Range.Text = "Tháng:\t" + selectedMonth + "\t\tNăm:\t" + selectedYear;
                    Date.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    Date.Range.Font.Size = 11;
                    Date.Range.Font.Bold = 1;
                    Date.Range.Font.Italic = 1;
                    Date.Range.InsertParagraphAfter();


                    Paragraph Kho = document.Content.Paragraphs.Add(ref missing);
                    Kho.Range.Text = "Mã kho:\t" + selectedMaKho + "\t\tTên kho:\t" + GetTenKho();
                    Kho.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    Kho.Range.Font.Italic = 0;
                    Kho.Range.Font.Bold = 0;
                    Kho.Range.ParagraphFormat.SpaceAfter = 6;
                    Kho.Range.InsertParagraphAfter();

                    Table MainTable = document.Tables.Add(Kho.Range, 2 + data.Count, 12);
                    MainTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    MainTable.Range.ParagraphFormat.SpaceAfter = 0;
                    MainTable.Borders.Enable = 1;
                    MainTable.Range.Font.Italic = 0;

                    MainTable.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;

                    MainTable.Columns[1].Width = 0.07f * PageWidth;
                    MainTable.Cell(1, 1).Range.Text = "STT";

                    MainTable.Columns[2].Width = 0.095f * PageWidth;
                    MainTable.Cell(1, 2).Range.Text = "Mã vật tư";
                   
                    MainTable.Columns[4].Width = 0.07f * PageWidth;
                    MainTable.Cell(1, 4).Range.Text = "Đơn vị tính";
                    
                    MainTable.Cell(1, 5).Range.Text = "Tồn đầu kỳ";
                    MainTable.Cell(2, 5).Range.Text = "Số lượng";
                    MainTable.Columns[5].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 6).Range.Text = "Thành tiền";
                    MainTable.Columns[6].Width = 0.095f * PageWidth;

                    MainTable.Cell(1, 7).Range.Text = "Nhập trong kỳ";
                    MainTable.Cell(2, 7).Range.Text = "Số lượng";
                    MainTable.Columns[7].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 8).Range.Text = "Thành tiền";
                    MainTable.Columns[8].Width = 0.095f * PageWidth;

                    MainTable.Cell(1, 9).Range.Text = "Xuất trong kỳ";
                    MainTable.Cell(2, 9).Range.Text = "Số lượng";
                    MainTable.Columns[9].Width =  0.07f * PageWidth;
                    MainTable.Cell(2, 10).Range.Text = "Thành tiền";
                    MainTable.Columns[10].Width = 0.095f * PageWidth;

                    MainTable.Cell(1, 11).Range.Text = "Tồn cuối kỳ";
                    MainTable.Cell(2, 11).Range.Text = "Số lượng";
                    MainTable.Columns[11].Width =  0.07f * PageWidth;
                    MainTable.Cell(2, 12).Range.Text = "Thành tiền";
                    MainTable.Columns[12].Width = 0.095f * PageWidth;

                    MainTable.Columns[3].Width = 0.11f * PageWidth;
                    MainTable.Cell(1, 3).Range.Text = "Tên vật tư";

                    MainTable.Cell(1, 1).Merge(MainTable.Cell(2, 1));
                    MainTable.Cell(1, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 2).Merge(MainTable.Cell(2, 2));
                    MainTable.Cell(1, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 3).Merge(MainTable.Cell(2, 3));
                    MainTable.Cell(1, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


                    MainTable.Cell(1, 4).Merge(MainTable.Cell(2, 4));
                    MainTable.Cell(1, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 5).Merge(MainTable.Cell(1, 6));

                    MainTable.Cell(1, 6).Merge(MainTable.Cell(1, 7));

                    MainTable.Cell(1, 7).Merge(MainTable.Cell(1, 8));

                    MainTable.Cell(1, 8).Merge(MainTable.Cell(1, 9));

                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                    nfi.CurrencyDecimalSeparator = ",";
                    nfi.CurrencyGroupSeparator = ".";
                    nfi.CurrencySymbol = "";


                    for (int i = 0; i < data.Count; i++)
                    {
                        for (int j = 1; j <= 11; j++)
                            MainTable.Cell(i + 5, j).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        //STT
                        MainTable.Cell(i + 3, 1).Range.Text = (i + 1).ToString();
                        //Mã VT
                        MainTable.Cell(i + 3, 2).Range.Text = data[i].MaVT;
                        //Tên VT
                        MainTable.Cell(i + 3, 3).Range.Text = data[i].TenVT;
                        MainTable.Cell(i + 3, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        //Tên DVT
                        MainTable.Cell(i + 3, 4).Range.Text = data[i].TenDVT;
                        //Tồn đầu kỳ
                        MainTable.Cell(i + 3, 5).Range.Text = data[i].TonDauKy.Key.ToString(); 
                        MainTable.Cell(i + 3, 6).Range.Text = data[i].TonDauKy.Value.ToString("C0", nfi);
                        MainTable.Cell(i + 3, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        //Nhập trong kỳ
                        MainTable.Cell(i + 3, 7).Range.Text = data[i].Nhap.Key.ToString();
                        MainTable.Cell(i + 3, 8).Range.Text = data[i].Nhap.Value.ToString("C0", nfi);
                        MainTable.Cell(i + 3, 8).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                        //Xuất trong kỳ
                        MainTable.Cell(i + 3, 9).Range.Text = data[i].Xuat.Key.ToString();
                        MainTable.Cell(i + 3, 10).Range.Text = data[i].Xuat.Value.ToString("C0", nfi);
                        MainTable.Cell(i + 3, 10).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        //Tồn cuối kỳ
                        MainTable.Cell(i + 3, 11).Range.Text = data[i].TonCuoiKy.Key.ToString();
                        MainTable.Cell(i + 3, 12).Range.Text = data[i].TonCuoiKy.Value.ToString("C0", nfi);
                        MainTable.Cell(i + 3, 12).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                    }

                    Paragraph Last = document.Content.Paragraphs.Add(ref missing);
                    Last.Range.Text = "";
                    Last.LeftIndent = 0;
                    Last.SpaceBefore = 0;
                    Last.Range.InsertParagraphAfter();

                    Range wordRange2 = document.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    Table SignTable = document.Tables.Add(wordRange2, 1, 3);
                    SignTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    SignTable.Range.Font.Italic = 0;
                    Range col1 = SignTable.Cell(1, 1).Range;
                    col1.Collapse(ref oCollapseStart);
                    col1.Text = "\vNgười ghi sổ\v";
                    col1.Font.Bold = 1;
                    col1.Collapse(oCollapseEnd);
                    col1.Text = "(Ký, họ tên)";
                    col1.Font.Italic = 1;

                    Range col2 = SignTable.Cell(1, 2).Range;
                    col2.Collapse(ref oCollapseStart);
                    col2.Text = "\vKế toán trưởng\v";
                    col2.Font.Bold = 1;
                    col2.Collapse(oCollapseEnd);
                    col2.Text = "(Ký, họ tên)";
                    col2.Font.Italic = 1;

                    Range col3 = SignTable.Cell(1, 3).Range;
                    col3.Collapse(ref oCollapseStart);
                    col3.Text = "Ngày .. tháng .. năm ....\v";
                    col3.Font.Italic = 1;
                    col3.Collapse(oCollapseEnd);
                    col3.Text = "Giám đốc\v";
                    col3.Font.Bold = 1;
                    col3.Collapse(oCollapseEnd);
                    col3.Text = "(Ký, họ tên)";
                    col3.Font.Italic = 1;

                    document.SaveAs2(sfd.FileName);
                    document.Close(ref missing, ref missing, ref missing);
                    document = null;
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                    Process.Start(sfd.FileName);
                }
            }
        }

    }
}
