
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
    public class LapSoChiTietViewModel : BaseViewModel
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
        private string _selectedMaVT;
        public string selectedMaVT
        {
            get => _selectedMaVT;
            set => SetProperty(ref _selectedMaVT, value);
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
        public LapSoChiTietViewModel()
        {
            selectedMaVT = selectedMaKho = string.Empty;
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
                ExportSoChiTietVatTu();
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
        public VatTuDetail GetDetailVT()
        {
            foreach (var item in ListVT)
            {
                if (item.MaVT == selectedMaVT)
                    return item;
            }
            return null;
        }
        public DuDauVatTu GetTonKho(string MaVT, string MaKho, int Thang, int Nam)
        {
            string url = "dudauvattu/tonkhothang?MaVT=" + MaVT + "&MaKho=" + MaKho + "&Thang=" + Thang + "&Nam=" + Nam;
            string data = CRUD.GetJsonData(url);
            var list = JsonConvert.DeserializeObject<ObservableCollection<DuDauVatTu>>(data);
            if (list.Count > 0)
                return list[0];
            return null;
        }
        public List<DataNhapXuat> GetListNhapXuat(string MaVT, string MaKho, DateTime NgayBD, int Thang, int Nam)
        {
            string url = "?MaVT=" + MaVT + "&MaKho=" + MaKho + "&NgayBD=" + NgayBD.ToString("yyyy-MM-dd") + "&Thang=" + Thang + "&Nam=" + Nam;
            string dataNhap = CRUD.GetJsonData("ct_phieunhap/ctpnthang" + url);
            var listNhap = JsonConvert.DeserializeObject<List<DataNhapXuat>>(dataNhap);
            string dataXuat = CRUD.GetJsonData("ct_phieuxuat/ctpxthang" + url);
            var listXuat = JsonConvert.DeserializeObject<List<DataNhapXuat>>(dataXuat);
            listNhap.AddRange(listXuat);
            return listNhap.OrderBy(item => item.Ngay).ToList();
        }
        public void ExportSoChiTietVatTu()
        {
            VatTuDetail detailVT = GetDetailVT();
            var TonKho = GetTonKho(detailVT.MaVT, selectedMaKho, selectedMonth, selectedYear);
            DateTime NgayBD = TonKho == null ? DateTime.Parse("01/01/0001") : TonKho.Ngay;
            var ListNX = GetListNhapXuat(detailVT.MaVT, selectedMaKho, NgayBD, selectedMonth, selectedYear);
            using (SaveFileDialog sfd = new SaveFileDialog() { FileName = "Sổ chi tiết vật tư", Filter = "Word Document | *.docx", ValidateNames = true })
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
                    col2Range.Text = "Mẫu số S10-DN\v";
                    col2Range.Font.Bold = 1;
                    col2Range.Collapse(oCollapseEnd);
                    col2Range.Text = "(Ban hành theo Thông tư số 200/2014/TT-BTC\vngày 22/12/2014 của Bộ Tài chính)";


                    Paragraph Title = document.Content.Paragraphs.Add(ref missing);
                    Title.Range.Text = "SỔ CHI TIẾT VẬT LIỆU, DỤNG CỤ (SẢN PHẨM, HÀNG HÓA)";
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


                    Paragraph TaiKhoan = document.Content.Paragraphs.Add(ref missing);
                    TaiKhoan.Range.Text = "Tài khoản:\t" + detailVT.MaTK + "\t\tTên kho:\t" + GetTenKho();
                    TaiKhoan.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    Date.Range.Font.Italic = 0;
                    TaiKhoan.Range.InsertParagraphAfter();

                    Paragraph Name = document.Content.Paragraphs.Add(ref missing);
                    Name.Range.Text = "Tên, quy cách nguyên liệu, vật liệu, công cụ, dụng cụ (sản phẩm, hàng hoá):\t" + detailVT.TenVT;
                    Name.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    Name.Range.Font.Bold = 0;
                    Name.Range.InsertParagraphAfter();

                    Paragraph DonViTinh = document.Content.Paragraphs.Add(ref missing);
                    DonViTinh.Range.Text = "Đơn vị tính:\t" + detailVT.TenDVT;
                    DonViTinh.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    DonViTinh.Range.Font.Italic = 1;
                    DonViTinh.LeftIndent = app.CentimetersToPoints(21);
                    DonViTinh.SpaceAfter = 12;
                    DonViTinh.Range.InsertParagraphAfter();

                    Table MainTable = document.Tables.Add(DonViTinh.Range, 5 + ListNX.Count, 12);
                    MainTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    MainTable.Range.ParagraphFormat.SpaceAfter = 0;
                    MainTable.Borders.Enable = 1;
                    MainTable.Range.Font.Italic = 0;

                    MainTable.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;
                    MainTable.Cell(1, 1).Range.Text = "Chứng từ";
                    //Số hiệu
                    MainTable.Columns[1].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 1).Range.Text = "Số hiệu";
                    //Ngày tháng
                    MainTable.Columns[2].Width = 0.09f * PageWidth;
                    MainTable.Cell(2, 2).Range.Text = "Ngày, tháng";
                    //Diễn giải
                    MainTable.Columns[3].Width = 0.13f * PageWidth;
                    MainTable.Cell(1, 3).Range.Text = "Diễn giải";
                    //TK đối ứng
                    MainTable.Columns[4].Width =  0.075f * PageWidth;
                    MainTable.Cell(1, 4).Range.Text = "Tài khoản đối ứng";
                    //Đơn giá
                    MainTable.Columns[5].Width = 0.07f * PageWidth;
                    MainTable.Cell(1, 5).Range.Text = "Đơn giá";

                    //Nhập
                    MainTable.Cell(1, 6).Range.Text = "Nhập";
                    MainTable.Columns[6].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 6).Range.Text = "Số lượng";
                    MainTable.Columns[7].Width = 0.095f * PageWidth;
                    MainTable.Cell(2, 7).Range.Text = "Thành tiền";

                    //Xuất
                    MainTable.Cell(1, 8).Range.Text = "Xuất";
                    MainTable.Columns[8].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 8).Range.Text = "Số lượng";
                    MainTable.Columns[9].Width = 0.095f * PageWidth;
                    MainTable.Cell(2, 9).Range.Text = "Thành tiền";

                    //Tồn
                    MainTable.Cell(1, 10).Range.Text = "Tồn";
                    MainTable.Columns[10].Width = 0.07f * PageWidth;
                    MainTable.Cell(2, 10).Range.Text = "Số lượng";
                    MainTable.Columns[11].Width = 0.095f * PageWidth;
                    MainTable.Cell(2, 11).Range.Text = "Thành tiền";

                    //Ghi chú
                    MainTable.Columns[12].Width = 0.07f * PageWidth;
                    MainTable.Cell(1, 12).Range.Text = "Ghi chú";

                    MainTable.Cell(1, 3).Merge(MainTable.Cell(2, 3));
                    MainTable.Cell(1, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 4).Merge(MainTable.Cell(2, 4));
                    MainTable.Cell(1, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 5).Merge(MainTable.Cell(2, 5));
                    MainTable.Cell(1, 5).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 12).Merge(MainTable.Cell(2, 12));
                    MainTable.Cell(1, 12).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 1).Merge(MainTable.Cell(1, 2));
                    MainTable.Cell(1, 5).Merge(MainTable.Cell(1, 6));
                    MainTable.Cell(1, 6).Merge(MainTable.Cell(1, 7));
                    MainTable.Cell(1, 7).Merge(MainTable.Cell(1, 8));


                    MainTable.Cell(3, 1).Range.Text = "A";
                    MainTable.Cell(3, 2).Range.Text = "B";
                    MainTable.Cell(3, 3).Range.Text = "C";
                    MainTable.Cell(3, 4).Range.Text = "D";
                    MainTable.Cell(3, 5).Range.Text = "1";
                    MainTable.Cell(3, 6).Range.Text = "2";
                    MainTable.Cell(3, 7).Range.Text = "3 = 1 x 2";
                    MainTable.Cell(3, 8).Range.Text = "4";
                    MainTable.Cell(3, 9).Range.Text = "5 = 1 x 4";
                    MainTable.Cell(3, 10).Range.Text = "6";
                    MainTable.Cell(3, 11).Range.Text = "7 = 1 x 6";
                    MainTable.Cell(3, 12).Range.Text = "8";

                    MainTable.Cell(4, 3).Range.Text = "Số dư đầu kỳ";
                    MainTable.Cell(4, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;


                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                    nfi.CurrencyDecimalSeparator = ",";
                    nfi.CurrencyGroupSeparator = ".";
                    nfi.CurrencySymbol = "";

                    MainTable.Cell(4, 5).Range.Text = TonKho != null ? TonKho.DonGia.ToString("C0", nfi) : 0.ToString();
                    MainTable.Cell(4, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    MainTable.Cell(4, 10).Range.Text = TonKho != null ? TonKho.SoLuong.ToString() : 0.ToString();

                    MainTable.Cell(4, 11).Range.Text = TonKho != null ? TonKho.ThanhTien.ToString("C0", nfi) : 0.ToString();
                    MainTable.Cell(4, 11).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    double SLNhapCK = 0;
                    decimal TongTienNhapCK = 0;
                    double SLXuatCK = 0;
                    decimal TongTienXuatCK = 0;
                    double SLTon = TonKho != null ? TonKho.SoLuong : 0;
                    for (int i = 0; i < ListNX.Count; i++)
                    {
                        for (int j = 1; j <= 11; j++)
                            MainTable.Cell(i + 5, j).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        //Số phiếu
                        MainTable.Cell(i + 5, 1).Range.Text = ListNX[i].SoPhieu;
                        //Ngày tháng
                        MainTable.Cell(i + 5, 2).Range.Text = ListNX[i].Ngay.ToString("dd/MM/yyyy");
                        //Diễn giải
                        MainTable.Cell(i + 5, 3).Range.Text = ListNX[i].LyDo;
                        MainTable.Cell(i + 5, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        //TK đối ứng
                        MainTable.Cell(i + 5, 4).Range.Text = ListNX[i].MaTK;
                        //Đơn giá
                        MainTable.Cell(i + 5, 5).Range.Text = ListNX[i].DonGia.ToString("C0", nfi);
                        MainTable.Cell(i + 5, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                        double sl = ListNX[i].SLThucTe;
                        decimal tt = ListNX[i].ThanhTien;
                        if (ListNX[i].SoPhieu.Contains("N"))
                        {
                            MainTable.Cell(i + 5, 6).Range.Text = sl.ToString();

                            MainTable.Cell(i + 5, 7).Range.Text = tt.ToString("C0", nfi);
                            MainTable.Cell(i + 5, 7).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                            SLTon += ListNX[i].SLThucTe;
                            SLNhapCK += sl;
                            TongTienNhapCK += tt;
                        }
                        else
                        {
                            MainTable.Cell(i + 5, 8).Range.Text = sl.ToString();

                            MainTable.Cell(i + 5, 9).Range.Text = tt.ToString("C0", nfi);
                            MainTable.Cell(i + 5, 9).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                            SLTon -= ListNX[i].SLThucTe;
                            SLXuatCK += sl;
                            TongTienXuatCK += tt;
                        }
                        MainTable.Cell(i + 5, 10).Range.Text = SLTon.ToString();

                        MainTable.Cell(i + 5, 11).Range.Text = ((decimal)SLTon * ListNX[i].DonGia).ToString("C0", nfi);
                        MainTable.Cell(i + 5, 11).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    }

                    int n = 5 + ListNX.Count;
                    for (int i = 1; i <= 11; i++)
                        MainTable.Cell(n, i).Range.Font.Bold = 1;

                    MainTable.Cell(n, 3).Range.Text = "Cộng tháng";
                    MainTable.Cell(n, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                    MainTable.Cell(n, 4).Range.Text = "x";

                    MainTable.Cell(n, 5).Range.Text = "x";

                    MainTable.Cell(n, 6).Range.Text = SLNhapCK.ToString();

                    MainTable.Cell(n, 7).Range.Text = TongTienNhapCK.ToString("C0", nfi);
                    MainTable.Cell(n, 7).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    MainTable.Cell(n, 8).Range.Text = SLXuatCK.ToString();

                    MainTable.Cell(n, 9).Range.Text = TongTienXuatCK.ToString("C0", nfi);
                    MainTable.Cell(n, 9).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    MainTable.Cell(n, 10).Range.Text = SLTon.ToString();

                    MainTable.Cell(n, 11).Range.Text = ListNX.Count > 0 ? ((decimal)SLTon * ListNX[ListNX.Count - 1].DonGia).ToString("C0", nfi) : 0.ToString();
                    MainTable.Cell(n, 11).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;


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
                    col3.Text = "(Ký, họ tên, đóng dấu)";
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
