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

            var page = parameter as Views.Pages.CreateProjectPage;
            ImageLoadSaveService _imageService = new ImageLoadSaveService();

            var context = page.DataContext as ProjectsPageViewModel;

            context.Picture = await _imageService.SearchImageFile();
        }
    }
}
