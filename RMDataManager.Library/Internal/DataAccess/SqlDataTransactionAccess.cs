﻿using Dapper;
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
    internal class SqlDataTransactionAccess : ISqlDataTransactionAccess
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly ISqlDataAccess _db;
        private bool _isClosed = false;
        private IConfiguration _configuration;
        private readonly ILogger<SqlDataAccess> _logger;

        public SqlDataTransactionAccess(IConfiguration configuration, ILogger<SqlDataAccess> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            var res = _configuration.GetConnectionString(name);
            return res;
        }

        public async Task SaveDataInTransaction<T>(string storedProcedure, T parameters = default)
        {
            await _connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public async Task<List<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters = default)
        {
            var rows = await _connection
                .QueryAsync<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transaction);

            return rows.ToList();
        }

        public void StartTransaction(string connectionStringName)
        {
            _connection = new SqlConnection(GetConnectionString(connectionStringName));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            _isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            _isClosed = true;
        }

        public void Dispose()
        {
            if(!_isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex,
                        "Commit transaction failed in the dispose method");
                }
            }

            _connection = null;
            _transaction = null;
        }
    }
}