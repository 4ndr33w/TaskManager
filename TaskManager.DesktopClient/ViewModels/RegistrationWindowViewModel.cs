using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

namespace TaskManager.DesktopClient.ViewModels
{
    public class RegistrationWindowViewModel : BindableBase
    {
        private byte[] _picture;
        public byte[] Picture
        {
            get => _picture;
            set
            {
                _picture = value;
                RaisePropertyChanged(nameof(Picture));
            }
        }

        public RegistrationWindowViewModel() { }
    }
}
