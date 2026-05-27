namespace Kron.Counting.Domain.Entities;

public sealed class StoreDailyMetric
{
    public long Id { get; set; }

    public Guid StoreId { get; set; }

    public DateOnly MetricDate { get; set; }

    public int PeopleIn { get; set; }

    public int PeopleOut { get; set; }

    public int PeakOccupancy { get; set; }

    public decimal AvgOccupancy { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public Store? Store { get; set; }
}