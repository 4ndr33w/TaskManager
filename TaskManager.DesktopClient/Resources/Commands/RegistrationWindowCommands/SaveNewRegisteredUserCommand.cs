using System.Windows;
using System.Windows.Media.Imaging;

using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.ViewModels;
using TaskManager.DesktopClient.Views.Windows;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Resources.Commands.RegistrationWindowCommands
{
    public class SaveNewRegisteredUserCommand : Abstractions.CommandBase
    {
        private byte[] _imageBytes;
        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set { _imageBytes = value; }
        }

        public override bool CanExecute(object parameter) => true;

        public async override void Execute(object parameter)
        {
            if (parameter != null)
            {
                var _usersRequestService = new UsersRequestService();
                var _imageService = new ImageLoadSaveService();
                var newUser = new UserDto();

                var window = parameter as Views.Windows.RegistrationWindow;

                var picture = window.PictureBox.Source as BitmapSource;


                UserDto userDto = new UserDto();
                userDto.Name = window.UsernameTextBox.Text;
                userDto.LastName = window.UserLastNameTextBox.Text;
                userDto.Email = window.UserEmailTextBox.Text;
                userDto.Password = window.UserPasswordTextBox.Text;
                userDto.Phone = window.UserPhoneTextBox.Text;
                userDto.Image = _imageService.GetByteArrayFromBitmapSource(picture);

                var result = await _usersRequestService.CreateAsync(userDto);
                if (result.ToLower() == "true")
                {
                    MessageBox.Show("Новый пользователь успешно зарегистрирован");
                }
                else
                {
                    MessageBox.Show("Возникла ошибка при регистрации");
                }

                (parameter as Window).Hide();
            }

        }
    }
}

/*
 * 
var window = (parameter as Views.Windows.RegistrationWindow);

            UserDto userDto = new UserDto();
            userDto.Name = window.UsernameTextBox.Text;
            userDto.LastName = window.UserLastNameTextBox.Text;
            userDto.Email = window.UserEmailTextBox.Text;
            userDto.Password = window.UserPasswordTextBox.Text;
            userDto.Phone = window.UserPhoneTextBox.Text;
            userDto.Image = this.ImageBytes;

            var result = await _usersRequestService.CreateAsync(userDto);

            (parameter as Window).Hide();
*/