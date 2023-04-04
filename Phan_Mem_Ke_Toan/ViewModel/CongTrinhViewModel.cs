using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.View;
using Phan_Mem_Ke_Toan.ValidRule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class CongTrinhViewModel : TableViewModel<CongTrinh>
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

        private string _txtMaCongTrinh;
        public string txtMaCongTrinh
        {
            get => _txtMaCongTrinh;
            set => SetProperty(ref _txtMaCongTrinh, value);
        }

        private string _txtTenCongTrinh;
        public string txtTenCongTrinh
        {
            get => _txtTenCongTrinh;
            set => SetProperty(ref _txtTenCongTrinh, value);
        }
        private string _txtDiaChi;
        public string txtDiaChi
        {
            get => _txtDiaChi;
            set => SetProperty(ref _txtDiaChi, value);
        }
        private string _txtMoTa;
        public string txtMoTa
        {
            get => _txtMoTa;
            set => SetProperty(ref _txtMoTa, value);
        }
        private string _tbVisibility;
        public string tbVisibility
        {
            get => _tbVisibility;
            set => SetProperty(ref _tbVisibility, value);
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
                    CongTrinh item = element as CongTrinh;
                    return item.MaCongTrinh.ToLower().Contains(text) || item.TenCongTrinh.ToLower().Contains(text);
                });
            }
        }
        public CongTrinhViewModel():base("CongTrinh")
        {
            tbVisibility = "Collapsed";
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
                CongTrinhDialog dialog = new CongTrinhDialog();
                TitleDialog = "Thêm công trình";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                CongTrinhDialog dialog = new CongTrinhDialog();
                TitleDialog = "Cập nhật công trình";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as CongTrinh;
                txtMaCongTrinh = itemData.MaCongTrinh;
                txtTenCongTrinh = itemData.TenCongTrinh;
                txtDiaChi = itemData.DiaChi;
                txtMoTa = itemData.MoTa;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    CongTrinh ct = new CongTrinh
                    {
                        MaCongTrinh = ListData.Count() == 0 ? "CT001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaCongTrinh),
                        TenCongTrinh = txtTenCongTrinh,
                        DiaChi = txtDiaChi,
                        MoTa = txtMoTa,
                    };
                    AddData(ct);
                }
                else
                {
                    CongTrinh ct = new CongTrinh
                    {
                        MaCongTrinh = txtMaCongTrinh,
                        TenCongTrinh = txtTenCongTrinh,
                        DiaChi = txtDiaChi,
                        MoTa = txtMoTa,
                    };
                    UpdateData(ct);
                }
                ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as CongTrinh;
                DeleteData(itemData.MaCongTrinh);
            });
        }

        public override void InitFilter()
        {
            Search = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaCongTrinh = string.Empty;
            txtTenCongTrinh = string.Empty;
            txtDiaChi = string.Empty;
            txtMoTa = string.Empty;
        }
    }
}
