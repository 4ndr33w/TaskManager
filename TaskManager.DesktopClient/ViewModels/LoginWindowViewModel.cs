using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Prism.Commands;
using Prism.Mvvm;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.Models;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        private readonly UsersRequestService _usersRequestService;
        private readonly ImageLoadSaveService _imageLoadSaveService;
        private readonly LoginUserService _loginUserService;

        public LoginWindowViewModel()
        {
            _usersRequestService = new UsersRequestService();
            _imageLoadSaveService = new ImageLoadSaveService();
            _loginUserService = new LoginUserService();

            OnStartup();
        }

        #region FIELDS

        private string _cachedUserButtonHeight = "0";
        private string _cachedUserLoginButtonVisibility = "Collapsed";

        public string CachedUserButtonHeigh
        {
            get => _cachedUserButtonHeight;
            set
            {
                _cachedUserButtonHeight = value;
                RaisePropertyChanged(nameof(CachedUserButtonHeigh));
            }
        }
        public string CachedUserLoginButtonVisibility
        {
            get => _cachedUserLoginButtonVisibility;
            set
            {
                _cachedUserLoginButtonVisibility = value;
                RaisePropertyChanged(nameof(CachedUserLoginButtonVisibility));
            }
        }

        private BitmapSource _bitmappedImage;
        public BitmapSource BitmappedImage
        {
            get => _bitmappedImage;
            set
            {
                _bitmappedImage = value;
                RaisePropertyChanged($"{nameof(BitmappedImage)}");
            }
        }

        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _cachedUserFileName = StaticResources.CachedUserFileName;
        private readonly string _localUserFileName = StaticResources.LocalUserFileName;
        private readonly string _cachedUserFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();

        #region Token 
        private AuthToken _token;
        public AuthToken Token
        {
            get => _token;
            set
            {
                _token = value;
                RaisePropertyChanged(nameof(Token));
            }
        }
        #endregion

        #region LocalUser 
        private UserDto _localUser;
        public UserDto LocalUser
        {
            get => _localUser;
            set
            {
                _localUser = value;
                RaisePropertyChanged(nameof(LocalUser));
            }
        }
        #endregion

        #region _netUser 
        private UserDto _currentUser;
        public UserDto CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }
        #endregion

        #endregion

        #region COMMANDS

        #endregion

        #region METHODS

        #region OnStartup 

        private async void OnStartup()
        {
            var cachedUser = await _loginUserService.LoadUserCache(_cachedUserFileName);

            CurrentUser = cachedUser;
            if (cachedUser.Password != string.Empty)
            {
                CachedUserButtonHeigh = "60";
                CachedUserLoginButtonVisibility = "Visible";

                BitmappedImage = _imageLoadSaveService.GetBitmapSource(cachedUser.Image);
            }
        }

        #endregion


        #endregion
    }
}
