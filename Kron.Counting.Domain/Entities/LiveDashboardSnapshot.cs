namespace Kron.Counting.Domain.Entities;

public sealed class LiveDashboardSnapshot
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public int CurrentOccupancy { get; set; }

    public int TodayIn { get; set; }

    public int TodayOut { get; set; }

    public DateTime? LastReadingAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Store? Store { get; set; }
}