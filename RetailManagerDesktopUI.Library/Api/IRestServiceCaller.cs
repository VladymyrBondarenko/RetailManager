using RetailManagerDesktopUI.Models;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api
{
    public interface IRestServiceCaller
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUser(string token);
    }
}