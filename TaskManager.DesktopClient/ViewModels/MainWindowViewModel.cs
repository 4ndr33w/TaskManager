using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Win32;

using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Views;
using TaskManager.DesktopClient.Views.Components.LoginPanels;
using TaskManager.DesktopClient.Views.Pages;
using TaskManager.Models;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private UsersRequestService _usersRequestService;
        private Services.ViewServices.BaseViewService _viewService;

        public MainWindowViewModel()
        {
            OnStartup();
        }

        public MainWindowViewModel(AuthToken token, UserDto? onlineUser, UserDto localUser, Window currentWindow = null)
        {
            Token = token;
            CurrentUser = onlineUser;
            LocalUser = localUser;
            CurrentWindow = currentWindow;

            DateFormat = CurrentUser == null? DateTime.Now.ToString("U") : CurrentUser.Created.ToString("U");
            OnStartup();

        }

        #region PROPERTIES

        private Window _currentWindow;
        public Window CurrentWindow
        {
            get => _currentWindow;
            private set
            {
                _currentWindow = value;
                RaisePropertyChanged(nameof(CurrentWindow));
            }
        }
        private Dictionary<string, DelegateCommand> _profileButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> ProfileButtons
        {
            get => _profileButtons;
            private set
            {
                _profileButtons = value;
                RaisePropertyChanged(nameof(ProfileButtons));
            }
        }

        private Dictionary<string, DelegateCommand> _onlineUserNavButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> OnlineUserNavButtons
        {
            get { return _onlineUserNavButtons; }
            private set
            {
                _onlineUserNavButtons = value;
                RaisePropertyChanged(nameof(OnlineUserNavButtons));
            }
        }

        private Dictionary<string, DelegateCommand> _localUserNavButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> LocalUserNavButtons
        {
            get { return _localUserNavButtons; }
            private set
            {
                _localUserNavButtons = value;
                RaisePropertyChanged(nameof(LocalUserNavButtons));
            }
        }

        private byte[] _imageBytes;
        public byte[] ImageBytes
        {
            get => _imageBytes;
            set
            {
                _imageBytes = value;
                RaisePropertyChanged(nameof(ImageBytes));
            }
        }

        private System.Drawing.Image _userPicture;
        public System.Drawing.Image UserPicture
        {
            get => _userPicture;
            set
            {
                _userPicture = value;
                RaisePropertyChanged(nameof(UserPicture));
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


        private string _selectedPageName;
        public string SelectedPageName
        {
            get => _selectedPageName;
            set
            {
                _selectedPageName = value;
                RaisePropertyChanged(nameof(SelectedPageName));
            }
        }

        private Page _selectedPage;
        public Page SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
                RaisePropertyChanged(nameof(SelectedPage));
            }
        }

        #region text variables 


        #region ButtonNames 

        private readonly string _userInfoButtonName = "Profile Info";
        private readonly string _manageUsersButtonName = "Users Management";
        private readonly string _logoutButtonName = "Log Out";

        private readonly string _userProjectsButtonName = "Collective Projects";
        private readonly string _userDesksButtonName = "Collective Desks";
        private readonly string _userTasksButtonName = "Collective Tasks";
        

        private readonly string _localProjectsButtonName = "Local Projects";
        private readonly string _localDesksButtonName = "Local Desks";
        private readonly string _localTasksButtonName = "Local Tasks";

        public readonly string CollectiveProjectsLabelString = "Collective Projects Work";

        #endregion


        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _savedUserLoginFilename = StaticResources.CachedUserFileName;
        private readonly string _savedUserLoginLocalFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();

        #region AuthorizationPanelBindings 

        private string _authorizedPanelVisibility = "Collapsed";
        private string _baseLoginPanelVisibility = "Visible";
        private string _cachedUserPanelVisibility = "Collapsed";

        private string _authorizedPanelHeight = "0";
        private string _baseLoginPanelHeight = "50";
        private string _cachedUserPanelHeight = "0";

        public string CachedUserPanelVisibility
        {
            get => _cachedUserPanelVisibility;
            set
            {
                _cachedUserPanelVisibility= value;
                RaisePropertyChanged(nameof(CachedUserPanelVisibility));
            }
        }
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

                if (_authorizedPanelHeight == "50")
                {
                    BaseLoginPanelHeight = "0";
                    CachedUserPanelHeight = "0";
                }
            }
        }
        public string CachedUserPanelHeight
        {
            get => _cachedUserPanelHeight;
            set
            {
                _cachedUserPanelHeight = value;
                RaisePropertyChanged(nameof(CachedUserPanelHeight));

                if (_cachedUserPanelHeight == "50")
                {
                    BaseLoginPanelHeight = "0";
                    AuthorizedPanelHeight = "0";
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
                if (_baseLoginPanelHeight == "50")
                {
                    AuthorizedPanelHeight = "0";
                    CachedUserPanelHeight = "0";
                }
            }
        }

        #endregion

        #endregion

        private string _dateFormat;
        public string DateFormat
        {
            get => _dateFormat;
            set
            {
                _dateFormat = value;
                RaisePropertyChanged(nameof(DateFormat));
            }
        }

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

        #region CurrentUser 

        private UserDto _currentUser;
        public UserDto CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }
        #endregion

        #region LocalUser 

        private UserDto _localUser;
        public UserDto LocalUser
        {
            get => _localUser;
            private set
            {
                _localUser = value;
                RaisePropertyChanged(nameof(LocalUser));
            }
        }
        #endregion

        #endregion


        #region COMMANDS

        #region Other 
        public DelegateCommand<object> GetUserCommand { get; private set; }
        public DelegateCommand<object> CloseLocalWindowCommand { get; private set; }
        public DelegateCommand<object> ShowAuthorizedUserPanelCommand { get; private set; }
        public DelegateCommand<object> RegisterNewUserCommand { get; private set; }
        public DelegateCommand<object> SearchImageFileCommand { get; private set; }

        public DelegateCommand CloseApplicationCommand { get; private set; }
        public DelegateCommand OpenRegisterWindowCommand { get; private set; }
        public DelegateCommand LoginCachedUserCommand { get; private set; }

        #endregion


        #region NavButtonsCommands 

        #region profileComands 

        public DelegateCommand OpenMyInfoPageCommand { get; private set; }
        public DelegateCommand OpenUserManagementCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        #endregion

        #region onlineUserCommands 

        public DelegateCommand OpenProjectsPageCommand { get; private set; }
        public DelegateCommand OpenDesksPageCommand { get; private set; }
        public DelegateCommand OpenTasksPageCommand { get; private set; }

        #endregion

        #region localUserCommands 

        public DelegateCommand OpenLocalProjectsPageCommand { get; private set; }
        public DelegateCommand OpenLocalDesksPageCommand { get; private set; }
        public DelegateCommand OpenLocalTasksPageCommand { get; private set; }

        #endregion

        #endregion

        #endregion

        #region METHODS

        #region OpenPages

        #region profile 



        private void OpenMyInfoPage()
        {
            var page = new UserInfoPage();
            BitmappedImage = GetBitmapSource(CurrentUser.Image);
            page.DataContext = this;
            OpenPage(page, _userInfoButtonName, this);
        }
        private void OpenUserManagement()
        {
            SelectedPageName = _manageUsersButtonName;
            _viewService.ShowMessage(_manageUsersButtonName);
            //MessageBox.Show(_manageUsersButtonName);
        }
        private void Logout()
        {
            var request = MessageBox.Show("Want to gol out?", "Logout", MessageBoxButton.YesNo);

            if (request == MessageBoxResult.Yes && CurrentWindow != null)
            {
                CurrentWindow.Close();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                
            }
        }

        #endregion

        #region projectButtons 

        private void OpenProjectsPage()
        {
            var page = new ProjectsPage();
            page.projectUsersListBox.Items.Clear();
            OpenPage(page, _userProjectsButtonName, new ProjectsPageViewModel(Token));
        }
        private void OpenDesksPage()
        {
            SelectedPageName = _userDesksButtonName;
            _viewService.ShowMessage(_userDesksButtonName);
        }
        private void OpenTasksPage()
        {
            var page = new TasksPage();
            OpenPage(page, _userTasksButtonName, new TasksPageViewModel(Token));
            //MessageBox.Show(_userTasksButtonName);
        }

        #endregion

        #region localProjectButtons 

        private void OpenLocalProjectsPage()
        {
            SelectedPageName += _localProjectsButtonName;
            _viewService.ShowMessage("ToDo: Projects Page");
        }
        private void OpenLocalDesksPage()
        {
            SelectedPageName = _localDesksButtonName;
            _viewService.ShowMessage("ToDo: Desks Page");
        }
        private void OpenLocalTasksPage()
        {
            SelectedPageName = _localTasksButtonName;
            _viewService.ShowMessage("ToDo: Tasks Page");
        }

        #endregion

        #endregion

        #region COMMAND METHODS

        public void OpenPage(Page page, string pageName, BindableBase viewModel)
        {
            DateFormat = CurrentUser.Created.ToString("U");
            SelectedPageName = pageName;
            SelectedPage = page;
            SelectedPage.DataContext = viewModel;
        }

        public async void GetUser(object parameter)
        {
            var passwordBox = (parameter as BaseLoginBarComponent).LoginWindowPasswordBox;
            Password = passwordBox.Password;

            CheckPasswordAndLoginIfEmpty(Login, Password);

            Token = await _usersRequestService.GetToken(Login, Password);

            if (Token.accessToken != null)
            {
                CurrentUser = await GetCurrentUser(Token);

                if (CurrentUser != null)
                {
                    if (CurrentUser.Image != null)
                    {
                        BitmappedImage = GetBitmapSource(CurrentUser.Image);
                    }
                    CacheCurrentUser(CurrentUser);
                }
            }
        }
        public void CloseApplication()
        {
            Environment.Exit(0);
            //Application.Current.MainWindow.Close();
            //Application.Current.Shutdown();
        }
        public void CloseLocalWindow(object parameter)
        {
            (parameter as Window).Hide();
        }
        public void ShowAuthorizedUserPanel(object parameter)
        {
            (parameter as UserControl).Visibility = Visibility.Collapsed;
        }
        public void DropAuthorization()
        {

        }
        public async void RegisterNewUser(object parameter)
        {
            var window = (parameter as Views.Windows.RegistrationWindow);

            UserDto userDto = new UserDto();
            userDto.Name = window.UsernameTextBox.Text;
            userDto.LastName = window.UserLastNameTextBox.Text;
            userDto.Email = window.UserEmailTextBox.Text;
            userDto.Password = window.UserPasswordTextBox.Text;
            userDto.Phone = window.UserPhoneTextBox.Text;
            userDto.Image = this.ImageBytes;
            //userDto.ImageBytes = window.

            var result = await _usersRequestService.CreateAsync(userDto);

            (parameter as Window).Hide();

        }
        public async void SearchImageFile(object parameter)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var result = dialog.ShowDialog();
                dialog.Filter = "Изображения (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|Все файлы (*.*)|*.*";
                dialog.Title = "Выбор изображения";

                if (result.Value == true)
                {
                    this.ImageBytes = EncodingImage(dialog.FileName);
                    //MessageBox.Show($"Вы выбрали файл: {selectedFile}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception)
            {

            }
            //https://metanit.com/sharp/wpf/22.6.php
            //https://learn.microsoft.com/ru-ru/dotnet/desktop/wpf/windows/how-to-open-common-system-dialog-box?view=netdesktop-9.0
            //https://wpf-tutorial.com/ru/46/%D0%B4%D0%B8%D0%B0%D0%BB%D0%BE%D0%B3%D0%BE%D0%B2%D1%8B%D0%B5-%D0%BE%D0%BA%D0%BD%D0%B0/openfiledialog/
        }
        public async void LoginCachedUser()
        {
            Password = CurrentUser.Password;

            Token = await _usersRequestService.GetToken(CurrentUser.Email, CurrentUser.Password);

            if (Token.accessToken != null)
            {
                CurrentUser = await GetCurrentUser(Token);

                if (CurrentUser != null)
                {
                    BitmappedImage = GetBitmapSource(CurrentUser.Image);
                    CacheCurrentUser(CurrentUser);
                }
            }
        }

        #endregion

        #region OnStartup
        private async void OnStartup()
        {
            _viewService = new Services.ViewServices.BaseViewService();

            #region otherCommands 

            _usersRequestService = new UsersRequestService();
            GetUserCommand = new DelegateCommand<object>(GetUser);
            CloseApplicationCommand = new DelegateCommand(CloseApplication);
            CloseLocalWindowCommand = new DelegateCommand<object>(CloseLocalWindow);
            ShowAuthorizedUserPanelCommand = new DelegateCommand<object>(ShowAuthorizedUserPanel);
            RegisterNewUserCommand = new DelegateCommand<object>(RegisterNewUser);
            SearchImageFileCommand = new DelegateCommand<object>(SearchImageFile);
            LoginCachedUserCommand = new DelegateCommand(LoginCachedUser);

            #endregion

            #region NavButtonsCommands 

            #region profileCommands 

            OpenMyInfoPageCommand = new DelegateCommand(OpenMyInfoPage);
            OpenUserManagementCommand = new DelegateCommand(OpenUserManagement);
            LogoutCommand = new DelegateCommand(Logout);

            #endregion

            #region onlineButtons 

            OpenProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            OpenDesksPageCommand = new DelegateCommand(OpenDesksPage);
            OpenTasksPageCommand = new DelegateCommand(OpenTasksPage);

            #endregion

            #region localButtons 

            OpenLocalProjectsPageCommand = new DelegateCommand(OpenLocalProjectsPage);
            OpenLocalDesksPageCommand = new DelegateCommand(OpenLocalDesksPage);
            OpenLocalTasksPageCommand = new DelegateCommand(OpenLocalTasksPage);

            #endregion

            #endregion


            #region navButtonsDictionaryFill 

            #region online 

            if (CurrentUser != null)
            {
                #region profileButtons 

                ProfileButtons.Add(_userInfoButtonName, OpenMyInfoPageCommand);
                if (CurrentUser != null && CurrentUser.UserStatus == Models.Enums.UserStatus.Admin)
                {
                    ProfileButtons.Add(_manageUsersButtonName, OpenUserManagementCommand);
                }
                ProfileButtons.Add(_logoutButtonName, LogoutCommand);

                #endregion

                #region onlineButtons 

                OnlineUserNavButtons.Add(_userProjectsButtonName, OpenProjectsPageCommand);
                OnlineUserNavButtons.Add(_userDesksButtonName, OpenDesksPageCommand);
                OnlineUserNavButtons.Add(_userTasksButtonName, OpenTasksPageCommand);

                #endregion
                BitmappedImage = GetBitmapSource(CurrentUser.Image);

                OpenMyInfoPage();
            }

            #endregion

            #region localButtons 

            var test = LocalUser;
            if (LocalUser != null)
            {
                LocalUserNavButtons.Add(_localProjectsButtonName, OpenLocalProjectsPageCommand);
                LocalUserNavButtons.Add(_localDesksButtonName, OpenLocalDesksPageCommand);
                LocalUserNavButtons.Add(_localTasksButtonName, OpenLocalTasksPageCommand);

            }
            #endregion

            #endregion
        }

        #endregion


        #region Alert Message
        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        #endregion

        private async Task<UserDto> GetCurrentUser(AuthToken token)
        {
            var user = await _usersRequestService.GetAsync(token);
            return user;
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
                return null;
            }
            
            
        }
        private string SerializeUser(UserDto userDto)
        {
            string user = JsonSerializer.Serialize(userDto);
            return user;
        }
        #endregion 

        private byte[] EncodingImage(string imagePath)
        {
            string outputFilePath = "";
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                byte[] imageBytes = ImageToByteArray(image);
                //var result = SaveByteArray(imageBytes, outputFilePath);

                return imageBytes;
            }

        }
        private static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using(MemoryStream mStream = new MemoryStream())
            {
                image.Save(mStream, image.RawFormat);

                return mStream.ToArray();
            }
        }

        private static void SaveByteArray(byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        #region Load / Save user Cache 

        private async void CacheCurrentUser(UserDto user)
        {
            if (user != null)
            {
                string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
                string filePath = directoryPath + _savedUserLoginFilename; ;

                var isDirectoryExists = IsDirectoryExistAndCreateIfNotExist(directoryPath);

                if (isDirectoryExists)
                {
                    var serializedUser = SerializeUser(user);
                    var encodedUser = EncodeUser(serializedUser);

                    await SaveUserCacheIntoLocalFile(filePath, encodedUser);
                }
            }
        }
        private async Task SaveUserCacheIntoLocalFile(string filePath, string encodedUser)
        {
            try
            {
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
            catch (Exception)
            {
            }
        }
        private bool IsDirectoryExistAndCreateIfNotExist(string directoryPath)
        {
            bool result = false;
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory($"{directoryPath}");
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        private async Task<UserDto> LoadUserCache()
        {
            string directoryPath = _environmentPath + _savedUserLoginLocalFilePath;
            string filePath = directoryPath + _savedUserLoginFilename;
            string userData = String.Empty;

            var userDto = new UserDto();

            var isDirectoryExists = IsDirectoryExistAndCreateIfNotExist(directoryPath);

            if (isDirectoryExists)
            {
                userData = ReadEncodedUserDataFromLocalFile(filePath);

                var decodedUser = DecodeUser(userData);
                userDto = DeserializeUser(decodedUser);
            }
            BitmappedImage = GetBitmapSource(userDto.Image);
            return userDto;
        }

        private BitmapSource GetBitmapSource(byte[] imageBytes)
        {
            try
            {
                using (MemoryStream mStream = new MemoryStream(imageBytes))
                {
                    System.Drawing.Image imageFromBytes = System.Drawing.Image.FromStream(mStream);

                    mStream.Position = 0;
                    BitmapDecoder decoder = BitmapDecoder.Create(
                        mStream,
                        BitmapCreateOptions.PreservePixelFormat,
                        BitmapCacheOption.OnLoad);

                    return decoder.Frames[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string ReadEncodedUserDataFromLocalFile(string filePath)
        {
            string userData = String.Empty;
            try
            {
                if (File.Exists(filePath))
                {
                    userData = File.ReadAllText(filePath);
                }
                return userData;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

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

        private void CheckIsUserAdmin()
        {
            if (CurrentUser.UserStatus == Models.Enums.UserStatus.Admin)
            {
                var test = new KeyValuePair<string, DelegateCommand>(_manageUsersButtonName, OpenUserManagementCommand);
                var isCommangActive = OnlineUserNavButtons.Contains(test);
                if (!isCommangActive)
                {
                    OnlineUserNavButtons.Add(_manageUsersButtonName, OpenUserManagementCommand);
                }
            }
        }

        #endregion
    }
}
