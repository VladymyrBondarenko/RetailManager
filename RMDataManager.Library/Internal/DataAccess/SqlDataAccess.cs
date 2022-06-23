using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.Internal.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqlDataAccess> _logger;

        public SqlDataAccess(IConfiguration configuration, ILogger<SqlDataAccess> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            var res = _configuration.GetConnectionString(name);
            return res;
        }

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, string connectionStringName, U parameters = default)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                var rows = await connection
                    .QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public async Task SaveData<T>(string storedProcedure, string connectionStringName, T parameters = default)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public ISqlDataTransactionAccess StartTransaction(string connectionStringName)
        {
            var transaction = new SqlDataTransactionAccess(_configuration, _logger);
            transaction.StartTransaction(connectionStringName);

            return transaction;
        }
    }
}
