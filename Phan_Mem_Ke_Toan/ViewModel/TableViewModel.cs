using Newtonsoft.Json;
using Phan_Mem_Ke_Toan.API;
using Phan_Mem_Ke_Toan.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    abstract class TableViewModel<T> : BaseViewModel
    {
        private string tableName;
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand BtnCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand ShowAllCommand { get; set; }

        private ObservableCollection<T> _listData;
        public ObservableCollection<T> ListData
        {
            get => _listData;
            set => SetProperty(ref _listData, value);
        }

        public FilterSupport filter { get; set; }
        public NotifyViewModel notify { get; set; }

        public TableViewModel(string tableName)
        {
            this.tableName = tableName;
            Event();
            filter = new FilterSupport();
            notify = new NotifyViewModel();
        }

        public virtual void Event()
        {
            ShowAllCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                showAll();
            });
        }

        public void LoadTableData()
        {
            string JsonData;
            if (typeof(T).Name.Contains("Detail")) JsonData = CRUD.GetJoinTableData(tableName);
            else JsonData = CRUD.GetJsonData(tableName);
            ListData = JsonConvert.DeserializeObject<ObservableCollection<T>>(JsonData);
            filter.FilterView = (CollectionView)CollectionViewSource.GetDefaultView(ListData);
            showAll();
        }

        public void showAll()
        {
            InitFilter();
            filter.ClearFilters();
        }

        public void AddData(object o, string addSuccess = "Thêm dữ liệu thành công", string addFail = null)
        {
            if (CRUD.InsertData(tableName, o))
            {
                LoadTableData();
                ClearTextboxValue();
                notify.updateDataSuccess(addSuccess);
            }
            else notify.updateDataFail(addFail);
        }
        public void UpdateData(object o, string updateSucess = "Cập nhật dữ liệu thành công", string updateFail = null)
        {

            if (CRUD.UpdateData(tableName, o))
            {
                LoadTableData();
                notify.updateDataSuccess(updateSucess);
            }
            else notify.updateDataFail(updateFail);
        }

        public void DeleteData(string key, string deleteSuccess = "Xoá dữ liệu thành công", string deleteFail = null)
        {
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn xoá " + key, "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (CRUD.DeleteData(tableName, key))
                {
                    LoadTableData();
                    notify.updateDataSuccess(deleteSuccess);
                }
                else notify.updateDataFail(deleteFail);
            }
        }

        public abstract void InitFilter();
        public abstract void ClearTextboxValue();
    }
}
