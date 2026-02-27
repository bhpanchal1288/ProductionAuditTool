using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProdAuditApp.Data.Model.Domain;
using System.Data;

namespace ProdAuditApp.Data.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task Save<T>(string spName, T parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<ITMessage> SaveData<ITMessage>(string spName, object parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            var result = await connection.QueryFirstOrDefaultAsync<ITMessage>(spName, parameters, commandType: CommandType.StoredProcedure);
            return result;
        }


    }
}
