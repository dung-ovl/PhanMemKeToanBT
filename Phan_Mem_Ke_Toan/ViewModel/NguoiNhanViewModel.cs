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
    class NguoiNhanViewModel : TableViewModel<NguoiNhan>
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
        private string _txtMaNguoiNhan;
        public string txtMaNguoiNhan
        {
            get => _txtMaNguoiNhan;
            set => SetProperty(ref _txtMaNguoiNhan, value);
        }
        private string _txtTenNguoiNhan;
        public string txtTenNguoiNhan
        {
            get => _txtTenNguoiNhan;
            set => SetProperty(ref _txtTenNguoiNhan, value);
        }
        private string _txtDiaChi;
        public string txtDiaChi
        {
            get => _txtDiaChi;
            set => SetProperty(ref _txtDiaChi, value);
        }
        private string _selectedMaCongTrinh;
        public string selectedMaCongTrinh
        {
            get => _selectedMaCongTrinh;
            set => SetProperty(ref _selectedMaCongTrinh, value);
        }

        private ObservableCollection<CongTrinh> _ListCongTrinh;
        public ObservableCollection<CongTrinh> ListCongTrinh
        {
            get => _ListCongTrinh;
            set => SetProperty(ref _ListCongTrinh, value);
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
                    NguoiNhan item = element as NguoiNhan;
                    return item.MaNguoiNhan.ToLower().Contains(text) || item.TenNguoiNhan.ToLower().Contains(text) ||
                    item.DiaChi.ToLower().Contains(text);
                });
            }
        }
        private string _filterCongTrinh;
        public string FilterCongTrinh
        {
            get => _filterCongTrinh;
            set
            {
                SetProperty(ref _filterCongTrinh, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("CongTrinh", element => ((NguoiNhan)element).MaCongTrinh.Equals(text));
            }
        }
        public NguoiNhanViewModel() : base("NguoiNhan")
        {
            tbVisibility = "Collapsed";
        }

        public override void Event()
        {
            base.Event();
            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {       
                LoadTableData();
                GetListCongTrinh();
                notify.init();
            });
            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                NguoiNhanDialog dialog = new NguoiNhanDialog();
                TitleDialog = "Thêm người nhận";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                NguoiNhanDialog dialog = new NguoiNhanDialog();
                TitleDialog = "Cập nhật người nhận";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as NguoiNhan;
                txtMaNguoiNhan = itemData.MaNguoiNhan;
                txtTenNguoiNhan = itemData.TenNguoiNhan;
                txtDiaChi = itemData.DiaChi;
                selectedMaCongTrinh = itemData.MaCongTrinh;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    NguoiNhan nn = new NguoiNhan
                    {
                        MaNguoiNhan = ListData.Count() == 0 ? "NN001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaNguoiNhan),
                        TenNguoiNhan = txtTenNguoiNhan,
                        DiaChi = txtDiaChi,
                        MaCongTrinh = selectedMaCongTrinh == "" ? null : selectedMaCongTrinh,
                    };
                    AddData(nn);
                }
                else
                {
                    NguoiNhan nn = new NguoiNhan
                    {
                        MaNguoiNhan = txtMaNguoiNhan,
                        TenNguoiNhan = txtTenNguoiNhan,
                        DiaChi = txtDiaChi,
                        MaCongTrinh = selectedMaCongTrinh == "" ? null : selectedMaCongTrinh,
                    };
                    UpdateData(nn);
                }
                 ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as NguoiNhan;
                DeleteData(itemData.MaNguoiNhan);
            });
        }
        public void GetListCongTrinh()
        {
            string data = CRUD.GetJsonData("CongTrinh");
            ListCongTrinh = JsonConvert.DeserializeObject<ObservableCollection<CongTrinh>>(data);
        }

        public override void InitFilter()
        {
            Search = "";
            FilterCongTrinh = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaNguoiNhan = string.Empty;
            txtTenNguoiNhan = string.Empty;
            txtDiaChi = string.Empty;
            selectedMaCongTrinh = string.Empty;
        }
    }
}
