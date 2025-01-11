using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DesktopClient.Resources
{
    public class TextData
    {
        public static readonly string CreateNewProjectString = "Create New Project";


        public static readonly string UserInfoButtonName = "Profile Info";
        public static readonly string ManageUsersButtonName = "Users Management";
        public static readonly string LogoutButtonName = "Log Out";

        public static readonly string UserProjectsButtonName = "Collective Projects";
        public static readonly string UserDesksButtonName = "Collective Desks";
        public static readonly string UserTasksButtonName = "Collective Tasks";


        public static readonly string LocalProjectsButtonName = "Local Projects";
        public static readonly string LocalDesksButtonName = "Local Desks";
        public static readonly string LocalTasksButtonName = "Local Tasks";
        public static readonly string EditUserPageName = "Edit user profile";

        public static readonly string CollectiveProjectsLabelString = "Collective Projects Work";

        #region LoginWindowTexts 

        public static readonly string ExitButtonString = "Exit";
        public static readonly string RegistrationButtonString = "Registration";
        public static readonly string LocalUserButtonString = "OK";
        public static readonly string LastUserButtonString = "Last user: ";
        public static readonly string EnterLoginString = "Enter Login: ";
        public static readonly string EnterPasswordString = "Enter Password: ";

        #endregion

        #region Common Text Fields Region 

        public static readonly string OkButtonString = "OK";
        public static readonly string CancelButtonString = "Cancel";

        #endregion


        public static readonly string test = @"..\\..\\..\Resources\Images\load.gif";

        public static readonly string appRelativeDirectory = @"..\\..\\..";
        public static readonly string LoadingGifRelativePath = @"\Resources\Images\load.gif";
        public static readonly string LoadingGifFilePath = appRelativeDirectory + LoadingGifRelativePath;

    }
}
