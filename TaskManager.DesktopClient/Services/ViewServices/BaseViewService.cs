using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.VisualBasic;

using Prism.Mvvm;

namespace TaskManager.DesktopClient.Services.ViewServices
{
    public class BaseViewService
    {
        public void ShowMessage(string messageText)
        {
            MessageBox.Show(messageText);
        }

        public void OpenPage(Page page, string pageName, BindableBase viewModel)
        {
            page.DataContext = viewModel;
            page.Title = pageName;
        }

        public Page OpenPage(Page page , BindableBase viewModel = null)
        {
            page.DataContext = viewModel;

            return page;
        }
    }
}
