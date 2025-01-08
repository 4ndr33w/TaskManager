using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.ViewModels;
using TaskManager.DesktopClient.Views;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Services.ViewServices
{
    public class LoginUserService
    {
        #region FIELDS 

        private readonly UsersRequestService _usersRequestService;
        private LoginWindow _loginWindow;
        private string _password;
        private string _login;
        private UserDto _localUser;
        private UserDto _onlineUser;
        private bool isNewUser = false;

        private readonly string _onlineUserCacheFileName = StaticResources.CachedUserFileName;
        private readonly string _localUserFileName = StaticResources.LocalUserFileName;
        private readonly string _cachedUserFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();

        private AuthToken _token;

        #endregion

        #region CTOR 

        public LoginUserService() 
        {
            _usersRequestService = new UsersRequestService();
        }
        public LoginUserService(object parameter)
        {
            _loginWindow = parameter as LoginWindow;
            _usersRequestService = new UsersRequestService();
        }

        #endregion

        public async Task LoginCachedUser(object parameter)
        {
            var cachedUser = await LoadUserCache(_onlineUserCacheFileName);
            await LoginMethod(parameter, cachedUser);
        }

        public async Task LoginMethod(object parameter, UserDto cachedUser = null)
        {
            _loginWindow = parameter as LoginWindow;
            if(_loginWindow != null)
            {
                string login = cachedUser == null ? _loginWindow.LoginWindowLoginTextBox.Text : cachedUser.Email;
                string password = cachedUser == null ?  _loginWindow.LoginWindowPasswordBox.Password : cachedUser.Password;
                _token = await _usersRequestService.GetToken(login, password);

                if (_token == null)
                {
                    var result = MessageBox.Show("Пользователь не найден. Продолжить работу локально?", "Local Work", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        _onlineUser = null;
                        _localUser = await LoadOrCreateLocalUser();

                        OpenMainWindow(_loginWindow, _localUser, null, null);
                    }
                }
                if (_token != null)
                {
                    var onlineUser = await GetUser(_token);

                    if (onlineUser == null)
                    {
                        var result = MessageBox.Show("Пользователь не найден. Продолжить работу локально?", "Local Work", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            _loginWindow.Hide();
                            OpenMainWindow(_loginWindow, _localUser, null, null);
                        }
                    }

                    cachedUser = cachedUser == null? await LoadUserCache(_onlineUserCacheFileName) : cachedUser;

                    if (onlineUser.Email != cachedUser.Email)
                    {
                        isNewUser = true;
                    }
                    if (isNewUser)
                    {
                        var saveUserMessage = MessageBox.Show("Сохранить логин и пароль?", "SaveData", MessageBoxButton.YesNo);

                        if (saveUserMessage == MessageBoxResult.Yes)
                        {
                            var result = await SaveUserCache(onlineUser, _onlineUserCacheFileName);
                        }
                    }
                    _onlineUser = onlineUser;
                    OpenMainWindow(_loginWindow, _localUser, _onlineUser, _token);
                }
            }
        }

        private async void OpenMainWindow(Window window, UserDto localUser, UserDto onlineUser = null, AuthToken token = null)
        {
            if (localUser == null)
            {
                await CreateLocalUser();
            }
            if(onlineUser == null && token != null)
            {
                _onlineUser = await GetUser(_token);
            }
            _localUser = await LoadOrCreateLocalUser();

            window.Hide();

            var mainWindow = new MainWindow();
            var mainWindowDataContext = new MainWindowViewModel(_token, _onlineUser, _localUser, mainWindow);

            mainWindow.DataContext = mainWindowDataContext;
            mainWindow.ShowDialog();
        }

        private async Task<UserDto> GetUser(AuthToken token)
        {
            var user = await _usersRequestService.GetAsync(token);
            return user;
        }

        #region LOAD AND SAVE CACHE AND CREATE LOCAL USER 

        private async Task<UserDto> LoadUserCache(string fileName)
        {
            var userDto = new UserDto();
            string directory = _environmentPath + _cachedUserFilePath;
            string filePath = directory + fileName;

            string userData = string.Empty;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                try
                {
                    userData = File.ReadAllText(filePath);
                    var decodedUser = DecodeUser(userData);
                    userDto = DeserializeUser(decodedUser);

                    return userDto;
                }
                catch (Exception)
                {
                }
                return null;
            }
            return null;
        }

        private async Task<bool> SaveUserCache(UserDto user, string filename)
        {
            if(user != null)
            {
                try
                {
                    string directory = _environmentPath + _cachedUserFilePath;
                    string filePath = directory + filename;

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var serializedUser = SerializeUser(user);
                    var encodedUser = EncodeUser(serializedUser);

                    if (File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, encodedUser);
                    }
                    else
                    {
                        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {

                            byte[] buffer = System.Text.Encoding.Default.GetBytes(encodedUser);
                            await stream.WriteAsync(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        private async Task<UserDto> LoadOrCreateLocalUser()
        {
            string directory = _environmentPath + _cachedUserFilePath;
            string localUserCachePath = directory + _localUserFileName;

            var localUser = await LoadUserCache(_localUserFileName);

            if(localUser == null)
            {
                localUser = await CreateLocalUser();
            }
            return localUser;
        }
        private async Task<UserDto> CreateLocalUser()
        {
            var userDto = new UserDto();

            userDto.Id = default;
            userDto.Name = "Local User";
            userDto.Email = "Local";
            userDto.UserStatus = Models.Enums.UserStatus.Local;
            _localUser = userDto;

            var savingCacheResult = await SaveUserCache(userDto, _localUserFileName);

            if (savingCacheResult)
            {
                return userDto;
            }
            else return null;
        }

        #endregion

        #region ENCODING / DECODING 

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
    }
}
