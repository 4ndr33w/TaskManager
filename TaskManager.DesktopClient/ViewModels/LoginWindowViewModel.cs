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

using Prism.Commands;
using Prism.Mvvm;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services;
using TaskManager.Models;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        private UsersRequestService _usersRequestService;

        public LoginWindowViewModel()
        {

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


        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _cachedUserFileName = StaticResources.CachedUserFileName;
        private readonly string _localUserFileName = StaticResources.LocalUserFileName;
        private readonly string _cachedUserFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();
        //private readonly string _fullLoginFilePath = Environment.SpecialFolder.MyDocuments.ToString() + StaticResources.CachedUserFilePath + StaticResources.CachedUserFileName;


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

        public DelegateCommand OpenRegisterWindowCommand { get; private set; }
        public DelegateCommand<object> GetUserCommand { get; private set; }
        public DelegateCommand CloseWindowCommand { get; private set; }
        public DelegateCommand CreateOrLoadLocalUserCommand { get; private set; }
        public DelegateCommand<object> OpenMainWindowCommand { get; private set; }
        public DelegateCommand<object> OpenMainWindowForLocalUserCommand { get; private set; }
        #endregion

        #region METHODS

        #region OnStartup 

        private async void OnStartup()
        {
            _usersRequestService = new UsersRequestService();
            GetUserCommand = new DelegateCommand<object>(GetUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            OpenRegisterWindowCommand = new DelegateCommand(OpenRegisterWindow);
            CreateOrLoadLocalUserCommand = new DelegateCommand(CreateLocalUser);
            OpenMainWindowCommand = new DelegateCommand<object> (OpenMainWindow);
            OpenMainWindowForLocalUserCommand = new DelegateCommand<object>(OpenMainWindowForLocalUser);

            var cachedUser = await LoadUserCache(_cachedUserFileName);

            CurrentUser = cachedUser;
            if (cachedUser.Password != string.Empty)
            {
                CachedUserButtonHeigh = "60";
                CachedUserLoginButtonVisibility = "Visible";
            }
        }

        #endregion

      
        public async void CreateLocalUser()
        {
            var userDto = new UserDto();

            userDto.Id = default;
            userDto.Name = "Local User";
            userDto.Email = "Local";
            userDto.UserStatus = Models.Enums.UserStatus.Local;
            LocalUser = userDto;

            SaveUserCache(LocalUser, _localUserFileName);
        }
        public void OpenRegisterWindow()
        {
            Views.Windows.RegistrationWindow registrationWindow = new Views.Windows.RegistrationWindow();
            registrationWindow.Owner = Application.Current.MainWindow;
            registrationWindow.ShowDialog();
        }
        public void CloseWindow()
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();

        }
        public async void GetUser(object parameter)
        {
            var window = (parameter as Views.LoginWindow);
            var passwordBox = window.LoginWindowPasswordBox;
            Password = passwordBox.Password;

            bool isNewUser = false;

            CheckPasswordAndLoginIfEmpty(Login, Password);

            Token = await _usersRequestService.GetToken(Login, Password);

            if (Token.accessToken == null)
            {
                var result = MessageBox.Show("Пользователь не найден. Продолжить работу локально?", "Local Work", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    CurrentUser = null;
                    OpenMainWindow(parameter);
                }
                if (result == MessageBoxResult.No && CurrentUser != null)
                {
                    result = MessageBox.Show("Пользователь не найден. Продолжить работу локально?", "Local Work", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        OpenMainWindow(parameter);
                    }
                }
            }

            if (Token.accessToken != null)
            {
                if (CurrentUser.Email != Login)
                {
                    isNewUser = true;
                }

                CurrentUser = await GetCurrentUser(Token);

                LocalUser = await TryToLoadLocalUser();
                if (LocalUser == null)
                {
                    CreateLocalUser();
                }

                if (CurrentUser == null)
                {
                    var result = MessageBox.Show("Пользователь не найден. Продолжить работу локально?", "Local Work", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        OpenMainWindowForLocalUser(parameter);
                    }
                }
                else
                {
                    if (isNewUser)
                    {
                        var saveUserMessage = MessageBox.Show("Сохранить логин и пароль?", "SaveData", MessageBoxButton.YesNo);

                        if (saveUserMessage == MessageBoxResult.Yes)
                        {
                            SaveUserCache(CurrentUser, _cachedUserFileName);
                        }
                    }
                    OpenMainWindow(parameter);
                }
            }
        }

        public void OpenMainWindowForLocalUser(object parameter)
        {
            CurrentUser= null;
            OpenMainWindow(parameter);
        }

        private async void OpenMainWindow(object parameter)
        {
            if (LocalUser == null)
            {
                CreateLocalUser();
            }
            if (CurrentUser != null)
            {
                if (Token == null)
                {
                    Token = await _usersRequestService.GetToken(CurrentUser.Email, CurrentUser.Password);
                }
            }
            var window = (parameter as Views.LoginWindow);
            window.Hide();

            var mainWindow = new MainWindow(/*Token, CurrentUser, LocalUser*/);
            var mainWindowDataContext = new MainWindowViewModel(Token, CurrentUser, LocalUser, mainWindow);
            mainWindow.DataContext = mainWindowDataContext;
            mainWindow.ShowDialog();
        }


        private async Task<UserDto> GetCurrentUser(AuthToken token)
        {
            try
            {
                var user = await _usersRequestService.GetAsync(token);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CheckPasswordAndLoginIfEmpty(string login, string password)
        {
            if (password == String.Empty && login == String.Empty)
            {
                MessageBox.Show("Enter login and password");
            }
            if (password == String.Empty)
            {
                MessageBox.Show("Enter password");
            }

            if (login == String.Empty)
            {
                MessageBox.Show("Enter login");
            }
        }

        #region ENCODING/DECODING

        private string EncodeUser(string serializedUser)
        {
            string encodingType = Resources.StaticResources.EncodingType;

            var test1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedUser));
            return test1;
        }
        private string DecodeUser(string encodedUser)
        {
            string encodingType = Resources.StaticResources.EncodingType;
            var encoding = System.Text.Encoding.GetEncoding(encodingType);

            var encLength = encodedUser.Length;
            int paddingLength = encLength % 4;
            if (paddingLength > 0)
            {
                encodedUser += new string('=', 4 - paddingLength);
            }
            string nameAndPassArray = encoding.GetString(Convert.FromBase64String(encodedUser));
            return nameAndPassArray.ToString();
        }
        private UserDto DeserializeUser(string user)
        {
            try
            {
                UserDto userDto = JsonSerializer.Deserialize<UserDto>(user);
                return userDto;
            }
            catch (Exception)
            {
                return new UserDto();
            }
        }
        private string SerializeUser(UserDto userDto)
        {
            string user = JsonSerializer.Serialize(userDto);
            return user;
        }
        #endregion

        #region CachedUser 

        private async void SaveUserCache(UserDto user, string filename)
        {
            if (user != null)
            {
                string directoryPath = _environmentPath + _cachedUserFilePath;
                string filePath = directoryPath + filename;

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var serializedUser = SerializeUser(user);
                var encodedUser = EncodeUser(serializedUser);

                if (!File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {

                        byte[] buffer = System.Text.Encoding.Default.GetBytes(encodedUser);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, encodedUser);
                }
            }
        }
        private async Task<UserDto> LoadUserCache(string fileName)
        {
            string directoryPath = _environmentPath + _cachedUserFilePath;
            string filePath = directoryPath + fileName;
            string userData = String.Empty;

            var userDto = new UserDto();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                userData = File.ReadAllText(filePath);
            }
            var decodedUser = DecodeUser(userData);
            userDto = DeserializeUser(decodedUser);

            return userDto;
        }
        public async Task<UserDto> TryToLoadLocalUser()
        {
            var userDto = new UserDto();
            string directory = _environmentPath + _cachedUserFilePath;
            string filePath = directory + _localUserFileName;
            string userData = String.Empty;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                try
                {
                    userData += File.ReadAllText(filePath);
                    var decodedUser = DecodeUser(userData);
                    userDto = DeserializeUser(decodedUser);
                    return userDto;
                }
                catch (Exception) {}
            }
            return null;
        }

        #endregion

        #endregion
    }
}
