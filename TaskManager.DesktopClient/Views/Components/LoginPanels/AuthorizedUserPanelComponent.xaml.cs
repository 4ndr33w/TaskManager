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

namespace TaskManager.DesktopClient.Views.Components.LoginPanels
{
    /// <summary>
    /// Логика взаимодействия для AuthorizedUserPanelComponent.xaml
    /// </summary>
    public partial class AuthorizedUserPanelComponent : UserControl
    {
        public AuthorizedUserPanelComponent()
        {
            InitializeComponent();
            OnStartup();
        }
        private void OnStartup()
        {
            DataContext = new ViewModels.MainWindowViewModel();
        }
    }
}
