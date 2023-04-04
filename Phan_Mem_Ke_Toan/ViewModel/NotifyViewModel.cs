using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class NotifyViewModel : BaseViewModel
    {
        private bool _isError;
        public bool IsError
        {
            get => _isError;
            set => SetProperty(ref _isError, value);
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }

        private string _contentNotify;
        public string ContentNotify
        {
            get => _contentNotify;
            set => SetProperty(ref _contentNotify, value);
        }

        public void init()
        {
            ContentNotify = "";
            IsError = false;
        }

        public void updateDataSuccess(string s = "Thành công")
        {
            if (s == null) s = "Thành công";
            ContentNotify = s;
            IsError = false;
            ShowNotify();
        }

        public void updateDataFail(string s = "Đã có lỗi xảy ra, vui lòng thử lại sau")
        {
            if (s == null) s = "Đã có lỗi xảy ra, vui lòng thử lại sau";
            ContentNotify = s;
            IsError = true;
            ShowNotify();
        }

        public void ShowNotify()
        {
            DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _timer.Start();
            _timer.Tick += (o, e) =>
            {
                init();
                _timer.Stop();
            };
        }
    }
}
