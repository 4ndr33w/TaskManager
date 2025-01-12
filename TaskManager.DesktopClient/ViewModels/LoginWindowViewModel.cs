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

        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _cachedUserFileName = StaticResources.CachedUserFileName;
        private readonly string _localUserFileName = StaticResources.LocalUserFileName;
        private readonly string _cachedUserFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();


        #region Localization Texts

        public readonly string ExitButtonString = TextData.ExitButtonString;
        public readonly string RegistrationButtonString = TextData.RegistrationButtonString;
        public readonly string LocalUserButtonString = TextData.LocalUserButtonString;
        public readonly string LastUserButtonString = TextData.LastUserButtonString;
        public string EnterLoginString = Resources.TextData.EnterLoginString;
        public readonly string EnterPasswordString = TextData.EnterPasswordString;

        public readonly string OkButtonString = TextData.OkButtonString;
        public readonly string CancelButtonString = TextData.CancelButtonString;

        #endregion


        #endregion

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

        #region NetUser 

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

        #region CachedUser 

        private UserDto _cachedUser;
        public UserDto CachedUser
        {
            get => _cachedUser;
            set
            {
                _cachedUser = value;
                RaisePropertyChanged(nameof(CachedUser));
            }
        }
        #endregion

        #region METHODS

        #region OnStartup 

        private async void OnStartup()
        {
            CachedUser = await _loginUserService.LoadUserCache(_cachedUserFileName);
        }

        #endregion


        #endregion
    }
}
