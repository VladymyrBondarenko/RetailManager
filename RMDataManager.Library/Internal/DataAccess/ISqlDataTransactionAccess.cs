using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Internal.DataAccess
{
    public interface ISqlDataTransactionAccess : IDisposable
    {
        void CommitTransaction();
        Task<List<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters = default);
        void RollbackTransaction();
        Task SaveDataInTransaction<T>(string storedProcedure, T parameters = default);
        void StartTransaction(string connectionStringName);
    }
}