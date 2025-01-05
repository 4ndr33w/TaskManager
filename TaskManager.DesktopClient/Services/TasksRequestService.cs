using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.Models;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.DesktopClient.Services
{
    public class TasksRequestService : BaseRequestService<TaskDto>
    {
        protected override string GetApiUrlString()
        {
            return HOST + tasksApiUrl;
        }

        public async Task<IEnumerable<TaskModel>> GetAllAsync(AuthToken token)
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

                    var entity = JsonSerializer.Deserialize<IEnumerable<TaskModel>>(result, options).ToList();
                    return entity;
                }
            }
            catch (Exception)
            {
                return new List<TaskModel>();
            }
            return new List<TaskModel>();
        }
    }
}
