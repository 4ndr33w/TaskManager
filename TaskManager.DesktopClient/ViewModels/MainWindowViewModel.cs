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
using TaskManager.DesktopClient.Services.ViewServices;
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
        private readonly ImageLoadSaveService _imageLoadSaveService;
        private readonly UsersRequestService _usersRequestService;
        private readonly BaseViewService _viewService;
        private readonly LoginUserService _loginUserService;

        public MainWindowViewModel()
        {
            _viewService = new Services.ViewServices.BaseViewService();
            _imageLoadSaveService = new ImageLoadSaveService();
            _usersRequestService = new UsersRequestService();
            _loginUserService = new LoginUserService();

            OnStartup();
        }

        public MainWindowViewModel(AuthToken token, UserDto? onlineUser, UserDto localUser, Window currentWindow = null)
        {
            Token = token;
            CurrentUser = onlineUser;
            LocalUser = localUser;
            CurrentWindow = currentWindow;

            DateFormat = CurrentUser == null? DateTime.Now.ToString("U") : CurrentUser.Created.ToString("U");

            _viewService = new Services.ViewServices.BaseViewService();
            _imageLoadSaveService = new ImageLoadSaveService();
            _usersRequestService = new UsersRequestService();
            _loginUserService = new LoginUserService();

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


        private byte[] _imageBytes;

        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set { _imageBytes = value; }
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
        private readonly string _EditUserPageName = "Edit user profile";

        public readonly string CollectiveProjectsLabelString = "Collective Projects Work";

        #endregion


        public string Login { get; set; }
        public string Password { get; private set; }

        private readonly string _savedUserLoginFilename = StaticResources.CachedUserFileName;
        private readonly string _savedUserLoginLocalFilePath = StaticResources.CachedUserFilePath;
        private readonly string _environmentPath = Environment.SpecialFolder.MyDocuments.ToString();

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

        public DelegateCommand EditUserPageCommand { get; private set; }
        public DelegateCommand ChangeImageCommand { get; private set; }

        #region Other 
        public DelegateCommand<object> RegisterNewUserCommand { get; private set; }
        public DelegateCommand SearchImageFileCommand { get; private set; }

        public DelegateCommand<object> SaveEditedUserCommand { get; private set; }

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
            BitmappedImage = _imageLoadSaveService.GetBitmapSource(CurrentUser.Image);
            page.DataContext = this;
            OpenPage(page, _userInfoButtonName, this);
        }
        private void OpenUserManagement()
        {
            SelectedPageName = _manageUsersButtonName;
            _viewService.ShowMessage(_manageUsersButtonName);
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
            OpenPage(page, _userProjectsButtonName, new ProjectsPageViewModel(Token, page, this));
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
        private void EditUserPage()
        {
            var page = new EditUserPage();
            SelectedPageName = _EditUserPageName;
            BitmappedImage = _imageLoadSaveService.GetBitmapSource(CurrentUser.Image);
            page.DataContext = this;
            OpenPage(page, _EditUserPageName, this);
        }

        private async void ChangeImage()
        {
            var result = MessageBox.Show("Изменить изображение?", "Choose new image file", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ImageBytes = await _imageLoadSaveService.SearchImageFile();
                CurrentUser.Image = ImageBytes;
            }
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

            var result = await _usersRequestService.CreateAsync(userDto);

            (parameter as Window).Hide();
        }

        public async void SaveEditedUser(object parameter)
        {
            var page = (parameter as Views.Pages.EditUserPage);


            CurrentUser.Name = page.EditedUserNameTextBox.Text;
            CurrentUser.LastName = page.EditedUserLastNameTextBox.Text;
            CurrentUser.Phone = page.EditedUserPhoneTextBox.Text;
            //CurrentUser.Image = 

            var result = await _usersRequestService.UpdateAsync(Token, CurrentUser);

            var savedUserCache = await _loginUserService.LoadUserCache(_savedUserLoginFilename);
            if (savedUserCache.Id == CurrentUser.Id)
            {
                result = await _loginUserService.SaveUserCache(CurrentUser, _savedUserLoginFilename);
                CurrentUser = await _loginUserService.LoadUserCache(_savedUserLoginFilename);

            }
            OpenMyInfoPage();
        }

        #endregion

        #region OnStartup
        private async void OnStartup()
        {
            #region otherCommands 

            RegisterNewUserCommand = new DelegateCommand<object>(RegisterNewUser);
            EditUserPageCommand = new DelegateCommand(EditUserPage);
            ChangeImageCommand = new DelegateCommand(ChangeImage);
            SaveEditedUserCommand = new DelegateCommand<object>(SaveEditedUser);

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

                BitmappedImage = _imageLoadSaveService.GetBitmapSource(CurrentUser.Image);

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

        #region Load / Save user Cache 

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
     
        #endregion

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
