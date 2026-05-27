using Dapper;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Infrastructure.Repositories;

public sealed class DashboardRepository : IDashboardRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DashboardRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<LiveDashboardSnapshot?> GetSnapshotByStoreIdAsync(
        Guid storeId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                StoreId,
                CurrentOccupancy,
                TodayIn,
                TodayOut,
                LastReadingAtUtc,
                UpdatedAtUtc
            FROM dbo.LiveDashboardSnapshots
            WHERE StoreId = @StoreId;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<LiveDashboardSnapshot>(
            sql,
            new { StoreId = storeId });
    }

    public async Task UpsertSnapshotAsync(
        LiveDashboardSnapshot snapshot,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            MERGE dbo.LiveDashboardSnapshots AS target
            USING
            (
                SELECT
                    @Id AS Id,
                    @StoreId AS StoreId,
                    @CurrentOccupancy AS CurrentOccupancy,
                    @TodayIn AS TodayIn,
                    @TodayOut AS TodayOut,
                    @LastReadingAtUtc AS LastReadingAtUtc,
                    @UpdatedAtUtc AS UpdatedAtUtc
            ) AS source
            ON target.StoreId = source.StoreId

            WHEN MATCHED THEN
                UPDATE SET
                    CurrentOccupancy = source.CurrentOccupancy,
                    TodayIn = source.TodayIn,
                    TodayOut = source.TodayOut,
                    LastReadingAtUtc = source.LastReadingAtUtc,
                    UpdatedAtUtc = source.UpdatedAtUtc

            WHEN NOT MATCHED THEN
                INSERT
                (
                    Id,
                    StoreId,
                    CurrentOccupancy,
                    TodayIn,
                    TodayOut,
                    LastReadingAtUtc,
                    UpdatedAtUtc
                )
                VALUES
                (
                    source.Id,
                    source.StoreId,
                    source.CurrentOccupancy,
                    source.TodayIn,
                    source.TodayOut,
                    source.LastReadingAtUtc,
                    source.UpdatedAtUtc
                );
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, snapshot);
    }

    public async Task<IEnumerable<StoreHourlyMetric>> GetHourlyMetricsAsync(
        Guid storeId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                StoreId,
                MetricDate,
                MetricHour,
                PeopleIn,
                PeopleOut,
                PeakOccupancy,
                AvgOccupancy
            FROM dbo.StoreHourlyMetrics
            WHERE StoreId = @StoreId
              AND MetricDate BETWEEN @FromDate AND @ToDate
            ORDER BY MetricDate, MetricHour;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<StoreHourlyMetric>(
            sql,
            new
            {
                StoreId = storeId,
                FromDate = fromDate,
                ToDate = toDate
            });
    }

    public async Task<IEnumerable<StoreDailyMetric>> GetDailyMetricsAsync(
        Guid storeId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                StoreId,
                MetricDate,
                PeopleIn,
                PeopleOut,
                PeakOccupancy,
                AvgOccupancy
            FROM dbo.StoreDailyMetrics
            WHERE StoreId = @StoreId
              AND MetricDate BETWEEN @FromDate AND @ToDate
            ORDER BY MetricDate;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<StoreDailyMetric>(
            sql,
            new
            {
                StoreId = storeId,
                FromDate = fromDate,
                ToDate = toDate
            });
    }
}