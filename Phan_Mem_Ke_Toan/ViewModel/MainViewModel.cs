using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance;

        public bool isLogout { get; set; }
        private string _titleOption;
        public string TitleOption
        {
            get => _titleOption;
            set => SetProperty(ref _titleOption, value);
        }
        private int _selectedIndexMenu;
        public int SelectedIndexMenu
        {
            get => _selectedIndexMenu;
            set
            {
                SetProperty(ref _selectedIndexMenu, value);
                if (value == -1) return;
                if (value == 0) SelectedIndexWorking = -1;
                NhomChucNangVMs = Menu[value].NhomChucNangVMs;
                TitleOption = Menu[value].text;
            }
        }

        private int _selectedIndexWorking;
        public int SelectedIndexWorking
        {
            get => _selectedIndexWorking;
            set
            {
                SetProperty(ref _selectedIndexWorking, value);
                ShowPage(value);
            }
        }

        private UserControl _currentPage;
        public UserControl CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
            }
        }
        public ObservableCollection<MenuViewModel> Menu { get; set; }
        public ObservableCollection<ChucNangViewModel> PageWorkings { get; set; }

        private ObservableCollection<NhomChucNangViewModel> _nhomChucNangVMs;
        public ObservableCollection<NhomChucNangViewModel> NhomChucNangVMs
        {
            get => _nhomChucNangVMs;
            set => SetProperty(ref _nhomChucNangVMs, value);
        }
        public ICommand ClosedCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public UserControl BangDieuKhien { get; set; }

        public MainViewModel()
        {
            Instance = this;

            Menu = new ObservableCollection<MenuViewModel>()
            {
                new MenuViewModel(){ icon="ViewDashboard", text="Bảng điều khiển", NhomChucNangVMs = new ObservableCollection<NhomChucNangViewModel>(){
                }},
                new MenuViewModel(){ icon="Cog", text="Hệ thống", NhomChucNangVMs = new ObservableCollection<NhomChucNangViewModel>(){
                    new NhomChucNangViewModel() { Title="Quản trị người dùng", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Đổi mật khẩu", icon="Key", iconColor="#FAAD14"},
                    }},
                    new NhomChucNangViewModel() { Title="Thoát", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Đăng xuất", icon="Logout", iconColor="#E80D00", isLogout = true },
                    }},
                }},
                new MenuViewModel(){ icon="Layers", text="Danh mục", NhomChucNangVMs = new ObservableCollection<NhomChucNangViewModel>(){
                    new NhomChucNangViewModel() { Title="Tài khoản", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Tài khoản", icon="CreditCardMultiple", iconColor="#4B32C3", page = new TaiKhoanUC() },
                    }},
                    new NhomChucNangViewModel() { Title="Đối tượng", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Nhà cung cấp", icon="AccountTie", iconColor="#000000", page = new NhaCungCapUC()},
                        new ChucNangViewModel() { text="Người giao", icon="AccountCowboyHat", iconColor="#F5DE19", page = new NguoiGiaoUC() },
                        new ChucNangViewModel() { text="Bộ phận", icon="AccountGroup", iconColor="#4630EB", page = new BoPhanUC() },
                        new ChucNangViewModel() { text="Nhân viên", icon="HumanChild", iconColor="#01A5F4", page = new NhanVienUC() },
                        new ChucNangViewModel() { text="Người nhận", icon="AccountHardHat", iconColor="#DD4C35", page= new NguoiNhanUC() },
                    }},
                    new NhomChucNangViewModel() { Title="Kho - Vật tư", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Kho vật tư", icon="GarageVariant", iconColor="#06CC14", page = new KhoUC() },
                        new ChucNangViewModel() { text="Loại vật tư", icon="Cube", iconColor="#B17CFF", page = new LoaiVatTuUC() },
                        new ChucNangViewModel() { text="Vật tư", icon="Wall", iconColor="#FB7604", page = new VatTuUC() },
                        new ChucNangViewModel() { text="Đơn vị tính", icon="Apps", iconColor="#bc2d32", page = new DonViTinhUC() },
                    }},
                    new NhomChucNangViewModel() { Title="Công trình", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Công trình", icon="OfficeBuilding", iconColor="#4DCC89", page = new CongTrinhUC() },
                    }},
                }},
                new MenuViewModel(){ icon="FileSwap", text="Chứng từ", NhomChucNangVMs=new ObservableCollection<NhomChucNangViewModel>(){
                       new NhomChucNangViewModel() { Title="Chứng từ", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Phiếu nhập kho", icon="FileImport", iconColor="#C64A31", page = new PhieuNhapUC() },
                        new ChucNangViewModel() { text="Phiếu xuất kho", icon="FileExport", iconColor="#5099B8", page = new PhieuXuatUC() },
                    }},
                       new NhomChucNangViewModel() { Title="Kiểm kê", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Biên bản kiểm kê", icon="ClipboardCheckOutline", iconColor="#FF4500", page = new BienBanUC() },
                    }},
                       new NhomChucNangViewModel() { Title="Đầu kỳ", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Dư đầu vật tư", icon="Warehouse", iconColor="#3773E1", page = new DuDauVatTuUC() },
                    }},
                }},
                new MenuViewModel(){ icon="Finance", text="Báo cáo", NhomChucNangVMs = new ObservableCollection<NhomChucNangViewModel>() {
                    new NhomChucNangViewModel() { Title="Kho", ChucNangVMs=new ObservableCollection<ChucNangViewModel>(){
                        new ChucNangViewModel() { text="Thẻ kho", icon="CardText", iconColor="#ffc400", page = new LapTheKhoDialog()},
                        new ChucNangViewModel() { text="Sổ chi tiết vật tư", icon="NotebookMultiple", iconColor="#4DCC89", page = new LapSoChiTietDialog()},
                        new ChucNangViewModel() { text="Báo cáo nhập xuất tồn", icon="BookOpen", iconColor="#275090", page = new LapBaoCaoNXTDialog()},
                    }}
                }},
                new MenuViewModel(){ icon="HelpCircle", text="Trợ giúp" },
            };

            PageWorkings = new ObservableCollection<ChucNangViewModel>();
            BangDieuKhien = new BangDieuKhien();
            Event();
        }

        private void Event()
        {
            LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                isLogout = false;
                ChucNangViewModel CNVM = new ChucNangViewModel() { text = "Quản trị người dùng", icon = "AccountDetails", iconColor = "#3773E1", page = new QuanTriNguoiDung() };
                var CNVMList = Menu[1].NhomChucNangVMs[0].ChucNangVMs;
                bool isContain = Menu[1].NhomChucNangVMs[0].ChucNangVMs.Count == 2;
                if (LoginViewModel.currentUser.Quyen.Equals("admin") && !isContain)
                {
                    CNVMList.Add(CNVM);
                }
                else if (LoginViewModel.currentUser.Quyen.Equals("user") && isContain)
                {
                    CNVMList.RemoveAt(1);
                }

                if (PageWorkings.Count != 0) PageWorkings.Clear();
                SelectedIndexMenu = 0;

                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                TitleNhap = "Tổng nhập tháng " + month + "/" + year;
                TitleXuat = "Tổng xuất tháng " + month + "/" + year;
                GetListNhap();
                GetListXuat();
            });

            ClosedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (!isLogout)
                    Application.Current.Shutdown();
            });
        }

        public void ShowPage(int value)
        {
            if (value == -1)
            {
                CurrentPage = BangDieuKhien;
            }
            else
            {
                if (SelectedIndexMenu == 0) SelectedIndexMenu = -1;
                object page = PageWorkings[value].page;
                if (page == null) return;
                else CurrentPage = page as UserControl;
            }
        }
        private ObservableCollection<DataChart> _ListNhap;
        public ObservableCollection<DataChart> ListNhap
        {
            get => _ListNhap;
            set => SetProperty(ref _ListNhap, value);
        }
        private ObservableCollection<DataChart> _ListXuat;
        public ObservableCollection<DataChart> ListXuat
        {
            get => _ListXuat;
            set => SetProperty(ref _ListXuat, value);
        }
        private string _TitleNhap;
        public string TitleNhap
        {
            get => _TitleNhap;
            set => SetProperty(ref _TitleNhap, value);
        }
        private string _TitleXuat;
        public string TitleXuat
        {
            get => _TitleXuat;
            set => SetProperty(ref _TitleXuat, value);
        }
        public void GetListNhap()
        {
            int Thang = DateTime.Now.Month;
            int Nam = DateTime.Now.Year;
            string url = "?Thang=" + Thang + "&Nam=" + Nam;
            string dataNhap = CRUD.GetJsonData("ct_phieunhap/chart" + url);
            var data = JsonConvert.DeserializeObject<ObservableCollection<DataChart>>(dataNhap);
            foreach (var item in data)
                item.TongTT /= 1000000;
            ListNhap = data;
        }
        public void GetListXuat()
        {
            int Thang = DateTime.Now.Month;
            int Nam = DateTime.Now.Year;
            string url = "?Thang=" + Thang + "&Nam=" + Nam;
            string dataXuat = CRUD.GetJsonData("ct_phieuxuat/chart" + url);
            var data = JsonConvert.DeserializeObject<ObservableCollection<DataChart>>(dataXuat);
            foreach (var item in data)
                item.TongTT /= 1000000;
            ListXuat = data;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
