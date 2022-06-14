using RetailManagerDesktopUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api
{
    public interface IRestServiceCaller
    {
        HttpClient HttpClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUser(string token);
        void LogOffUser();
    }
}