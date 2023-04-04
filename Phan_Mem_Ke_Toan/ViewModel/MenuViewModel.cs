using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class MenuViewModel : BaseViewModel
    {
        public string icon { get; set; }
        public string text { get; set; }
        public ObservableCollection<NhomChucNangViewModel> NhomChucNangVMs { get; set; }
    }
}
