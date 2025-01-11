using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using TaskManager.Models.Content;
using TaskManager.Models.Enums;

namespace TaskManager.DesktopClient.Services.Abstractions
{
    public class BaseRequestService<T> where T : class
    {
        protected readonly string HOST = Resources.StaticResources.HOST;
        protected readonly string usersApiUrl = Resources.StaticResources.UsersApiUrl;
        protected readonly string tokenUrl = Resources.StaticResources.TokenUrl;
        protected readonly string projectsApiUrl = Resources.StaticResources.ProjectsApiUrl;
        protected readonly string desksApiUrl = Resources.StaticResources.DesksApiUrl;
        protected readonly string tasksApiUrl = Resources.StaticResources.TasksApiUrl;

        protected virtual string GetApiUrlString()
        {
            return HOST;
        }

        protected HttpClient GetHttpClient(Models.Content.DataContent httpContent)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddHttpClient();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IHttpClientFactory httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            var client = httpClientFactory?.CreateClient();
            var clientWithHeadersAndContent = GetHeadersForHttpClient(client, httpContent);
            return clientWithHeadersAndContent;
        }

        private HttpClient GetHeadersForHttpClient(HttpClient httpClient, Models.Content.DataContent httpContent)
        {
            Uri requestUri = new Uri(HOST + httpContent.Url);

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.BaseAddress = requestUri;
            httpClient.DefaultRequestHeaders.Add("AcceptCharset", CharSet.Unicode.ToString());

            switch (httpContent.AuthorizationType)
            {
                case AuthorizationType.Basic:
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", httpContent.Content);
                        break;
                    }
                case AuthorizationType.Bearer:
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpContent.Token);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return httpClient;
        }

        private async Task<HttpResponseMessage> HttpPostMethod(HttpClient client, Models.Content.DataContent content)
        {
            Uri usersApiUri = new Uri(content.Url);

            switch (content.AuthorizationType)
            {
                case AuthorizationType.Basic:
                    {
                        return await client.PostAsync(usersApiUri, new StringContent(content.Content));
                    }
                default:
                    {
                        T entity = JsonSerializer.Deserialize<T>(content.Content);
                        JsonContent jsonContent = JsonContent.Create(entity);
                        return await client.PostAsync(usersApiUri, jsonContent);
                    }
            }
        }
        private string EncodedLoginPassString(Models.Content.DataContent httpContent)
        {
            string encodingType = Resources.StaticResources.EncodingType;
            return Convert.ToBase64String(Encoding.GetEncoding(encodingType).GetBytes(httpContent.Login + ":" + httpContent.Password));
        }

        protected async Task<string> GetDataByUrl(Models.Content.DataContent httpContent)
        {
            string result = "";
            Uri requestUri = new Uri(httpContent.Url);

            if (httpContent != null)
            {
                HttpClient httpClient = GetHttpClient(httpContent);

                var httpResponse = new HttpResponseMessage();

                switch (httpContent.HttpMethod)
                {
                    case Models.Enums.HttpMethod.GET:
                        {
                            httpResponse = await httpClient.GetAsync(requestUri);
                            break;
                        }
                    case Models.Enums.HttpMethod.POST:
                        {
                            httpResponse = await HttpPostMethod(httpClient, httpContent);
                            break;
                        }

                    case Models.Enums.HttpMethod.DELETE:
                        {
                            httpResponse = await httpClient.DeleteAsync(requestUri);
                            break;
                        }
                    case Models.Enums.HttpMethod.PATCH:
                        {
                            T entity = JsonSerializer.Deserialize<T>(httpContent.Content);
                            JsonContent jsonContent = JsonContent.Create(entity);
                            httpResponse = await httpClient.PatchAsync(requestUri, jsonContent);
                            break;
                        }
                }
                result = await httpResponse.Content.ReadAsStringAsync();
            }
            return result;
        }

        //---------------------------------------------
        public async Task<AuthToken> GetToken(string login, string password)
        {
            var token = new AuthToken();
            string url = HOST + tokenUrl;

            Models.Content.DataContent httpContent = new Models.Content.DataContent
            {
                Login = login,
                Password = password,
                Url = url,
                Content = string.Empty,
                HttpMethod = Models.Enums.HttpMethod.POST,
                AuthorizationType = AuthorizationType.Basic
            };

            Uri requestUri = new Uri(url);

            if (login != null && password != null)
            {
                string encodedLoginAdnPassword = EncodedLoginPassString(httpContent);
                httpContent.Content = encodedLoginAdnPassword;

                var result = await GetDataByUrl(httpContent);

                token = JsonSerializer.Deserialize<AuthToken>(result);
            }
            return token;
        }
        //------------------------------------------------

        public async Task<string> CreateAsync(T entity, AuthToken token = null)
        {
            string url = GetApiUrlString();
            string userJson = JsonSerializer.Serialize(entity);

            Models.Content.DataContent content = new Models.Content.DataContent
            {
                Content = userJson,
                HttpMethod = Models.Enums.HttpMethod.POST,
                Url = url,
                AuthorizationType = token == null ? AuthorizationType.NoAuth : AuthorizationType.Bearer,
                Login = token == null ? null : token.userName,
                Token = token == null ? null : token.accessToken
            };
            var result = await GetDataByUrl(content);

            return result;
        }
        public async Task<bool> DeleteAsync(AuthToken token, Guid id = default)
        {
            string url = GetApiUrlString();
            if (id != default)
            {
                url += id;
            }
            Models.Content.DataContent content = new Models.Content.DataContent
            {
                Login = token.userName,
                Token = token.accessToken,
                Url = url,
                AuthorizationType = AuthorizationType.Bearer,
                HttpMethod = Models.Enums.HttpMethod.DELETE
            };
            var result = await GetDataByUrl(content);

            if (result == "true")
            {
                return true;
            }
            else return false;
        }
        public async Task<T> GetAsync(AuthToken token, Guid id = default)
        {
            try
            {
                string url = GetApiUrlString();
                if (id != default)
                {
                    url += id.ToString();
                }
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

                var entity = JsonSerializer.Deserialize<T>(result, options);
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> UpdateAsync(AuthToken token, T Entity, Guid id = default)
        {
            string url = GetApiUrlString();
            if (id != default)
            {
                url += id.ToString();
            }

            Models.Content.DataContent content = new Models.Content.DataContent
            {
                AuthorizationType = AuthorizationType.Bearer,
                Url = url,
                HttpMethod = Models.Enums.HttpMethod.PATCH,
                Login = token.userName,
                Token = token.accessToken,
                Content = JsonSerializer.Serialize(Entity)
            };

            var result = await GetDataByUrl(content);
            if (result == "true")
            {
                return true;
            }
            else return false;
        }
    }
}