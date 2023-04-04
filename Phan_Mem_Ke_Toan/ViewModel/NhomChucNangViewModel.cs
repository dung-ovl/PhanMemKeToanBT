using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Phan_Mem_Ke_Toan.ViewModel
{
    class NhomChucNangViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public ObservableCollection<ChucNangViewModel> ChucNangVMs { get; set; }
    }
}
