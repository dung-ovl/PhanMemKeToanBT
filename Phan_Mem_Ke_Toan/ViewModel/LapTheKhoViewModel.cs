
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
    public class LapTheKhoViewModel : BaseViewModel
    {
        private DateTime _selectedNgayLap;
        public DateTime selectedNgayLap
        {
            get => _selectedNgayLap;
            set => SetProperty(ref _selectedNgayLap, value);
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

        public ICommand ExportCommand { get; set; }
        public LapTheKhoViewModel()
        {
            selectedMaVT = selectedMaKho = string.Empty;
            GetListVatTu();
            GetListKho();
            selectedNgayLap = DateTime.Now;
            ExportCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                ExportTheKho();
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
        public DuDauVatTu GetTonKho(string MaVT, string MaKho, DateTime NgayLap)
        {
            string url = "dudauvattu/tonkho?MaVT=" + MaVT + "&MaKho=" + MaKho + "&NgayLap=" + NgayLap.ToString("yyyy-MM-dd");
            string data = CRUD.GetJsonData(url);
            var list = JsonConvert.DeserializeObject<ObservableCollection<DuDauVatTu>>(data);
            if (list.Count > 0)
                return list[0];
            return null;
        }
        public List<DataNhapXuat> GetListNhapXuat(string MaVT, string MaKho, DateTime NgayBD, DateTime NgayKT)
        {
            string url = "?MaVT=" + MaVT + "&MaKho=" + MaKho + "&NgayBD=" + NgayBD.ToString("yyyy-MM-dd") + "&NgayKT=" + NgayKT.ToString("yyyy-MM-dd");
            string dataNhap = CRUD.GetJsonData("ct_phieunhap/ctpn" + url);
            var listNhap = JsonConvert.DeserializeObject<List<DataNhapXuat>>(dataNhap);
            string dataXuat = CRUD.GetJsonData("ct_phieuxuat/ctpx" + url);
            var listXuat = JsonConvert.DeserializeObject<List<DataNhapXuat>>(dataXuat);
            listNhap.AddRange(listXuat);
            return listNhap.OrderBy(item => item.Ngay).ToList();
        }
        public void ExportTheKho()
        {
            string Ngay = selectedNgayLap.ToString("dd");
            string Thang = selectedNgayLap.ToString("MM");
            string Nam = selectedNgayLap.ToString("yyyy");
            VatTuDetail detailVT = GetDetailVT();
            var TonKho = GetTonKho(detailVT.MaVT, selectedMaKho, selectedNgayLap);
            DateTime NgayBD = TonKho == null ? DateTime.Parse("01/01/0001") : TonKho.Ngay;
            var ListNX = GetListNhapXuat(detailVT.MaVT, selectedMaKho, NgayBD, selectedNgayLap);
            using (SaveFileDialog sfd = new SaveFileDialog() { FileName = "Thẻ kho", Filter = "Word Document | *.docx", ValidateNames = true })
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
                    col2Range.Text = "Mẫu số S12-DN\v";
                    col2Range.Font.Bold = 1;
                    col2Range.Collapse(oCollapseEnd);
                    col2Range.Text = "(Ban hành theo Thông tư số 200/2014/TT-BTC\vngày 22/12/2014 của Bộ Tài chính)";


                    Paragraph Title = document.Content.Paragraphs.Add(ref missing);
                    Title.Range.Text = "THẺ KHO (SỔ KHO)";
                    Title.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    Title.Range.Font.Size = 13;
                    Title.Range.Font.Bold = 1;
                    Title.Range.InsertParagraphAfter();

                    Paragraph Date = document.Content.Paragraphs.Add(ref missing);
                    Date.Range.Text = "Ngày lập thẻ: " + selectedNgayLap.ToString("dd/MM/yyyy") + "\vTờ số: 01";
                    Date.Range.Font.Size = 11;
                    Date.Range.Font.Bold = 0;
                    Date.Range.InsertParagraphAfter();


                    Paragraph Detail = document.Content.Paragraphs.Add(ref missing);
                    Detail.Range.Text = "\t- Tên, nhãn hiệu, quy cách vật tư:\t" + detailVT.TenVT +
                                        "\v\t- Đơn vị tính:\t" + detailVT.TenDVT +
                                        "\v\t- Mã số:\t" + detailVT.MaVT;
                    Detail.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    Detail.Range.InsertParagraphAfter();

                    Table MainTable = document.Tables.Add(Detail.Range, 6 + ListNX.Count, 10);
                    MainTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    MainTable.Range.ParagraphFormat.SpaceAfter = 0;
                    MainTable.Borders.Enable = 1;

                    MainTable.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;
                    //STT
                    MainTable.Columns[1].Width = 0.055f * PageWidth;
                    MainTable.Cell(1, 1).Range.Text = "STT";
                    //Ngày tháng
                    MainTable.Columns[2].Width = 0.109f * PageWidth;
                    MainTable.Cell(1, 2).Range.Text = "Ngày, tháng";

                    MainTable.Cell(1, 3).Range.Text = "Số hiệu chứng từ";
                    //Nhập
                    MainTable.Columns[3].Width = 0.073f * PageWidth;
                    MainTable.Cell(2, 3).Range.Text = "Nhập";
                    //Xuất
                    MainTable.Columns[4].Width = 0.073f * PageWidth;
                    MainTable.Cell(2, 4).Range.Text = "Xuất";
                    //Diễn giải
                    MainTable.Columns[5].Width = 0.182f * PageWidth;
                    MainTable.Cell(1, 5).Range.Text = "Diễn giải";
                    //Ngày nhập, xuất
                    MainTable.Columns[6].Width = 0.109f * PageWidth;
                    MainTable.Cell(1, 6).Range.Text = "Ngày nhập, xuất";
                    //Nhập - xuất - tồn
                    MainTable.Columns[7].Width = 0.073f * PageWidth;
                    MainTable.Columns[8].Width = 0.073f * PageWidth;
                    MainTable.Columns[9].Width = 0.073f * PageWidth;
                    MainTable.Cell(1, 7).Range.Text = "Số lượng";
                    MainTable.Cell(2, 7).Range.Text = "Nhập";
                    MainTable.Cell(2, 8).Range.Text = "Xuất";
                    MainTable.Cell(2, 9).Range.Text = "Tồn";
                    //Ký xác nhận
                    MainTable.Columns[10].Width = 0.18f * PageWidth;
                    MainTable.Cell(1, 10).Range.Text = "Ký xác nhận của kế toán";

                    MainTable.Cell(1, 1).Merge(MainTable.Cell(2, 1));
                    MainTable.Cell(1, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 2).Merge(MainTable.Cell(2, 2));
                    MainTable.Cell(1, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 5).Merge(MainTable.Cell(2, 5));
                    MainTable.Cell(1, 5).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 6).Merge(MainTable.Cell(2, 6));
                    MainTable.Cell(1, 6).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 10).Merge(MainTable.Cell(2, 10));
                    MainTable.Cell(1, 10).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    MainTable.Cell(1, 3).Merge(MainTable.Cell(1, 4));
                    MainTable.Cell(1, 6).Merge(MainTable.Cell(1, 7));
                    MainTable.Cell(1, 6).Merge(MainTable.Cell(1, 7));

                    MainTable.Cell(3, 1).Range.Text = "A";
                    MainTable.Cell(3, 2).Range.Text = "B";
                    MainTable.Cell(3, 3).Range.Text = "C";
                    MainTable.Cell(3, 4).Range.Text = "D";
                    MainTable.Cell(3, 5).Range.Text = "E";
                    MainTable.Cell(3, 6).Range.Text = "F";
                    MainTable.Cell(3, 7).Range.Text = "1";
                    MainTable.Cell(3, 8).Range.Text = "2";
                    MainTable.Cell(3, 9).Range.Text = "3";
                    MainTable.Cell(3, 10).Range.Text = "G";
                    for (int i = 1; i <= 9; i++)
                        MainTable.Cell(4, 1).Merge(MainTable.Cell(4, 2));
                    MainTable.Cell(4, 1).Range.Text = "Mã kho: " + selectedMaKho + "\t\tTên kho: " + GetTenKho();
                    MainTable.Cell(4, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    MainTable.Cell(4, 1).Range.Font.Bold = 1;
                    MainTable.Cell(4, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                    MainTable.Cell(5, 5).Range.Text = "Số dư đầu kỳ";
                    MainTable.Cell(5, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    MainTable.Cell(5, 9).Range.Text = TonKho != null ? TonKho.SoLuong.ToString() : 0.ToString();


                    double NhapCK = 0;
                    double XuatCK = 0;
                    double ton = TonKho != null ? TonKho.SoLuong : 0;
                    for (int i = 0; i < ListNX.Count; i++)
                    {
                        for (int j = 1; j <= 10; j++)
                            MainTable.Cell(i + 6, j).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        //STT
                        MainTable.Cell(i + 6, 1).Range.Text = (i + 1).ToString();

                        //Ngày tháng
                        MainTable.Cell(i + 6, 2).Range.Text = ListNX[i].Ngay.ToString("dd/MM/yyyy");

                        //Nhập hoặc xuất
                        string sp = ListNX[i].SoPhieu;
                        double sl = ListNX[i].SLThucTe;
                        if (sp.Contains("N"))
                        {
                            MainTable.Cell(i + 6, 3).Range.Text = sp;
                            //SL nhập
                            MainTable.Cell(i + 6, 7).Range.Text = sl.ToString();
                            //Tồn
                            ton += sl;                         
                            NhapCK += sl;
                        }
                        else
                        {
                            MainTable.Cell(i + 6, 4).Range.Text = sp;
                            //SL xuất
                            MainTable.Cell(i + 6, 8).Range.Text = sl.ToString();
                            //Tồn
                            ton -= sl;
                            XuatCK += sl;
                        }
                        MainTable.Cell(i + 6, 9).Range.Text = ton.ToString();

                        //Diễn giải
                        MainTable.Cell(i + 6, 5).Range.Text = ListNX[i].LyDo;
                        MainTable.Cell(i + 6, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        //Ngày nhập, xuất
                        MainTable.Cell(i + 6, 6).Range.Text = ListNX[i].Ngay.ToString("dd/MM/yyyy");
                    }

                    int n = 6 + ListNX.Count;
                    MainTable.Cell(n, 5).Range.Text = "Cộng cuối kỳ";
                    MainTable.Cell(n, 5).Range.Font.Bold = 1;
                    MainTable.Cell(n, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    MainTable.Cell(n, 7).Range.Text = NhapCK.ToString();
                    MainTable.Cell(n, 7).Range.Font.Bold = 1;
                    MainTable.Cell(n, 8).Range.Text = XuatCK.ToString();
                    MainTable.Cell(n, 8).Range.Font.Bold = 1;
                    MainTable.Cell(n, 9).Range.Text = ton.ToString();
                    MainTable.Cell(n, 9).Range.Font.Bold = 1;
        


                    Paragraph Last = document.Content.Paragraphs.Add(ref missing);
                    Last.Range.Text = "\t- Sổ này có 01 trang, đánh số từ trang 01 đến trang 01\v\t- Ngày mở sổ: " + selectedNgayLap.ToString("dd/MM/yyyy");
                    Last.SpaceBefore = 6.0f;
                    Last.Range.InsertParagraphAfter();

                    Range wordRange2 = document.Bookmarks.get_Item(ref oEndOfDoc).Range;

                    Table SignTable = document.Tables.Add(wordRange2, 1, 3);
                    SignTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

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
                    col3.Text = "Ngày " + Ngay + " tháng " + Thang + " năm " + Nam + "\v";
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
