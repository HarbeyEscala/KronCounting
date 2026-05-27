using Dapper;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Infrastructure.Repositories;

public sealed class DeviceReadingRepository : IDeviceReadingRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DeviceReadingRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAsync(
        DeviceReading reading,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.DeviceReadings
            (
                DeviceId,
                ReadingTimestampUtc,
                PeopleIn,
                PeopleOut,
                Occupancy,
                ConfidenceScore,
                RawPayloadJson,
                CreatedAtUtc
            )
            VALUES
            (
                @DeviceId,
                @ReadingTimestampUtc,
                @PeopleIn,
                @PeopleOut,
                @Occupancy,
                @ConfidenceScore,
                @RawPayloadJson,
                @CreatedAtUtc
            );

            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<long>(sql, reading);
    }

    public async Task<IEnumerable<DeviceReading>> GetByDeviceIdAsync(
        Guid deviceId,
        DateTime? fromUtc = null,
        DateTime? toUtc = null,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                DeviceId,
                ReadingTimestampUtc,
                PeopleIn,
                PeopleOut,
                Occupancy,
                ConfidenceScore,
                RawPayloadJson,
                CreatedAtUtc
            FROM dbo.DeviceReadings
            WHERE DeviceId = @DeviceId
              AND (@FromUtc IS NULL OR ReadingTimestampUtc >= @FromUtc)
              AND (@ToUtc IS NULL OR ReadingTimestampUtc <= @ToUtc)
            ORDER BY ReadingTimestampUtc DESC;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<DeviceReading>(
            sql,
            new
            {
                DeviceId = deviceId,
                FromUtc = fromUtc,
                ToUtc = toUtc
            });
    }
}