namespace Kron.Counting.Application.DTOs.Requests;

public sealed class IngestCounterSnapshotRequestDto
{
    public string SerialNumber { get; set; } = default!;

    public DateTime ReadingTimestampUtc { get; set; }

    public int TotalIn { get; set; }

    public int TotalOut { get; set; }
}