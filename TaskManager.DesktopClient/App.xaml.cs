using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.DesktopClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var resourceDictionaty = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Resources/Styles/MainStyleDictionaty.xaml")
            };
            Resources.MergedDictionaries.Add(resourceDictionaty);
            new /*TaskManager.DesktopClient.Views.*/MainWindow().ShowDialog();

            Shutdown();
        }
    }
}
