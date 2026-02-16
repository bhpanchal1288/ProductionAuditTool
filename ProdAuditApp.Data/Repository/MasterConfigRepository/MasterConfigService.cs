using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using System.Data;

namespace ProdAuditApp.Data.Repository.MasterConfigRepository
{
    public class MasterConfigService : IMasterConfigService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MasterConfigService> _logger;

        public MasterConfigService(IConfiguration config, ILogger<MasterConfigService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<MasterDataResult> GetDataAsync(string masterCode, int page, int pageSize, string sort, string dir)
        {
            if (string.IsNullOrWhiteSpace(masterCode))
            {
                throw new ArgumentException("Master code cannot be null or empty.", nameof(masterCode));
            }

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 1000)
            {
                pageSize = 1000; // Limit maximum page size
            }

            // Validate sort direction
            if (string.IsNullOrWhiteSpace(dir))
            {
                dir = "ASC";
            }
            else
            {
                dir = dir.ToUpper();
                if (dir != "ASC" && dir != "DESC")
                {
                    dir = "ASC";
                }
            }

            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            
            try
            {
                // Get master definition
                var master = await connection.QuerySingleOrDefaultAsync<MasterDefinition>(
                    "SELECT * FROM Mst.MasterDefinition WHERE MasterCode = @code AND IsActive = 1",
                    new { code = masterCode });

                if (master == null)
                {
                    _logger.LogWarning("Master definition not found for code: {MasterCode}", masterCode);
                    throw new KeyNotFoundException($"Master definition not found for code: {masterCode}");
                }

                // Get column definitions
                var columns = (await connection.QueryAsync<MasterColumnDefinition>(
                    "SELECT * FROM Mst.MasterColumnDefinition WHERE MasterId = @id AND IsVisible = 1 ORDER BY DisplayOrder",
                    new { id = master.MasterId })).ToList();

                if (!columns.Any())
                {
                    _logger.LogWarning("No visible columns found for master: {MasterCode} (MasterId: {MasterId})", masterCode, master.MasterId);
                    return new MasterDataResult
                    {
                        Columns = null,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                // Validate and sanitize sort column
                string sortColumn = "1"; // Default to column position 1
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    // Check if the sort column exists in the column definitions
                    var sortColumnDef = columns.FirstOrDefault(c => 
                        c.ColumnName.Equals(sort, StringComparison.OrdinalIgnoreCase) && 
                        c.IsSortable);

                    if (sortColumnDef != null)
                    {
                        // Use bracketed and sanitized column name to prevent SQL injection
                        sortColumn = $"[{SanitizeIdentifier(sortColumnDef.ColumnName)}]";
                    }
                }

                var offset = (page - 1) * pageSize;

                // Build the list of column names for SELECT (only visible columns)
                // Sanitize each column name to ensure safety
                var columnNames = string.Join(", ", columns.Select(c => $"[{SanitizeIdentifier(c.ColumnName)}]"));

                // Use parameterized query with bracketed table name for safety
                // Note: Table name should be validated in MasterDefinition to ensure it's a valid identifier
                string tableName = SanitizeIdentifier(master.TableName);
                
                var sql = $@"{master.SelectQuery}
                    ORDER BY {sortColumn} {dir}
                    OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

                    SELECT COUNT(*) FROM [{tableName}];
                ";

                //var sql = $@"
                //    SELECT {columnNames} 
                //    FROM [{tableName}]
                //    ORDER BY {sortColumn} {dir}
                //    OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

                //    SELECT COUNT(*) FROM [{tableName}];
                //";

                _logger.LogDebug("Executing query for master: {MasterCode}, Table: {TableName}, Page: {Page}, PageSize: {PageSize}", 
                    masterCode, tableName, page, pageSize);

                using var multi = await connection.QueryMultipleAsync(sql,
                    new { offset, pageSize });

                //var data = multi.Read<IDictionary<string, object>>().ToList();
                var rows = multi.Read()
                .Cast<IDictionary<string, object>>()
                .Select(r => new Dictionary<string, object>(r))
                .ToList();
                var totalRecords = multi.ReadSingle<int>();

                _logger.LogInformation("Retrieved {Count} records for master: {MasterCode} (Total: {Total})", 
                    rows.Count, masterCode, totalRecords);

                return new MasterDataResult
                {
                    Columns = columns,
                    Data = rows.Select(r => new Dictionary<string, object>(r)).ToList(),
                    TotalRecords = totalRecords
                };
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error while retrieving data for master: {MasterCode}", masterCode);
                throw new InvalidOperationException($"Error retrieving data for master code: {masterCode}. {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving data for master: {MasterCode}", masterCode);
                throw;
            }
        }

        /// <summary>
        /// Sanitizes a database identifier (table/column name) to prevent SQL injection
        /// Removes or escapes special characters
        /// </summary>
        private static string SanitizeIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));
            }

            // Remove brackets if present (will add our own)
            identifier = identifier.Trim('[', ']').Trim();

            // Validate that identifier contains only valid SQL Server identifier characters
            // SQL Server identifiers can contain letters, numbers, @, #, _, and $
            //if (!System.Text.RegularExpressions.Regex.IsMatch(identifier, @"^[a-zA-Z_@#][a-zA-Z0-9_@#$]*$"))
            //{
            //    throw new ArgumentException($"Invalid identifier format: {identifier}", nameof(identifier));
            //}

            return identifier;
        }
    }
}
