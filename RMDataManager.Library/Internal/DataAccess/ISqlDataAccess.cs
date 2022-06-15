using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string name);
        Task<List<T>> LoadData<T, U>(string storedProcedure, string connectionStringName, U parameters = default);
        Task SaveData<T>(string storedProcedure, string connectionStringName, T parameters = default);
        ISqlDataTransactionAccess StartTransaction(string connectionStringName);
    }
}