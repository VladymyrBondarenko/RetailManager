using RetailManagerDesktopUI.Library.Models;
using RetailManagerDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api
{
    public class RestServiceCaller : IRestServiceCaller
    {
        private HttpClient _httpClient;
        private readonly ILoggedInUserModel _loggedInUser;

        public RestServiceCaller(ILoggedInUserModel loggedInUser)
        {
            initilizeClient();
            _loggedInUser = loggedInUser;
        }

        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        private void initilizeClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (var httpResponse = await _httpClient.PostAsync("/Token", content))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }

        public void LogOffUser()
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task GetLoggedInUser(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (var httpResponse = await _httpClient.GetAsync("api/User"))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }
    }
}
