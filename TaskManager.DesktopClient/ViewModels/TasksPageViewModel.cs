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
        //public List<ClientSideTaskModel> TasksCollection = new List<ClientSideTaskModel> { };

        public TasksPageViewModel() { }
        public TasksPageViewModel(AuthToken token)
        {
            _token = token;
            _tasksRequestService = new TasksRequestService(_usersRequestService, _deskRequestsService);
            OnStartup();
        }


        private ObservableCollection<ClientSideTaskModel> _tasksColection;
        public ObservableCollection<ClientSideTaskModel>  TasksColection
        {
            get => _tasksColection;
            set
            {
                _tasksColection = value;
                RaisePropertyChanged(nameof(TasksColection));
            }
        }

        private async void OnStartup()
        {
            TasksColection = await GetAllTasks(_token);
        }
        private async Task<ObservableCollection<ClientSideTaskModel>> GetAllTasks(AuthToken token)
        {
            var tasksCollection = await _tasksRequestService.GetAllAsync(token);


            var result = new ObservableCollection<ClientSideTaskModel>();

            foreach (var task in tasksCollection)
            {
                result.Add(task);
            }

            return result;
        }
    }
}
