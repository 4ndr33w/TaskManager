using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnStartup();
        }

        public MainWindow(AuthToken token, UserDto onlineUser, UserDto localUser)
        {
            InitializeComponent();
            OnStartup(token, onlineUser, localUser);
        }

        private void OnStartup()
        {
            DataContext = new ViewModels.MainWindowViewModel();
        }
        private void OnStartup(AuthToken token, UserDto onlineUser, UserDto localUser)
        {
            DataContext = new ViewModels.MainWindowViewModel(token, onlineUser, localUser);
        }
    }
}
