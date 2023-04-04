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
    class BoPhanViewModel : TableViewModel<BoPhan>
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

        private string _txtMaBoPhan;
        public string txtMaBoPhan
        {
            get => _txtMaBoPhan;
            set => SetProperty(ref _txtMaBoPhan, value);
        }

        private string _txtTenBoPhan;
        public string txtTenBoPhan
        {
            get => _txtTenBoPhan;
            set => SetProperty(ref _txtTenBoPhan, value);
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
                    BoPhan item = element as BoPhan;
                    return item.MaBoPhan.ToLower().Contains(text) || item.TenBoPhan.ToLower().Contains(text);
                });
            }
        }

        public BoPhanViewModel() : base("BoPhan")
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
                BoPhanDialog dialog = new BoPhanDialog();
                TitleDialog = "Thêm bộ phận";
                BtnContent = "Thêm";
                tbVisibility = "Collapsed";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                BoPhanDialog dialog = new BoPhanDialog();
                TitleDialog = "Cập nhật bộ phận";
                BtnContent = "Lưu";
                tbVisibility = "Visible";
                var itemData = p as BoPhan;
                txtMaBoPhan = itemData.MaBoPhan;
                txtTenBoPhan = itemData.TenBoPhan;
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
                    BoPhan bp = new BoPhan
                    {
                        MaBoPhan = ListData.Count() == 0 ? "BP001" : CRUD.GeneratePrimaryKey(ListData[ListData.Count() - 1].MaBoPhan),
                        TenBoPhan = txtTenBoPhan,
                        MoTa = txtMoTa,
                    };
                    AddData(bp);
                }
                else
                {
                    BoPhan bp = new BoPhan
                    {
                        MaBoPhan = txtMaBoPhan,
                        TenBoPhan = txtTenBoPhan,
                        MoTa = txtMoTa,
                    };
                    UpdateData(bp);
                }
                              ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as BoPhan;
                DeleteData(itemData.MaBoPhan);
            });
        }

        public override void InitFilter()
        {
            Search = "";
        }

        public override void ClearTextboxValue()
        {
            txtMaBoPhan = string.Empty;
            txtTenBoPhan = string.Empty;
            txtMoTa = string.Empty;
        }
    }
}
