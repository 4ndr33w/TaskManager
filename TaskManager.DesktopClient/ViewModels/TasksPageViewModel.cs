using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

using TaskManager.DesktopClient.Services;
using TaskManager.Models;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Content;

namespace TaskManager.DesktopClient.ViewModels
{
    public class TasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private readonly UsersRequestService _usersRequestService = new UsersRequestService();
        private readonly DesksRequestService _deskRequestsService = new DesksRequestService();

        public TasksPageViewModel() { }
        public TasksPageViewModel(AuthToken token)
        {
            _token = token;
            _tasksRequestService = new TasksRequestService(_usersRequestService, _deskRequestsService);
        }


        private List<ClientSideTaskModel> _tasksColection;
        public List<ClientSideTaskModel>  TasksColection
        {
            get => _tasksColection;
            set
            {
                _tasksColection = value;
                RaisePropertyChanged(nameof(TasksColection));
            }
        }

        private async Task<IEnumerable<ClientSideTaskModel>> GetAllTasks(AuthToken token)
        {
            var tasks = await _tasksRequestService.GetAllAsync(token);

            return tasks;
        }
        
        //public AuthToken Token
        //{
        //    get => _token;
        //    set
        //    {
        //        _token = value;
        //        RaisePropertyChanged(nameof(Token));
        //    }
        //}

       

    }
}
