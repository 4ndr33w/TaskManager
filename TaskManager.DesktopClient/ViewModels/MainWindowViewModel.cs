using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Prism.Commands;
using Prism.Mvvm;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Views.Components.LoginPanels;
using TaskManager.Models;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private UsersRequestService _usersRequestService;

        public MainWindowViewModel()
        {
            OnStartup();
        }


        #region FIELDS

        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _savedUserLoginFilename = StaticResources.SavedUserLoginFileName;
        private readonly string _savedUserLoginLocalFilePath = StaticResources.SavedLoginLocalPath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();

        private string _authorizedPanelVisibility = "Collapsed";
        private string _baseLoginPanelVisibility = "Visible";

        private string _authorizedPanelHeight = "0";
        private string _baseLoginPanelHeight = "30";

        public string AuthorizedPanelVisibility
        {
            get => _authorizedPanelVisibility;
            set
            {
                _authorizedPanelVisibility = value;
                RaisePropertyChanged(nameof(AuthorizedPanelVisibility));

                if (_authorizedPanelVisibility == "Collapsed")
                {
                    BaseLoginPanelVisibility = "Visible";
                }
            }
        }

        public string BaseLoginPanelVisibility
        {
            get => _baseLoginPanelVisibility;
            set
            {
                _baseLoginPanelVisibility = value;
                RaisePropertyChanged(nameof(BaseLoginPanelVisibility));
                if (_baseLoginPanelVisibility == "Collapsed")
                {
                    AuthorizedPanelVisibility = "Visible";
                }
            }
        }

        public string AuthorizedPanelHeight
        {
            get => _authorizedPanelHeight;
            set
            {
                _authorizedPanelHeight = value;
                RaisePropertyChanged(nameof(AuthorizedPanelHeight));

                if (_authorizedPanelHeight == "30")
                {
                    BaseLoginPanelHeight = "0";
                }
            }
        }

        public string BaseLoginPanelHeight
        {
            get => _baseLoginPanelHeight;
            set
            {
                _baseLoginPanelHeight = value;
                RaisePropertyChanged(nameof(BaseLoginPanelHeight));
                if (_baseLoginPanelHeight == "30")
                {
                    AuthorizedPanelHeight = "0";
                }
            }
        }

        //private User _user;
        //public User User 
        //{ 
        //    get => _user;
        //    set =>
        //        {
        //        _user = value;
        //        RaisePropertyChanged(nameof(User));
        //    }
        //}


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
        public DelegateCommand CloseApplicationCommand { get; private set; }
        public DelegateCommand<object> CloseLocalWindowCommand { get; private set; }
        public DelegateCommand OpenRegisterWindowCommand { get; private set; }
        public DelegateCommand<object> ShowAuthorizedUserPanelCommand {  get; private set; }
        public DelegateCommand DropAuthorizationCommand { get; private set; }
        #endregion

        #region METHODS
        private async void OnStartup()
        {
            _usersRequestService = new UsersRequestService();
            GetUserCommand = new DelegateCommand<object>(GetUser);
            CloseApplicationCommand = new DelegateCommand(CloseApplication);
            OpenRegisterWindowCommand = new DelegateCommand(OpenRegisterWindow);
            CloseLocalWindowCommand = new DelegateCommand<object>(CloseLocalWindow);
            ShowAuthorizedUserPanelCommand = new DelegateCommand<object>(ShowAuthorizedUserPanel);
            DropAuthorizationCommand = new DelegateCommand(DropAuthorization);

            //Login = await LoadUserCache() == String.Empty ? "Default" : await LoadUserCache();
        }


        public void DropAuthorization()
        {
            BaseLoginPanelVisibility = "Visible";
            AuthorizedPanelVisibility = "Collapsed";

            AuthorizedPanelHeight = "0";
            BaseLoginPanelHeight = "30";
        }
        public void CloseApplication()
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
        public void ShowAuthorizedUserPanel(object parameter)
        {
            (parameter as UserControl).Visibility = Visibility.Hidden;
        }
        public void CloseLocalWindow(object parameter)
        {
            (parameter as Window).Hide();
        }
        public void OpenRegisterWindow()
        {
            Views.Windows.RegistrationWindow registrationWindow = new Views.Windows.RegistrationWindow();
            registrationWindow.Owner = Application.Current.MainWindow;
            registrationWindow.ShowDialog();
        }
        public async void GetUser(object parameter)
        {
            var passwordBox = (parameter as BaseLoginBarComponent).LoginWindowPasswordBox; //parameter as PasswordBox;
            Password = passwordBox.Password;

            CheckPasswordAndLoginIfEmpty(Login, Password);
            

            Token = await _usersRequestService.GetToken(Login, Password);

            if (Token.accessToken == null)
            {
                return;
            }

            if (Token.accessToken != null)
            {
                CurrentUser = await GetCurrentUser(Token);

                if (CurrentUser != null)
                {
                    CacheCurrentUser(CurrentUser);

                    BaseLoginPanelVisibility = "Collapsed";
                    AuthorizedPanelVisibility = "Visible";

                    AuthorizedPanelHeight = "30";
                    BaseLoginPanelHeight = "0";
                    //ShowAuthorizedUserPanel(parameter);
                }
                
                //MessageBox.Show(CurrentUser.Name + "\n" + CurrentUser.UserStatus.ToString());
            }
        }

        private async Task<UserDto> GetCurrentUser(AuthToken token)
        {
            var user = await _usersRequestService.GetAsync(token);
            return user;
        }

        private string EncodeUser(string serializedUser)
        {
            string encodingType = Resources.StaticResources.EncodingType;
            var result = Convert.ToBase64String(System.Text.Encoding.GetEncoding(encodingType).GetBytes(serializedUser));
            return result;
        }
        private string DecodeUser(string encodedUser)
        {
            string encodingType = Resources.StaticResources.EncodingType;
            var encoding = System.Text.Encoding.GetEncoding(encodingType);

            string nameAndPassArray = encoding.GetString(Convert.FromBase64String(encodedUser));
            return nameAndPassArray;
        }
        private UserDto DeserializeUser(string user)
        {
            UserDto userDto = JsonSerializer.Deserialize<UserDto>(user);
            return userDto;
        }
        private string SerializeUser(UserDto userDto)
        {
            string user = JsonSerializer.Serialize(userDto);
            return user;
        }

        private async void CacheCurrentUser(UserDto user)
        {
            if (user != null)
            {
                string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
                string filePath = directoryPath + _savedUserLoginFilename; ;

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
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                    {
                        byte[] buffer = System.Text.Encoding.Default.GetBytes(encodedUser);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        private async Task<UserDto> LoadUserCache()
        {
            string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
            string filePath = directoryPath + _savedUserLoginFilename;
            string userData = String.Empty;

            var userDto = new UserDto();

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

                    userData = System.Text.Encoding.Default.GetString(buffer);

                    //return userData;
                }
            }
            var decodedUser = DecodeUser(userData);
            userDto = DeserializeUser(decodedUser);
            return userDto;
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
