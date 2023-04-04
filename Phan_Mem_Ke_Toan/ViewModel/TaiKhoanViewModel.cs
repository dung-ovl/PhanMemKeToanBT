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
using Phan_Mem_Ke_Toan.Utils;
using System.Windows.Data;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class TaiKhoanViewModel : TableViewModel<TaiKhoan>
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

        private string _txtMaTK;
        public string txtMaTK
        {
            get => _txtMaTK;
            set => SetProperty(ref _txtMaTK, value);
        }

        private string _txtTenTK;
        public string txtTenTK
        {
            get => _txtTenTK;
            set => SetProperty(ref _txtTenTK, value);
        }
        private int _selectedCapTK;
        public int selectedCapTK
        {
            get => _selectedCapTK;
            set
            {
                SetProperty(ref _selectedCapTK, value);
                GetListTK(selectedCapTK - 1);
            }
        }
        private string _selectedTK;
        public string selectedTK
        {
            get => _selectedTK;
            set => SetProperty(ref _selectedTK, value);

        }
        private string _txtLoaiTK;
        public string txtLoaiTK
        {
            get => _txtLoaiTK;
            set => SetProperty(ref _txtLoaiTK, value);
        }
        private bool _MaTKEnable;
        public bool MaTKEnable
        {
            get => _MaTKEnable;
            set => SetProperty(ref _MaTKEnable, value);
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
                    TaiKhoan item = element as TaiKhoan;
                    return item.MaTK.ToLower().Contains(text) || item.TenTK.ToLower().Contains(text);
                });
            }
        }

        public ObservableCollection<int> ListCapTK { get; set; }
        private ObservableCollection<TaiKhoan> _listTKMe;
        public ObservableCollection<TaiKhoan> ListTKMe
        {
            get => _listTKMe;
            set => SetProperty(ref _listTKMe, value);
        }
        public TaiKhoanViewModel() : base("TaiKhoan")
        {
            ListCapTK = new ObservableCollection<int>() { 1, 2, 3 };
        }

        public override void Event()
        {
            base.Event();
            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadTableData();
                notify.init();
            });

            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                TaiKhoanDialog dialog = new TaiKhoanDialog();
                TitleDialog = "Thêm tài khoản";
                BtnContent = "Thêm";
                MaTKEnable = true;
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                TaiKhoanDialog dialog = new TaiKhoanDialog();
                TitleDialog = "Cập nhật tài khoản";
                BtnContent = "Lưu";
                MaTKEnable = false;
                var itemData = p as TaiKhoan;
                txtMaTK = itemData.MaTK;
                txtTenTK = itemData.TenTK;
                selectedCapTK = itemData.CapTK;
                selectedTK = itemData.TKMe;
                txtLoaiTK = itemData.LoaiTK;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                TaiKhoan tk = new TaiKhoan
                {
                    MaTK = txtMaTK,
                    TenTK = txtTenTK,
                    CapTK = selectedCapTK,
                    TKMe = selectedTK,
                    LoaiTK = txtLoaiTK,
                };
                if (BtnContent == "Thêm")
                    AddData(tk);
                else UpdateData(tk);

                ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as TaiKhoan;
                DeleteData(itemData.MaTK);
            });
        }
        public void GetListTK(int CapTK)
        {
            ListTKMe = new ObservableCollection<TaiKhoan>();
            foreach (var item in ListData)
            {
                if (item.CapTK == CapTK)
                {
                    item.TenTK = item.MaTK.ToString() + " - " + item.TenTK;
                    ListTKMe.Add(item);
                }
            }
        }

        public override void InitFilter()
        {
            Search = "";
        }

        public override void ClearTextboxValue()
        {
            txtTenTK = string.Empty;
            txtMaTK = string.Empty;
            selectedCapTK = 1;
            txtLoaiTK = string.Empty;
        }
    }
}
