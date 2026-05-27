namespace Kron.Counting.Application.DTOs.Responses;

public sealed class DashboardSnapshotDto
{
    public Guid StoreId { get; set; }

    public int CurrentOccupancy { get; set; }

    public int TodayIn { get; set; }

    public int TodayOut { get; set; }

    public DateTime? LastReadingAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}