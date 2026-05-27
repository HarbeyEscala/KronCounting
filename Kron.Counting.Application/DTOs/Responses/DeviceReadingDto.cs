namespace Kron.Counting.Application.DTOs.Responses;

public sealed class DeviceReadingDto
{
    public long Id { get; set; }

    public Guid DeviceId { get; set; }

    public DateTime ReadingTimestampUtc { get; set; }

    public int PeopleIn { get; set; }

    public int PeopleOut { get; set; }

    public int Occupancy { get; set; }

    public decimal? ConfidenceScore { get; set; }

    public string? RawPayloadJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}