using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Content;
using TaskManager.Models;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.DesktopClient.Services
{
    internal class ProjectsRequestService : BaseRequestService<ProjectDto>
    {
        protected override string GetApiUrlString()
        {
            return HOST + projectsApiUrl;
        }

        public async Task<List<Models.ClientModels.ClientModel<ProjectDto>>> GetAllAsync(AuthToken token)
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

                    var deserializedProjectsCollection = JsonSerializer.Deserialize<IEnumerable<ProjectDto>>(result, options).ToList();

                    var clientProjectsModelCollection = new List<Models.ClientModels.ClientModel<ProjectDto>>();

                    foreach (var project in deserializedProjectsCollection)
                    {
                        var clientProjectModel = new ClientModel<ProjectDto>(project);

                        if (!clientProjectsModelCollection.Contains(clientProjectModel))
                        {
                            clientProjectsModelCollection.Add(clientProjectModel);
                        }
                    }
                    return clientProjectsModelCollection;
                }
                return new List<Models.ClientModels.ClientModel<ProjectDto>>();
            }
            catch (Exception)
            {
                return new List<Models.ClientModels.ClientModel<ProjectDto>>();
            }
        }
    }
}
