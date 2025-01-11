using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;
using TaskManager.Models.ShortInfoModels;

namespace TaskManager.DesktopClient.Services
{
    public class UsersRequestService : BaseRequestService<UserDto>
    {
        protected override string GetApiUrlString()
        {
            return HOST + usersApiUrl;
        }
        private readonly string userInfoUri = StaticResources.UsersInfoUri;


        public async Task<List<UserInfo>> GetUsersInfoAsync(AuthToken token)
        {
            try
            {
                string url = GetApiUrlString() + userInfoUri;
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

                var entity = JsonSerializer.Deserialize<List<UserInfo>>(result, options);
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
