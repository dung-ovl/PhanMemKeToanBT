using Phan_Mem_Ke_Toan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Phan_Mem_Ke_Toan.API;
using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.View;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Phan_Mem_Ke_Toan.ValidRule;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    public class DonViTinhViewModel : BaseViewModel
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

        private string _txtMaDVT;
        public string txtMaDVT
        {
            get => _txtMaDVT;
            set => SetProperty(ref _txtMaDVT, value);
        }

        private string _txtTenDVT;
        public string txtTenDVT
        {
            get => _txtTenDVT;
            set => SetProperty(ref _txtTenDVT, value);
        }
        private string _tbVisibility;
        public string tbVisibility
        {
            get => _tbVisibility;
            set => SetProperty(ref _tbVisibility, value);
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand BtnCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        private ObservableCollection<DonViTinh> _listData;
        public ObservableCollection<DonViTinh> ListData
        {
            get => _listData;
            set => SetProperty(ref _listData, value);
        }
        public void ClearTextboxValue()
        {
            txtTenDVT = string.Empty;
            txtMaDVT = string.Empty;
        }
        public DonViTinhViewModel()
        {
            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                DonViTinhDialog dialog = new DonViTinhDialog();
                TitleDialog = "Thêm đơn vị tính";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                DonViTinhDialog dialog = new DonViTinhDialog();
                TitleDialog = "Cập nhật đơn vị tính";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as DonViTinh;
                txtMaDVT = itemData.MaDVT;
                txtTenDVT = itemData.TenDVT;
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                if (BtnContent == "Thêm")
                {
                    DonViTinh dvt = new DonViTinh
                    {
                        MaDVT = ListData.Count() == 0 ? "DVT001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaDVT),
                        TenDVT = txtTenDVT,
                    };
                    if (CRUD.InsertData("donvitinh", dvt))
                    {
                        MessageBox.Show("Thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTableData();
                        ClearTextboxValue();
                    } 
                    else
                    {
                        MessageBox.Show("Đã có lỗi xảy ra, vui lòng thử lại sau", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    DonViTinh dvt = new DonViTinh
                    {
                        MaDVT = txtMaDVT,
                        TenDVT = txtTenDVT,
                    };
                    if (CRUD.UpdateData("donvitinh", dvt))
                    {
                        MessageBox.Show("Thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTableData();
                    }
                    else
                    {
                        MessageBox.Show("Đã có lỗi xảy ra, vui lòng thử lại sau", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    ((Window)p).Close();
                }

            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as DonViTinh;
                if (CRUD.DeleteData("donvitinh", itemData.MaDVT))
                {
                    MessageBox.Show("Thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTableData();
                }
                else
                {
                    MessageBox.Show("Đã có lỗi xảy ra, vui lòng thử lại sau", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }); 
            LoadTableData();
        }
        public void LoadTableData()
        {
            string JsonData = CRUD.GetJsonData("donvitinh");
            ListData = JsonConvert.DeserializeObject<ObservableCollection<DonViTinh>>(JsonData);
        } 
    }
}
