using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Interfaces;

public interface IDashboardRepository
{
    Task<LiveDashboardSnapshot?> GetSnapshotByStoreIdAsync(
        Guid storeId,
        CancellationToken cancellationToken = default);

    Task UpsertSnapshotAsync(
        LiveDashboardSnapshot snapshot,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<StoreHourlyMetric>> GetHourlyMetricsAsync(
        Guid storeId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<StoreDailyMetric>> GetDailyMetricsAsync(
        Guid storeId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default);
}