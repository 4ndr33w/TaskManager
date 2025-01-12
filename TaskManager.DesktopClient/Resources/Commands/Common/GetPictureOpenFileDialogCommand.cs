using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.ViewModels;
using TaskManager.DesktopClient.Views.Windows;

namespace TaskManager.DesktopClient.Resources.Commands.Common
{
    public class GetPictureOpenFileDialogCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override async void Execute(object parameter)
        {
            ImageLoadSaveService _imageService = new ImageLoadSaveService();

            if (parameter is Views.Pages.CreateProjectPage)
            {
                var page = parameter as Views.Pages.CreateProjectPage;
                var context = page.DataContext as ProjectsPageViewModel;
                context.Picture = await _imageService.SearchImageFile();
            }

            if (parameter is Views.Windows.RegistrationWindow)
            {
                var window = parameter as Views.Windows.RegistrationWindow;

                var context = window.DataContext as RegistrationWindowViewModel;

                context.Picture = await _imageService.SearchImageFile();
            }
        }
    }
}
