using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.Utils;
using Phan_Mem_Ke_Toan.ValidRule;
using Phan_Mem_Ke_Toan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class QuanTriNguoiDungViewModel : TableViewModel<AccountSystem>
    {
        private string _btnContent;
        public string BtnContent
        {
            get => _btnContent;
            set => SetProperty(ref _btnContent, value);
        }
        private AccountSystem _accountSignUp;
        public AccountSystem accountSignUp
        {
            get => _accountSignUp;
            set => SetProperty(ref _accountSignUp, value);
        }

        private string _passwordConfirm;
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                SetProperty(ref _passwordConfirm, value);
            }
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
                    AccountSystem item = element as AccountSystem;
                    return item.TenDangNhap.ToLower().Contains(text) || item.HoTen.ToLower().Contains(text);
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
                filter.AddFilter("BoPhan", element => ((AccountSystem)element).MaBoPhan.Equals(text));
            }
        }
        private string _filterQuyen;
        public string FilterQuyen
        {
            get => _filterQuyen;
            set
            {
                SetProperty(ref _filterQuyen, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("Quyen", element => ((AccountSystem)element).Quyen.Equals(text));
            }
        }

        public QuanTriNguoiDungViewModel() : base("nguoidung") { }

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
                NguoiDungDialog dialog = new NguoiDungDialog();
                dialog.Title = "Thêm người dùng";
                BtnContent = "Thêm";
                ClearTextboxValue();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                BtnContent = "Lưu";
                accountSignUp = new AccountSystem(p as AccountSystem);
                PasswordConfirm = accountSignUp.MatKhau;
                NguoiDungDialog dialog = new NguoiDungDialog();
                dialog.Title = "Chỉnh sửa người dùng";
                dialog.ShowDialog();
            });
            BtnCommand = new RelayCommand<object>((p) => { return Valid.IsValid(p as DependencyObject); }, (p) =>
            {
                if (BtnContent == "Thêm")
                    AddData(accountSignUp);
                else UpdateData(accountSignUp);
                ((Window)p).Close();
            });
            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as AccountSystem;
                DeleteData(itemData.TenDangNhap);
            });
        }

        public void GetListBoPhan()
        {
            string JsonData = CRUD.GetJsonData("bophan");
            ListBoPhan = JsonConvert.DeserializeObject<ObservableCollection<BoPhan>>(JsonData);
        }

        public override void InitFilter()
        {
            FilterBoPhan = "";
            FilterQuyen = "";
            Search = "";
        }

        public override void ClearTextboxValue()
        {
            accountSignUp = new AccountSystem();
            PasswordConfirm = "";
        }
    }
}
