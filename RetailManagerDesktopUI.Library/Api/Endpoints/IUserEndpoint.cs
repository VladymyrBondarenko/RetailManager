using RetailManagerDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public interface IUserEndpoint
    {
        Task AddToRole(string userId, string roleId);
        Task<List<UserModel>> GetAll();
        Task<Dictionary<string, string>> GetAllRoles();
        Task RemoveFromRole(string userId, string roleId);
    }
}