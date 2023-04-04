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
    class NhanVienViewModel : TableViewModel<NhanVien>
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
        private string _txtMaNV;
        public string txtMaNV
        {
            get => _txtMaNV;
            set => SetProperty(ref _txtMaNV, value);
        }
        private string _txtTenNV;
        public string txtTenNV
        {
            get => _txtTenNV;
            set => SetProperty(ref _txtTenNV, value);
        }
        private string _selectedMaBP;
        public string selectedMaBP
        {
            get => _selectedMaBP;
            set => SetProperty(ref _selectedMaBP, value);
        }
        private ObservableCollection<BoPhan> _ListBoPhan;
        public ObservableCollection<BoPhan> ListBoPhan
        {
            get => _ListBoPhan;
            set => SetProperty(ref _ListBoPhan, value);
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
                    NhanVien item = element as NhanVien;
                    return item.MaNV.ToLower().Contains(text) || item.TenNV.ToLower().Contains(text);
                });
            }
        }
        private string _filterBoPhan;
        public string FilterBoPhan
        {
            get => _filterBoPhan;
            set
            {
                SetProperty(ref _filterBoPhan, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("BoPhan", element => ((NhanVien)element).MaBoPhan.Equals(text));
            }
        }
        public NhanVienViewModel():base("NhanVien")
        {
            tbVisibility = "Collapsed";
        }
        public override void Event()
        {
            base.Event();
            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadTableData();
                GetListBoPhan();
                notify.init();
            });
            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                NhanVienDialog dialog = new NhanVienDialog();
                TitleDialog = "Thêm nhân viên";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                NhanVienDialog dialog = new NhanVienDialog();
                TitleDialog = "Cập nhật nhân viên";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as NhanVien;
                txtMaNV = itemData.MaNV;
                txtTenNV = itemData.TenNV;
                selectedMaBP = itemData.MaBoPhan;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    NhanVien nv = new NhanVien
                    {
                        MaNV = ListData.Count() == 0 ? "NV001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaNV),
                        TenNV = txtTenNV,
                        MaBoPhan = selectedMaBP == "" ? null : selectedMaBP,
                    };
                    AddData(nv);
                }
                else
                {
                    NhanVien nv = new NhanVien
                    {
                        MaNV = txtMaNV,
                        TenNV = txtTenNV,
                        MaBoPhan = selectedMaBP == "" ? null : selectedMaBP,
                    };
                    UpdateData(nv);
                }
                ((Window)p).Close();

            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as NhanVien;
                DeleteData(itemData.MaNV);
            });
        }
        public void GetListBoPhan()
        {
            string data = CRUD.GetJsonData("BoPhan");
            ListBoPhan = JsonConvert.DeserializeObject<ObservableCollection<BoPhan>>(data);
        }

        public override void InitFilter()
        {
            Search = "";
            FilterBoPhan = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaNV = string.Empty;
            txtTenNV = string.Empty;
            selectedMaBP = string.Empty;
        }
    }
}
