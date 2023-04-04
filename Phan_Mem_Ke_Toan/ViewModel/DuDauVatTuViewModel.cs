using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Model;
using Phan_Mem_Ke_Toan.ValidRule;
using Phan_Mem_Ke_Toan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class DuDauVatTuViewModel : TableViewModel<DuDauVatTu>
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
        private DuDauVatTu _duDauVTModel;
        public DuDauVatTu DuDauVTModel
        {
            get => _duDauVTModel;
            set => SetProperty(ref _duDauVTModel, value);
        }

        private ObservableCollection<VatTuDetail> _listVT;
        public ObservableCollection<VatTuDetail> ListVT
        {
            get => _listVT;
            set => SetProperty(ref _listVT, value);
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

        private ObservableCollection<Kho> _ListKho;
        public ObservableCollection<Kho> ListKho
        {
            get => _ListKho;
            set => SetProperty(ref _ListKho, value);
        }

        private string _filterKho;
        public string FilterKho
        {
            get => _filterKho;
            set
            {
                SetProperty(ref _filterKho, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("Kho", element => ((DuDauVatTu)element).MaKho.Equals(text));
            }
        }

        private string _filterVatTu;
        public string FilterVaTu
        {
            get => _filterVatTu;
            set
            {
                SetProperty(ref _filterVatTu, value);
                string text = value.Trim();
                if (text == "") return;
                filter.AddFilter("VatTu", element => ((DuDauVatTu)element).MaVT.Equals(text));
            }
        }
        public DuDauVatTuViewModel() : base("dudauvattu")
        {
        }


        public override void Event()
        {
            base.Event();

            LoadedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadNewData();
                notify.init();
            });

            AddCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                LoadNewData();
                TitleDialog = "Thêm dư đầu kỳ";
                BtnContent = "Thêm";
                ClearTextboxValue();
                DuDauVatTuDialog dialog = new DuDauVatTuDialog();
                dialog.ShowDialog();
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                TitleDialog = "Cập nhật dư đầu kỳ";
                BtnContent = "Lưu";
                DuDauVTModel = new DuDauVatTu(p as DuDauVatTu);
                DuDauVatTuDialog dialog = new DuDauVatTuDialog();
                dialog.ShowDialog();
            });

            BtnCommand = new RelayCommand<object>((p) =>
            {
                return Valid.IsValid(p as DependencyObject);
            }, (p) =>
            {
                DuDauVTModel.ThanhTien = (decimal)DuDauVTModel.SoLuong * DuDauVTModel.DonGia;
                if (BtnContent == "Thêm")
                {
                    if (!CheckExistDuDauKy())
                    {
                        AddData(DuDauVTModel);
                    }
                    else notify.updateDataFail("Thông tin vật tư đã tại trong kỳ này");
                }
                else UpdateData(DuDauVTModel);
                ((Window)p).Close();
            });

            DeleteItemCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var itemData = p as DuDauVatTu;
                DeleteData(itemData.MaSo.ToString());
            });
        }

        public void LoadNewData()
        {
            LoadTableData();
            GetListVatTu();
            GetListKho();
        }

        public bool CheckExistDuDauKy()
        {
            string url = "dudauvattu/kiemtradudauky?MaVT=" + DuDauVTModel.MaVT + "&MaKho=" + DuDauVTModel.MaKho + "&Nam=" + DuDauVTModel.Ngay.Year;
            string data = CRUD.GetJsonData(url);
            return !(String.IsNullOrEmpty(data));
        }
        public void GetListVatTu()
        {
            string data = CRUD.GetJoinTableData("VatTu");
            ListVT = JsonConvert.DeserializeObject<ObservableCollection<VatTuDetail>>(data);
        }

        public void GetListKho()
        {
            string data = CRUD.GetJsonData("Kho");
            ListKho = JsonConvert.DeserializeObject<ObservableCollection<Kho>>(data);
        }

        public override void InitFilter()
        {
            FilterKho = "";
            FilterVaTu = "";
        }

        public override void ClearTextboxValue()
        {
            DuDauVTModel = new DuDauVatTu();
            DuDauVTModel.Ngay = DateTime.Now;
        }
    }
}
