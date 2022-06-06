using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;
        private readonly string connectionId = "RetailManagerDataConnection";

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<UserModel>> GetUserById(string id)
        {
            return await _db.LoadData<UserModel, dynamic>(
                "dbo.spUserLookup",
                new { id = id },
                connectionId);
        }
    }
}
