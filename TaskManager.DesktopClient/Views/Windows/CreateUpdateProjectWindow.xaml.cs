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
using System.Windows.Shapes;

using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateUpdateProjectWindow.xaml
    /// </summary>
    public partial class CreateUpdateProjectWindow : Window
    {
        private Models.ClientModels.ClientModel<ProjectDto> SelectedProject { get; set; }

        private byte[] _picture;
        public byte[] Picture
        {
            get => _picture;
            set
            {
                _picture = value;
            }
        }

        public CreateUpdateProjectWindow()
        {
            //SelectedProject = new Models.ClientModels.ClientModel<ProjectDto>();
            //ProjectStatusComboBox.Items.Clear();

            //ProjectStatusComboBox.ItemsSource = null;
            InitializeComponent();
        }

        public CreateUpdateProjectWindow(Models.ClientModels.ClientModel<ProjectDto> projectDto)
        {
            SelectedProject = projectDto;
            InitializeComponent();
        }
    }
}
