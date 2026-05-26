using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Shared.Settings;

namespace Kron.Counting.Infrastructure.Data;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly DatabaseSettings _databaseSettings;

    public SqlConnectionFactory(IOptions<DatabaseSettings> databaseSettings)
    {
        _databaseSettings = databaseSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_databaseSettings.ConnectionString);
    }
}