namespace Kron.Counting.Application.DTOs.Responses;

public sealed class StoreHourlyMetricDto
{
    public Guid StoreId { get; set; }

    public DateOnly MetricDate { get; set; }

    public byte MetricHour { get; set; }

    public int PeopleIn { get; set; }

    public int PeopleOut { get; set; }

    public int PeakOccupancy { get; set; }

    public decimal AvgOccupancy { get; set; }
}