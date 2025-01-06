using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.Models;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.DesktopClient.Services
{
    public class TasksRequestService : BaseRequestService<TaskDto>
    {
        private readonly UsersRequestService _usersRequestService;
        private readonly DesksRequestService _desksRequestService;

        protected override string GetApiUrlString()
        {
            return HOST + tasksApiUrl;
        }

        public TasksRequestService(UsersRequestService usersRequestService, DesksRequestService desksRequestService)
        {
            _usersRequestService = usersRequestService;
            _desksRequestService = desksRequestService;
        }


        public async Task<IEnumerable<ClientSideTaskModel>> GetAllAsync(AuthToken token)
        {
            try
            {
                string url = GetApiUrlString();

                if (token.accessToken != String.Empty)
                {
                    Models.Content.DataContent httpContent = new Models.Content.DataContent
                    {
                        HttpMethod = Models.Enums.HttpMethod.GET,
                        AuthorizationType = AuthorizationType.Bearer,
                        Url = url,
                        Token = token.accessToken,
                        Login = token.userName
                    };

                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;
                    var result = await GetDataByUrl(httpContent);

                    var deserializedTasksCollection = JsonSerializer.Deserialize<IEnumerable<TaskModel>>(result, options).ToList();



                    var clientTaskModelCollection = new List<ClientSideTaskModel>();

                    foreach (var task in deserializedTasksCollection)
                    {
                        var clientTaskModel = new ClientSideTaskModel(task);

                        clientTaskModel.Creator = new User( await _usersRequestService.GetAsync(token, task.CreatorId) );
                        clientTaskModel.Executor = new User(await _usersRequestService.GetAsync(token, task.ExecutorId));
                        clientTaskModel.Desk = new Desk(await _desksRequestService.GetAsync(token, task.DeskId));


                        clientTaskModelCollection.Add(clientTaskModel);
                    }
                    return clientTaskModelCollection;
                }
            }
            catch (Exception)
            {
                return new List<ClientSideTaskModel>();
            }
            return new List<ClientSideTaskModel>();
        }
    }
}
