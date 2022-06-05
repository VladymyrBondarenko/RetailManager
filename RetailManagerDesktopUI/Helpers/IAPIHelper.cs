using RetailManagerDesktopUI.Models;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}