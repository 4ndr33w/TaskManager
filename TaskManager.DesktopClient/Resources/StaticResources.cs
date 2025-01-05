using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DesktopClient.Resources
{
    public static class StaticResources
    {
        private static Resources.Settings.AppSettings _settings = new Settings.AppSettings();

        public static string HOST = StaticResources._settings.HOST;
        public static string UsersApiUrl = StaticResources._settings.UsersApi;
        public static string TokenUrl = StaticResources._settings.TokenUrl;
        public static string ProjectsApiUrl = StaticResources._settings.ProjectsApi;
        public static string DesksApiUrl = StaticResources._settings.DesksApi;
        public static string TasksApiUrl = StaticResources._settings.TasksApi;

        public static string CachedUserFileName = StaticResources._settings.CachedUserFileName;
        public static string LocalUserFileName = StaticResources._settings.LocalUserFileName;
        public static string CachedUserFilePath = StaticResources._settings.SavedLoginLocalPath;

        public static string EncodingType = StaticResources._settings.EncodingType;
    }
}
