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
    class VatTuViewModel : TableViewModel<VatTu>
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
        private string _txtMaVT;
        public string txtMaVT
        {
            get => _txtMaVT;
            set => SetProperty(ref _txtMaVT, value);
        }
        private string _txtTenVT;
        public string txtTenVT
        {
            get => _txtTenVT;
            set => SetProperty(ref _txtTenVT, value);
        }
        private string _selectedMaLoai;
        public string selectedMaLoai
        {
            get => _selectedMaLoai;
            set => SetProperty(ref _selectedMaLoai, value);
        }
        private string _selectedMaDVT;
        public string selectedMaDVT
        {
            get => _selectedMaDVT;
            set => SetProperty(ref _selectedMaDVT, value);
        }

        private string _selectedMaTK;
        public string selectedMaTK
        {
            get => _selectedMaTK;
            set => SetProperty(ref _selectedMaTK, value);
        }

        private ObservableCollection<LoaiVatTu> _ListLoaiVT;
        public ObservableCollection<LoaiVatTu> ListLoaiVT
        {
            get => _ListLoaiVT;
            set => SetProperty(ref _ListLoaiVT, value);
        }
        private ObservableCollection<DonViTinh> _ListDVT;
        public ObservableCollection<DonViTinh> ListDVT
        {
            get => _ListDVT;
            set => SetProperty(ref _ListDVT, value);
        }
        private ObservableCollection<TaiKhoan> _ListTaiKhoan;
        public ObservableCollection<TaiKhoan> ListTaiKhoan
        {
            get => _ListTaiKhoan;
            set => SetProperty(ref _ListTaiKhoan, value);
        }
        public VatTuViewModel():base("VatTu")
        {
            tbVisibility = "Collapsed";
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
                    VatTu item = element as VatTu;
                    return item.MaVT.ToLower().Contains(text) || item.TenVT.ToLower().Contains(text);
                });
            }
        }
        private string _filterTaiKhoan;
        public string FilterTaiKhoan
        {
            get => _filterTaiKhoan;
            set
            {
                SetProperty(ref _filterTaiKhoan, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("TaiKhoan", element => ((VatTu)element).MaTK.Equals(text));
            }
        }
        private string _filterLoaiVatTu;
        public string FilterLoaiVatTu
        {
            get => _filterLoaiVatTu;
            set
            {
                SetProperty(ref _filterLoaiVatTu, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("LoaiVatTu", element => ((VatTu)element).MaLoai.Equals(text));
            }
        }

        public override void Event()
        {
            base.Event();

            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadTableData();
                GetListLoaiVT();
                GetListDVT();
                GetListTK();
                notify.init();
            });

            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                VatTuDialog dialog = new VatTuDialog();
                TitleDialog = "Thêm vật tư";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                VatTuDialog dialog = new VatTuDialog();
                TitleDialog = "Cập nhật vật tư";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as VatTu;
                txtMaVT = itemData.MaVT;
                txtTenVT = itemData.TenVT;
                selectedMaLoai = itemData.MaLoai;
                selectedMaDVT = itemData.MaDVT;
                selectedMaTK = itemData.MaTK;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    VatTu vt = new VatTu
                    {
                        MaVT = ListData.Count() == 0 ? "VT001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaVT),
                        TenVT = txtTenVT,
                        MaLoai = selectedMaLoai == "" ? null : selectedMaLoai,
                        MaDVT = selectedMaDVT == "" ? null : selectedMaDVT,
                        MaTK = selectedMaTK == "" ? null : selectedMaTK,
                    };
                    AddData(vt);
                }
                else
                {
                    VatTu vt = new VatTu
                    {
                        MaVT = txtMaVT,
                        TenVT = txtTenVT,
                        MaLoai = selectedMaLoai,
                        MaDVT = selectedMaDVT,
                        MaTK = selectedMaTK,
                    };
                    UpdateData(vt);
                }
                ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as VatTu;
                DeleteData(itemData.MaVT);
            });
        }
        public void GetListLoaiVT()
        {
            string data = CRUD.GetJsonData("LoaiVatTu");
            ListLoaiVT = JsonConvert.DeserializeObject<ObservableCollection<LoaiVatTu>>(data);
        }
        public void GetListDVT()
        {
            string data = CRUD.GetJsonData("DonViTinh");
            ListDVT = JsonConvert.DeserializeObject<ObservableCollection<DonViTinh>>(data);
        }
        public void GetListTK()
        {
            string data = CRUD.GetJsonData("TaiKhoan");
            ListTaiKhoan = JsonConvert.DeserializeObject<ObservableCollection<TaiKhoan>>(data);
            foreach(var item in ListTaiKhoan)
            {
                item.TenTK = item.MaTK + " - " + item.TenTK;
            }
        }

        public override void InitFilter()
        {
            Search = "";
            FilterTaiKhoan = "";
            FilterLoaiVatTu = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaVT = string.Empty;
            txtTenVT = string.Empty;
            selectedMaLoai = string.Empty;
            selectedMaDVT = string.Empty;
            selectedMaTK = string.Empty;
        }
    }
}
