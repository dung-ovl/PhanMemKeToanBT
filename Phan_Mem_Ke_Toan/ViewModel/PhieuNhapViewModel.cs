using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.ValidRule;
using Phan_Mem_Ke_Toan.View;
using Phan_Mem_Ke_Toan.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using MessageBox = System.Windows.MessageBox;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class PhieuNhapViewModel : TableViewModel<PhieuNhapDetail>
    {
        private string _btnContent;
        public string BtnContent
        {
            get => _btnContent;
            set => SetProperty(ref _btnContent, value);
        }

        private string _txtSoPhieu;
        public string txtSoPhieu
        {
            get => _txtSoPhieu;
            set => SetProperty(ref _txtSoPhieu, value);
        }
        private DateTime _selectedNgayNhap;
        public DateTime selectedNgayNhap
        {
            get => _selectedNgayNhap;
            set => SetProperty(ref _selectedNgayNhap, value);
        }
        private string _selectedMaNguoiGiao;
        public string selectedMaNguoiGiao
        {
            get => _selectedMaNguoiGiao;
            set
            {
                SetProperty(ref _selectedMaNguoiGiao, value);
                foreach (var item in ListNguoiGiaoDetail)
                {
                    if (item.MaNguoiGiao == value)
                    {
                        selectedMaNCC = item.MaNCC;
                        txtTenNCC = item.TenNCC;
                    }
                }

            }
        }
        private string _selectedMaNCC;
        public string selectedMaNCC
        {
            get => _selectedMaNCC;
            set => SetProperty(ref _selectedMaNCC, value);
        }
        private string _txtTenNCC;
        public string txtTenNCC
        {
            get => _txtTenNCC;
            set => SetProperty(ref _txtTenNCC, value);
        }
        private string _selectedMaKho;
        public string selectedMaKho
        {
            get => _selectedMaKho;
            set => SetProperty(ref _selectedMaKho, value);
        }
        private string _txtChungTuLQ;
        public string txtChungTuLQ
        {
            get => _txtChungTuLQ;
            set => SetProperty(ref _txtChungTuLQ, value);
        }
        private string _txtLyDo;
        public string txtLyDo
        {
            get => _txtLyDo;
            set => SetProperty(ref _txtLyDo, value);
        }
        private string _selectedTKCo;
        public string selectedTKCo
        {
            get => _selectedTKCo;
            set => SetProperty(ref _selectedTKCo, value);
        }
        private decimal _txtTongTien;
        public decimal txtTongTien
        {
            get => _txtTongTien;
            set => SetProperty(ref _txtTongTien, value);
        }
        private ObservableCollection<NguoiGiaoDetail> _ListNguoiGiaoDetail;
        public ObservableCollection<NguoiGiaoDetail> ListNguoiGiaoDetail
        {
            get => _ListNguoiGiaoDetail;
            set => SetProperty(ref _ListNguoiGiaoDetail, value);
        }
        private ObservableCollection<Kho> _ListKho;
        public ObservableCollection<Kho> ListKho
        {
            get => _ListKho;
            set => SetProperty(ref _ListKho, value);
        }
        private ObservableCollection<TaiKhoan> _ListTK;
        public ObservableCollection<TaiKhoan> ListTK
        {
            get => _ListTK;
            set => SetProperty(ref _ListTK, value);
        }

        public ICommand ExportCommand { get; set; }
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddCommandCT { get; set; }
        public ICommand DeleteItemCommandCT { get; set; }


        private ObservableCollection<CT_PhieuNhapDetail> _listDataCT;
        public ObservableCollection<CT_PhieuNhapDetail> ListDataCT
        {
            get => _listDataCT;
            set => SetProperty(ref _listDataCT, value);
        }
        public IEnumerable<VatTuDetail> ListVTSelect
        {
            get
            {
                if (ListVT == null) return null;
                if (ListDataCT.Count == 0) return ListVT;
                return ListVT.Where(x =>
                {
                    foreach (var item in ListDataCT)
                    {
                        if (item.MaVT == x.MaVT) return false;
                    }
                    return true;
                });
            }
        }
        private ObservableCollection<VatTuDetail> _listVT;
        public ObservableCollection<VatTuDetail> ListVT
        {
            get => _listVT;
            set
            {
                SetProperty(ref _listVT, value);
                OnPropertyChanged("ListVTSelect");
            }
        }

        private decimal _txtThanhTien;
        public decimal txtThanhTien
        {
            get => _txtThanhTien;
            set => SetProperty(ref _txtThanhTien, value);
        }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                string text = value.Trim().ToLower();
                filter.AddFilter("Search", element =>
                {
                    PhieuNhapDetail item = element as PhieuNhapDetail;
                    return item.SoPhieu.ToLower().Contains(text);
                });
            }
        }

        private DateTime? _beginDate;
        public DateTime? BeginDate
        {
            get => _beginDate;
            set
            {
                SetProperty(ref _beginDate, value);
                FilterDate();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                FilterDate();
            }
        }

        public void FilterDate()
        {
            if (_endDate == null || _beginDate == null) return;
            filter.AddFilter("date", element =>
            {
                PhieuNhapDetail item = element as PhieuNhapDetail;
                return item.NgayNhap >= BeginDate && item.NgayNhap <= EndDate;
            });
        }
        public VatTuDetail selectedVT { get; set; }
        public PhieuNhapViewModel() : base("PhieuNhap")
        {
            ListDataCT = new ObservableCollection<CT_PhieuNhapDetail>();
        }

        public override void Event()
        {
            base.Event();
            ExportCommand = new RelayCommand<object>((p) => p != null, (p) =>
            {
                var selectedPhieuNhap = p as PhieuNhapDetail;
                GetListCT(selectedPhieuNhap.SoPhieu);
                if (ListDataCT.Count == 0)
                {
                    notify.updateDataFail("Chưa có dữ liệu chi tiết, không thể xuất file");
                    return;
                }
                if (selectedPhieuNhap.TongTien == 0)
                {
                    notify.updateDataFail("Yêu cầu tính giá xuất kho");
                    return;
                }
                ExportPhieuNhap(selectedPhieuNhap);
            });

            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadNewData();
            });

            ShowDetailCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var selectedPhieuNhap = p as PhieuNhapDetail;
                txtSoPhieu = selectedPhieuNhap.SoPhieu;
                txtTongTien = selectedPhieuNhap.TongTien;
                GetListCT(selectedPhieuNhap.SoPhieu);
                CT_PhieuNhapDialog dialog = new CT_PhieuNhapDialog();
                dialog.ShowDialog();
            });

            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadNewData();
                GetListVatTu();
                txtSoPhieu = ListData.Count() == 0 ? "PN001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].SoPhieu);
                BtnContent = "Xong";
                ClearTextboxValue();
                PhieuNhapDialog dialog = new PhieuNhapDialog();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as PhieuNhapDetail;
                GetListCT(itemData.SoPhieu);
                GetListVatTu();
                BtnContent = "Lưu";
                txtSoPhieu = itemData.SoPhieu;
                selectedNgayNhap = itemData.NgayNhap;
                selectedMaNguoiGiao = itemData.MaNguoiGiao;
                selectedMaKho = itemData.MaKho;
                txtChungTuLQ = itemData.ChungTuLQ;
                txtLyDo = itemData.LyDo;
                selectedTKCo = itemData.TKCo;
                txtTongTien = itemData.TongTien;
                PhieuNhapDialog dialog = new PhieuNhapDialog();
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                bool isSuccess = true;
                if (BtnContent == "Xong")
                {
                    PhieuNhap pn = new PhieuNhap
                    {
                        SoPhieu = txtSoPhieu,
                        NgayNhap = selectedNgayNhap.Date,
                        MaNguoiGiao = selectedMaNguoiGiao == "" ? null : selectedMaNguoiGiao,
                        MaNCC = selectedMaNCC == "" ? null : selectedMaNCC,
                        MaKho = selectedMaKho == "" ? null : selectedMaKho,
                        LyDo = txtLyDo,
                        TKCo = selectedTKCo == "" ? null : selectedTKCo,
                        ChungTuLQ = txtChungTuLQ,
                    };

                    if (CRUD.InsertData("PhieuNhap", pn))
                    {
                        foreach (var item in ListDataCT)
                        {
                            item.ThanhTien = (decimal)item.SLThucTe * item.DonGia;
                            CT_PhieuNhap ctpn = new CT_PhieuNhap
                            {
                                SoPhieu = txtSoPhieu,
                                MaVT = item.MaVT,
                                SLSoSach = item.SLSoSach,
                                SLThucTe = item.SLThucTe,
                                DonGia = item.DonGia,
                                ThanhTien = item.ThanhTien,
                            };
                            isSuccess = CRUD.InsertData("CT_PhieuNhap", ctpn);

                            if (!isSuccess) break;
                        }
                    }
                    else isSuccess = false;

                    if (isSuccess)
                    {
                        UpdateTongTienPN(pn);
                        LoadTableData();
                        notify.updateDataSuccess("Thêm phiếu nhập thành công");
                        if (pn.NgayNhap.Month == DateTime.Now.Month && pn.NgayNhap.Year == DateTime.Now.Year)
                            MainViewModel.Instance.GetListNhap();
                    }
                    else notify.updateDataFail();
                }
                else
                {
                    PhieuNhap pn = new PhieuNhap
                    {
                        SoPhieu = txtSoPhieu,
                        NgayNhap = selectedNgayNhap.Date,
                        MaNguoiGiao = selectedMaNguoiGiao == "" ? null : selectedMaNguoiGiao,
                        MaNCC = selectedMaNCC == "" ? null : selectedMaNCC,
                        MaKho = selectedMaKho == "" ? null : selectedMaKho,
                        LyDo = txtLyDo,
                        TKCo = selectedTKCo == "" ? null : selectedTKCo,
                        ChungTuLQ = txtChungTuLQ,
                    };

                    if (CRUD.UpdateData("PhieuNhap", pn))
                    {
                        if (CRUD.DeleteData("CT_PhieuNhap", txtSoPhieu))
                        {
                            foreach (var item in ListDataCT)
                            {
                                item.ThanhTien = (decimal)item.SLThucTe * item.DonGia;
                                CT_PhieuXuat ctpn = new CT_PhieuXuat
                                {
                                    SoPhieu = txtSoPhieu,
                                    MaVT = item.MaVT,
                                    SLSoSach = item.SLSoSach,
                                    SLThucTe = item.SLThucTe,
                                    DonGia = item.DonGia,
                                    ThanhTien = item.ThanhTien,
                                };
                                isSuccess = CRUD.InsertData("CT_PhieuNhap", ctpn);

                                if (!isSuccess) break;
                            }
                        }
                        else isSuccess = false;
                    }
                    else isSuccess = false;


                    if (isSuccess)
                    {
                        UpdateTongTienPN(pn);
                        LoadTableData();
                        notify.updateDataSuccess("Cập nhật phiếu nhập thành công");
                        if (pn.NgayNhap.Month == DateTime.Now.Month && pn.NgayNhap.Year == DateTime.Now.Year)
                            MainViewModel.Instance.GetListNhap();
                    }
                    else notify.updateDataFail();
                }
                 ((System.Windows.Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as PhieuNhapDetail;
                DeleteData(itemData.SoPhieu);
                if (itemData.NgayNhap.Month == DateTime.Now.Month && itemData.NgayNhap.Year == DateTime.Now.Year)
                    MainViewModel.Instance.GetListNhap();
            });

            AddCommandCT = new RelayCommand<object>((p) => selectedVT != null, (p) =>
            {
                CT_PhieuNhapDetail ct = new CT_PhieuNhapDetail()
                {
                    SoPhieu = txtSoPhieu,
                    MaVT = selectedVT.MaVT,
                    TenVT = selectedVT.TenVT,
                    TenDVT = selectedVT.TenDVT,
                    MaTK = selectedVT.MaTK,
                };
                ListDataCT.Add(ct);
                OnPropertyChanged("ListVTSelect");
            });

            DeleteItemCommandCT = new RelayCommand<object>((p) => true, (p) =>
            {
                ListDataCT.Remove(p as CT_PhieuNhapDetail);
                OnPropertyChanged("ListVTSelect");
            });
        }

        public void LoadNewData()
        {
            LoadTableData();
            GetListNguoiGiao();
            GetListKho();
            GetListTaiKhoan();
            notify.init();
        }
        public override void InitFilter()
        {
            Search = "";
            EndDate = null;
            BeginDate = null;
        }

        public override void ClearTextboxValue()
        {
            ListDataCT.Clear();
            selectedMaNguoiGiao = string.Empty;
            selectedMaKho = string.Empty;
            selectedMaNCC = string.Empty;
            selectedNgayNhap = DateTime.Now;
            txtTenNCC = string.Empty;
            txtChungTuLQ = string.Empty;
            txtLyDo = string.Empty;
            selectedTKCo = string.Empty;
            txtTongTien = 0;
        }
        public void GetListNguoiGiao()
        {
            string data = CRUD.GetJoinTableData("NguoiGiao");
            ListNguoiGiaoDetail = JsonConvert.DeserializeObject<ObservableCollection<NguoiGiaoDetail>>(data);
            foreach (var item in ListNguoiGiaoDetail)
            {
                item.TenNguoiGiao = item.MaNguoiGiao + " - " + item.TenNguoiGiao;
            }
        }
        public void GetListKho()
        {
            string data = CRUD.GetJsonData("Kho");
            ListKho = JsonConvert.DeserializeObject<ObservableCollection<Kho>>(data);
            foreach (var item in ListKho)
            {
                item.TenKho = item.MaKho + " - " + item.TenKho;
            }
        }
        public void GetListTaiKhoan()
        {
            string data = CRUD.GetJsonData("TaiKhoan");
            ListTK = JsonConvert.DeserializeObject<ObservableCollection<TaiKhoan>>(data);
            foreach (var item in ListTK)
            {
                item.TenTK = item.MaTK + " - " + item.TenTK;
            }
        }

        public void GetListCT(string SoPhieu)
        {
            string data = CRUD.GetDataByColumnName("CT_PhieuNhap", SoPhieu);
            ListDataCT = JsonConvert.DeserializeObject<ObservableCollection<CT_PhieuNhapDetail>>(data);
        }
        public void GetListVatTu()
        {
            string data = CRUD.GetJoinTableData("VatTu");
            ListVT = JsonConvert.DeserializeObject<ObservableCollection<VatTuDetail>>(data);
        }
        public void UpdateTongTienPN(PhieuNhap selectedPhieuNhap)
        {
            decimal Tong = 0;
            foreach (var item in ListDataCT)
            {
                Tong += item.ThanhTien;
            }
            selectedPhieuNhap.TongTien = Tong;
            CRUD.UpdateTongTien("phieunhap", selectedPhieuNhap.SoPhieu, selectedPhieuNhap);
        }
        public void ExportPhieuNhap(PhieuNhapDetail selectedPhieuNhap)
        {
            string Ngay = selectedPhieuNhap.NgayNhap.Day.ToString();
            string Thang = selectedPhieuNhap.NgayNhap.Month.ToString();
            string Nam = selectedPhieuNhap.NgayNhap.Year.ToString();
            using (SaveFileDialog sfd = new SaveFileDialog() { FileName = "Phiếu nhập kho ngày " + Ngay + "-" + Thang + "-" + Nam, Filter = "Word Document | *.docx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    notify.IsProcessing = true;
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        try
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
                            col2Range.Text = "Mẫu số 01 - VT\v";
                            col2Range.Font.Bold = 1;
                            col2Range.Collapse(oCollapseEnd);
                            col2Range.Text = "(Ban hành theo Thông tư số 200/2014/TT-BTC\vngày 22/12/2014 của Bộ Tài chính)";

                            Paragraph Title = document.Content.Paragraphs.Add(ref missing);
                            Title.Range.Text = "PHIẾU NHẬP KHO";
                            Title.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            Title.Range.Font.Size = 13;
                            Title.Range.Font.Bold = 1;
                            Title.SpaceAfter = 0;
                            Title.Range.InsertParagraphAfter();

                            Range range = document.Bookmarks.get_Item(ref oEndOfDoc).Range;
                            Table TitleTable = document.Tables.Add(range, 1, 3);
                            TitleTable.Range.Font.Bold = 0;
                            TitleTable.Range.Font.Size = 11;

                            Range rngCol2 = TitleTable.Cell(1, 2).Range;
                            rngCol2.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            rngCol2.Collapse(ref oCollapseStart);
                            rngCol2.Text = "Ngày " + Ngay + " tháng " + Thang + " năm " + Nam + "\v";
                            rngCol2.Font.Italic = 1;
                            rngCol2.Collapse(oCollapseEnd);
                            rngCol2.Text = "Số: " + selectedPhieuNhap.SoPhieu;
                            rngCol2.Font.Italic = 0;

                            List<string> listTKNo = new List<string>();
                            foreach (var item in ListDataCT)
                                listTKNo.Add(item.MaTK);

                            TitleTable.Cell(1, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            TitleTable.Cell(1, 3).Range.Text = "Nợ: " + Utils.Utils.FormatListTK(listTKNo) + "\vCó: " + selectedPhieuNhap.TKCo;


                            Paragraph Date = document.Content.Paragraphs.Add(ref missing);
                            Date.Range.Font.Size = 11;
                            Date.Range.Font.Bold = 0;
                            Date.Range.Text = "\t- Họ và tên người giao: " + selectedPhieuNhap.TenNguoiGiao +
                                "\v\t- Theo biên bản bàn giao hàng hóa số " + selectedPhieuNhap.SoPhieu.Substring(2) + "/BBBG ngày " + Ngay + "/" + Thang + "/" + Nam +
                                " của " + selectedPhieuNhap.TenNCC +
                                "\v\t- Nhập tại kho: " + selectedPhieuNhap.TenKho + "\t\tđịa điểm: " + selectedPhieuNhap.DiaChi;
                            Date.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            Date.Range.ParagraphFormat.SpaceAfter = 6;
                            Date.Range.InsertParagraphAfter();


                            Table MainTable = document.Tables.Add(Date.Range, ListDataCT.Count + 4, 8);
                            MainTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            MainTable.Range.ParagraphFormat.SpaceAfter = 0;
                            MainTable.Borders.Enable = 1;
                            MainTable.Range.Font.Bold = 0;
                            MainTable.Range.Font.Size = 11;
                            MainTable.Rows[ListDataCT.Count + 4].Range.Font.Bold = 1;

                            MainTable.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;

                            //STT
                            MainTable.Columns[1].Width = 0.073f * PageWidth;
                            MainTable.Cell(1, 1).Range.Text = "STT";
                            //Tên
                            MainTable.Columns[2].Width = 0.273f * PageWidth;
                            MainTable.Cell(1, 2).Range.Text = "Tên, nhãn hiệu, quy cách, phẩm chất vật tư, dụng cụ sản phẩm, hàng hoá";
                            //Mã số
                            MainTable.Columns[3].Width = 0.091f * PageWidth;
                            MainTable.Cell(1, 3).Range.Text = "Mã số";
                            //Đơn vị tính
                            MainTable.Columns[4].Width = 0.091f * PageWidth;
                            MainTable.Cell(1, 4).Range.Text = "Đơn vị tính";
                            //Số lượng
                            MainTable.Columns[5].Width = 0.109f * PageWidth;
                            MainTable.Cell(1, 5).Range.Text = "Số lượng";
                            MainTable.Cell(2, 5).Range.Text = "Theo chứng từ";
                            MainTable.Columns[6].Width = 0.109f * PageWidth;
                            MainTable.Cell(2, 6).Range.Text = "Thực nhập";

                            MainTable.Columns[7].Width = 0.109f * PageWidth;
                            MainTable.Cell(1, 7).Range.Text = "Đơn giá";

                            //Ghi chú
                            MainTable.Columns[8].Width = 0.143f * PageWidth;
                            MainTable.Cell(1, 8).Range.Text = "Thành tiền";

                            MainTable.Cell(1, 1).Merge(MainTable.Cell(2, 1));
                            MainTable.Cell(1, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 2).Merge(MainTable.Cell(2, 2));
                            MainTable.Cell(1, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 3).Merge(MainTable.Cell(2, 3));
                            MainTable.Cell(1, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 4).Merge(MainTable.Cell(2, 4));
                            MainTable.Cell(1, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 7).Merge(MainTable.Cell(2, 7));
                            MainTable.Cell(1, 7).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 8).Merge(MainTable.Cell(2, 8));
                            MainTable.Cell(1, 8).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            MainTable.Cell(1, 5).Merge(MainTable.Cell(1, 6));


                            MainTable.Cell(3, 1).Range.Text = "A";
                            MainTable.Cell(3, 2).Range.Text = "B";
                            MainTable.Cell(3, 3).Range.Text = "C";
                            MainTable.Cell(3, 4).Range.Text = "D";
                            MainTable.Cell(3, 5).Range.Text = "1";
                            MainTable.Cell(3, 6).Range.Text = "2";
                            MainTable.Cell(3, 7).Range.Text = "3";
                            MainTable.Cell(3, 8).Range.Text = "4";

                            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                            nfi.CurrencyDecimalSeparator = ",";
                            nfi.CurrencyGroupSeparator = ".";
                            nfi.CurrencySymbol = "";
                            for (int i = 0; i < ListDataCT.Count; i++)
                            {
                                //STT
                                MainTable.Cell(i + 4, 1).Range.Text = (i + 1).ToString();
                                MainTable.Cell(i + 4, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                //Tên
                                MainTable.Cell(i + 4, 2).Range.Text = ListDataCT[i].TenVT;
                                MainTable.Cell(i + 4, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                MainTable.Cell(i + 4, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                                //MaVT
                                MainTable.Cell(i + 4, 3).Range.Text = ListDataCT[i].MaVT;
                                MainTable.Cell(i + 4, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                //DVT
                                MainTable.Cell(i + 4, 4).Range.Text = ListDataCT[i].TenDVT;
                                MainTable.Cell(i + 4, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                //SLSS
                                MainTable.Cell(i + 4, 5).Range.Text = ListDataCT[i].SLSoSach.ToString();
                                MainTable.Cell(i + 4, 5).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                //SLTT
                                MainTable.Cell(i + 4, 6).Range.Text = ListDataCT[i].SLThucTe.ToString();
                                MainTable.Cell(i + 4, 6).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                //DonGia
                                MainTable.Cell(i + 4, 7).Range.Text = ListDataCT[i].DonGia.ToString("C0", nfi);
                                MainTable.Cell(i + 4, 7).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                MainTable.Cell(i + 4, 7).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                                //ThanhTien
                                MainTable.Cell(i + 4, 8).Range.Text = ListDataCT[i].ThanhTien.ToString("C0", nfi);
                                MainTable.Cell(i + 4, 8).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                MainTable.Cell(i + 4, 8).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            }
                            int n = ListDataCT.Count + 4;
                            //Cong
                            MainTable.Cell(n, 2).Range.Text = "Cộng";
                            MainTable.Cell(n, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            //x
                            for (int col = 3; col <= 7; col++)
                            {
                                MainTable.Cell(n, col).Range.Text = "x";
                                MainTable.Cell(n, col).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            }
                            //Tong tien
                            MainTable.Cell(n, 8).Range.Text = selectedPhieuNhap.TongTien.ToString("C0", nfi);
                            MainTable.Cell(n, 8).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            MainTable.Cell(n, 8).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                            string money = Utils.Utils.NumberToText(selectedPhieuNhap.TongTien);
                            money = money[0].ToString().ToUpper() + money.Substring(1) + ".";
                            Paragraph TextMoney = document.Content.Paragraphs.Add(ref missing);
                            TextMoney.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            TextMoney.Range.Font.Bold = 0;
                            TextMoney.Range.Font.Size = 11;
                            TextMoney.Range.Text = "\n\t- Tổng số tiền (viết bằng chữ): " + money;
                            TextMoney.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            TextMoney.SpaceBefore = 6.0f;

                            object start = TextMoney.Range.Start + TextMoney.Range.Text.IndexOf("chữ):") + 5;
                            object end = TextMoney.Range.Start + TextMoney.Range.Text.IndexOf("chữ):") + 5 + money.Length;
                            var rngItalic = document.Range(ref start, ref end);
                            rngItalic.Italic = 1;
                            TextMoney.Range.InsertParagraphAfter();

                            Paragraph Last = document.Content.Paragraphs.Add(ref missing);
                            Last.Range.Font.Bold = 0;
                            Last.Range.Font.Size = 11;
                            Last.SpaceBefore = 6.0f;
                            Last.Range.Text = "\t- Số chứng từ gốc kèm theo: " + selectedPhieuNhap.ChungTuLQ;
                            Last.Range.InsertParagraphAfter();

                            Range wordRange2 = document.Bookmarks.get_Item(ref oEndOfDoc).Range;

                            Table SignTable = document.Tables.Add(wordRange2, 1, 4);
                            SignTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            SignTable.Range.Font.Italic = 0;
                            SignTable.Range.Font.Bold = 0;
                            SignTable.Range.Font.Size = 11;
                            Range col1 = SignTable.Cell(1, 1).Range;
                            col1.Collapse(ref oCollapseStart);
                            col1.Text = "\vNgười lập phiếu\v";
                            col1.Font.Bold = 1;
                            col1.Collapse(oCollapseEnd);
                            col1.Text = "(Ký, họ tên)";
                            col1.Font.Italic = 1;

                            Range col2 = SignTable.Cell(1, 2).Range;
                            col2.Collapse(ref oCollapseStart);
                            col2.Text = "\vNgười giao hàng\v";
                            col2.Font.Bold = 1;
                            col2.Collapse(oCollapseEnd);
                            col2.Text = "(Ký, họ tên)";
                            col2.Font.Italic = 1;

                            Range col3 = SignTable.Cell(1, 3).Range;
                            col3.Collapse(ref oCollapseStart);
                            col3.Text = "\vThủ kho\v";
                            col3.Font.Bold = 1;
                            col3.Collapse(oCollapseEnd);
                            col3.Text = "(Ký, họ tên)";
                            col3.Font.Italic = 1;

                            Range col4 = SignTable.Cell(1, 4).Range;
                            col4.Collapse(ref oCollapseStart);
                            col4.Text = "Ngày " + Ngay + " tháng " + Thang + " năm " + Nam + "\v";
                            col4.Font.Italic = 1;
                            col4.Collapse(oCollapseEnd);
                            col4.Text = "Kế toán trưởng\v(Hoặc bộ phận có nhu cầu nhập)\v";
                            col4.Font.Bold = 1;
                            col4.Collapse(oCollapseEnd);
                            col4.Text = "(Ký, họ tên)";
                            col4.Font.Italic = 1;

                            document.SaveAs2(sfd.FileName);
                            document.Close(ref missing, ref missing, ref missing);
                            document = null;
                            app.Quit(ref missing, ref missing, ref missing);
                            app = null;
                            Process.Start(sfd.FileName);
                        }
                        catch
                        {
                            notify.IsProcessing = false;
                            notify.updateDataFail("Xuất file thất bại");
                        }
                        notify.IsProcessing = false;
                    }));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }
    }
}
