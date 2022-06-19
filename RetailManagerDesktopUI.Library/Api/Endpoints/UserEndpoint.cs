using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IRestServiceCaller _restServiceCaller;

        public UserEndpoint(IRestServiceCaller restServiceCaller)
        {
            _restServiceCaller = restServiceCaller;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (var httpResponse = await _restServiceCaller.HttpClient.GetAsync("api/User/Admin/GetAllUsers"))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var res = await httpResponse.Content.ReadAsAsync<List<UserModel>>();
                    return res;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }

        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            using (var httpResponse = await _restServiceCaller.HttpClient.GetAsync("api/User/Admin/GetAllRoles"))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var res = await httpResponse.Content.ReadAsAsync<Dictionary<string, string>>();
                    return res;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }

        public async Task AddToRole(string userId, string roleName)
        {
            using (var httpResponse = 
                await _restServiceCaller.HttpClient.PostAsJsonAsync(
                    "api/User/Admin/AddToRole", new { userId, roleName }))
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }

        public async Task RemoveFromRole(string userId, string roleName)
        {
            using (var httpResponse =
                await _restServiceCaller.HttpClient.PostAsJsonAsync(
                    "api/User/Admin/RemoveFromRole", new { userId, roleName }))
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }
    }
}