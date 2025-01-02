using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Prism.Commands;
using Prism.Mvvm;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services;
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

        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _savedUserLoginFilename = StaticResources.SavedUserLoginFileName;
        private readonly string _savedUserLoginLocalFilePath = StaticResources.SavedLoginLocalPath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();
        //private readonly string _fullLoginFilePath = Environment.SpecialFolder.MyDocuments.ToString() + StaticResources.SavedLoginLocalPath + StaticResources.SavedUserLoginFileName;


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

        #region COMMANDS

        public DelegateCommand<object> GetUserCommand { get; private set; }
        public DelegateCommand CloseWindowCommand { get; private set; }
        #endregion

        #region METHODS
        private async void OnStartup()
        {
            _usersRequestService = new UsersRequestService();
            GetUserCommand = new DelegateCommand<object>(GetUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);

            Login = await LoadSavedUserLogin() == String.Empty ? "Default" : await LoadSavedUserLogin();
        }

        public void CloseWindow()
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();

        }
        public async void GetUser(object parameter/*, out UserDto user*/)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;

            CheckPasswordAndLoginIfEmpty(Login, Password);
            SaveCurrentLogin(Login);

            Token = await _usersRequestService.GetToken(Login, Password);

            if (Token.accessToken == null)
            {
                return;
            }

            if (Token.accessToken != null)
            {
                CurrentUser = await GetCurrentUser(Token);

                MessageBox.Show(CurrentUser.Name + "\n" + CurrentUser.UserStatus.ToString());
            }
            //else
            //{
            //    MessageBox.Show("Пользователь не найден");
            //}
        }

        private async Task<UserDto> GetCurrentUser(AuthToken token)
        {
            var user = await _usersRequestService.GetAsync(token);
            return user;
        }

        private async void SaveCurrentLogin(string login)
        {
            if (login != String.Empty)
            {
                string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
                string filePath = directoryPath + _savedUserLoginFilename; ;

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(Login);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
                if (File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(Login);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
            }

        }

        private async Task<string> LoadSavedUserLogin()
        {
            string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
            string filePath = directoryPath + _savedUserLoginFilename;
            string _login = String.Empty;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[stream.Length];

                    await stream.ReadAsync(buffer, 0, buffer.Length);

                    _login = Encoding.Default.GetString(buffer);

                    //return _login;
                }
            }
            return _login;
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

        #endregion
    }
}
