using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.DesktopClient.Services.ViewServices
{
    public class BaseViewService
    {
        public void ShowMessage(string messageText)
        {
            MessageBox.Show(messageText);
        }
    }
}
