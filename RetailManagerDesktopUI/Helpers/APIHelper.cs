using RetailManagerDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient;

        public APIHelper()
        {
            initilizeClient();
        }

        private void initilizeClient()
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (var httpResponse = await _apiClient.PostAsync("/Token", content))
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
    }
}
