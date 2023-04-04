using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.ValidRule;
using Phan_Mem_Ke_Toan.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class KhoViewModel : TableViewModel<Kho>
    {
        private string _titleDialog;
        public string TitleDialog
        {
            get => _titleDialog;
            set => SetProperty(ref _titleDialog, value);
        }

        private string _btnContent;
        public string BtnContent
        {
            get => _btnContent;
            set => SetProperty(ref _btnContent, value);
        }

        private string _tbVisibility;
        public string tbVisibility
        {
            get => _tbVisibility;
            set => SetProperty(ref _tbVisibility, value);
        }
        private string _txtMaKho;
        public string txtMaKho
        {
            get => _txtMaKho;
            set => SetProperty(ref _txtMaKho, value);
        }
        private string _txtTenKho;
        public string txtTenKho
        {
            get => _txtTenKho;
            set => SetProperty(ref _txtTenKho, value);
        }
        private string _txtDiaChi;
        public string txtDiaChi
        {
            get => _txtDiaChi;
            set => SetProperty(ref _txtDiaChi, value);
        }
        private string _txtSDT;
        public string txtSDT
        {
            get => _txtSDT;
            set => SetProperty(ref _txtSDT, value);
        }

        private string _selectedMaNV;
        public string selectedMaNV
        {
            get => _selectedMaNV;
            set => SetProperty(ref _selectedMaNV, value);
        }


        private ObservableCollection<NhanVienDetail> _ListThuKho;
        public ObservableCollection<NhanVienDetail> ListThuKho
        {
            get => _ListThuKho;
            set => SetProperty(ref _ListThuKho, value);
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
                    Kho item = element as Kho;
                    return item.MaKho.ToLower().Contains(text) || item.TenKho.ToLower().Contains(text) ||
                    item.DiaChi.ToLower().Contains(text) || item.SDT.ToLower().Contains(text);
                });
            }
        }
        private string _filterThuKho;
        public string FilterThuKho
        {
            get => _filterThuKho;
            set
            {
                SetProperty(ref _filterThuKho, value);
                if (value == "" || value == null) return;
                filter.AddFilter("ThuKho", element => ((Kho)element).MaThuKho.Equals(value));
            }
        }
        public KhoViewModel() : base("Kho")
        {
            tbVisibility = "Collapsed";
        }


        public override void Event()
        {
            base.Event();

            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadTableData();
                GetListThuKho();
                notify.init();
            });

            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                KhoDialog dialog = new KhoDialog();
                TitleDialog = "Thêm kho";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                KhoDialog dialog = new KhoDialog();
                TitleDialog = "Cập nhật kho";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as Kho;
                txtMaKho = itemData.MaKho;
                txtTenKho = itemData.TenKho;
                txtDiaChi = itemData.DiaChi;
                txtSDT = itemData.SDT;
                selectedMaNV = itemData.MaThuKho;
                dialog.ShowDialog();
            });

            BtnCommand = new RelayCommand<object>((p) =>
            {
                bool checkSDT = string.IsNullOrEmpty(txtSDT) ? true : Valid.isPhoneNumber(txtSDT);
                return Valid.IsValid(p as DependencyObject) && checkSDT;
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    Kho k = new Kho
                    {
                        MaKho = ListData.Count() == 0 ? "K001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaKho),
                        TenKho = txtTenKho,
                        DiaChi = txtDiaChi,
                        SDT = txtSDT,
                        MaThuKho = selectedMaNV == "" ? null : selectedMaNV,
                    };
                    AddData(k);
                }
                else
                {
                    Kho k = new Kho
                    {
                        MaKho = txtMaKho,
                        TenKho = txtTenKho,
                        DiaChi = txtDiaChi,
                        SDT = txtSDT,
                        MaThuKho = selectedMaNV == "" ? null : selectedMaNV,
                    };
                    UpdateData(k);
                }
                ((Window)p).Close();
            });

            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as Kho;
                DeleteData(itemData.MaKho);
            });
        }

        public void GetListThuKho()
        {
            string data = CRUD.GetJoinTableData("NhanVien");
            var list = JsonConvert.DeserializeObject<ObservableCollection<NhanVienDetail>>(data);
            ListThuKho = new ObservableCollection<NhanVienDetail>(list.Where(item => item.TenBoPhan == "Kế toán vật tư").ToList());
            foreach (var item in ListThuKho)
            {
                item.TenNV = item.MaNV + " - " + item.TenNV;
            }
        }

        public override void InitFilter()
        {
            Search = "";
            FilterThuKho = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaKho = string.Empty;
            txtTenKho = string.Empty;
            txtDiaChi = string.Empty;
            txtSDT = string.Empty;
            selectedMaNV = string.Empty;
        }
    }
}
